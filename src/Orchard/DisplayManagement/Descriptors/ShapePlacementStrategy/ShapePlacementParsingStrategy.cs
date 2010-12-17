﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Orchard.Environment.Descriptor.Models;
using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;

namespace Orchard.DisplayManagement.Descriptors.ShapePlacementStrategy {
    /// <summary>
    /// This component discovers and announces the shape alterations implied by the contents of the Placement.info files
    /// </summary>
    public class ShapePlacementParsingStrategy : IShapeTableProvider {
        private readonly IExtensionManager _extensionManager;
        private readonly ShellDescriptor _shellDescriptor;
        private readonly IPlacementFileParser _placementFileParser;

        public ShapePlacementParsingStrategy(
            IExtensionManager extensionManager,
            ShellDescriptor shellDescriptor,
            IPlacementFileParser placementFileParser) {
            _extensionManager = extensionManager;
            _shellDescriptor = shellDescriptor;
            _placementFileParser = placementFileParser;
        }

        public void Discover(ShapeTableBuilder builder) {

            var availableFeatures = _extensionManager.AvailableFeatures();
            var activeFeatures = availableFeatures.Where(fd => FeatureIsTheme(fd) || FeatureIsEnabled(fd));
            var activeExtensions = Once(activeFeatures);

            foreach (var extensionDescriptor in activeExtensions) {
                foreach (var featureDescriptor in extensionDescriptor.Features.Where(fd => fd.Id == fd.Extension.Id)) {
                    ProcessFeatureDescriptor(builder, featureDescriptor);
                }
            }
        }

        private void ProcessFeatureDescriptor(ShapeTableBuilder builder, FeatureDescriptor featureDescriptor) {
            var virtualPath = featureDescriptor.Extension.Location + "/" + featureDescriptor.Extension.Id + "/Placement.info";
            var placementFile = _placementFileParser.Parse(virtualPath);
            if (placementFile != null) {
                ProcessPlacementFile(builder, featureDescriptor, placementFile);
            }
        }

        private void ProcessPlacementFile(ShapeTableBuilder builder, FeatureDescriptor featureDescriptor, PlacementFile placementFile) {
            var feature = new Feature { Descriptor = featureDescriptor };

            // invert the tree into a list of leaves and the stack
            var entries = DrillDownShapeLocations(placementFile.Nodes, Enumerable.Empty<PlacementMatch>());
            foreach (var entry in entries) {
                var shapeLocation = entry.Item1;
                var matches = entry.Item2;

                string shapeType;
                string differentiator;
                GetShapeType(shapeLocation, out shapeType, out differentiator);

                Func<ShapePlacementContext, bool> predicate = ctx => true;
                if (differentiator != "") {
                    predicate = ctx => (ctx.Differentiator ?? "") == differentiator;
                }

                if (matches.Any()) {
                    predicate = matches.SelectMany(match => match.Terms).Aggregate(predicate, BuildPredicate);
                }

                builder.Describe(shapeType)
                    .From(feature)
                    .Placement(predicate, shapeLocation.Location);
            }
        }

        private void GetShapeType(PlacementShapeLocation shapeLocation, out string shapeType, out string differentiator) {
            differentiator = "";
            shapeType = shapeLocation.ShapeType;
            var dashIndex = shapeType.LastIndexOf('-');
            if (dashIndex > 0 && dashIndex < shapeType.Length - 1) {
                differentiator = shapeType.Substring(dashIndex + 1);
                shapeType = shapeType.Substring(0, dashIndex);
            }
        }

        private Func<ShapePlacementContext, bool> BuildPredicate(Func<ShapePlacementContext, bool> predicate, KeyValuePair<string, string> term) {
            var expression = term.Value;
            switch (term.Key) {
                case "ContentType":
                    if (expression.EndsWith("*")) {
                        var prefix = expression.Substring(0, expression.Length - 1);
                        return ctx => (ctx.ContentType ?? "").StartsWith(prefix) && predicate(ctx);
                    }
                    return ctx => (ctx.ContentType == expression) && predicate(ctx);
                case "DisplayType":
                    if (expression.EndsWith("*")) {
                        var prefix = expression.Substring(0, expression.Length - 1);
                        return ctx => (ctx.DisplayType ?? "").StartsWith(prefix) && predicate(ctx);
                    }
                    return ctx => (ctx.DisplayType == expression) && predicate(ctx);
            }
            return predicate;
        }


        private static IEnumerable<Tuple<PlacementShapeLocation, IEnumerable<PlacementMatch>>> DrillDownShapeLocations(
            IEnumerable<PlacementNode> nodes,
            IEnumerable<PlacementMatch> path) {

            // return shape locations nodes in this place
            foreach (var placementShapeLocation in nodes.OfType<PlacementShapeLocation>()) {
                yield return new Tuple<PlacementShapeLocation, IEnumerable<PlacementMatch>>(placementShapeLocation, path);
            }
            // recurse down into match nodes
            foreach (var placementMatch in nodes.OfType<PlacementMatch>()) {
                foreach (var findShapeLocation in DrillDownShapeLocations(placementMatch.Nodes, path.Concat(new[] { placementMatch }))) {
                    yield return findShapeLocation;
                }
            }
        }

        private bool FeatureIsTheme(FeatureDescriptor fd) {
            return DefaultExtensionTypes.IsTheme(fd.Extension.ExtensionType);
        }

        private bool FeatureIsEnabled(FeatureDescriptor fd) {
            return _shellDescriptor.Features.Any(sf => sf.Name == fd.Id);
        }

        private static IEnumerable<ExtensionDescriptor> Once(IEnumerable<FeatureDescriptor> featureDescriptors) {
            var once = new ConcurrentDictionary<string, object>();
            return featureDescriptors.Select(fd => fd.Extension).Where(ed => once.TryAdd(ed.Id, null)).ToList();
        }

    }
}