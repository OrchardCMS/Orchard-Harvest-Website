﻿using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Shapes;

namespace Orchard.ContentManagement.Drivers {
    public abstract class ContentFieldDriver<TField> : IContentFieldDriver where TField : ContentField, new() {
        protected virtual string Prefix { get { return ""; } }
        protected virtual string Zone { get { return "Content"; } }

        DriverResult IContentFieldDriver.BuildDisplayShape(BuildDisplayContext context) {
            return Process(context.ContentItem, (part, field) => Display(part, field, context.DisplayType, context.New));
        }

        DriverResult IContentFieldDriver.BuildEditorShape(BuildEditorContext context) {
            return Process(context.ContentItem, (part, field) => Editor(part, field, context.New));
        }

        DriverResult IContentFieldDriver.UpdateEditorShape(UpdateEditorContext context) {
            return Process(context.ContentItem, (part, field) => Editor(part, field, context.Updater, context.New));
        }

        DriverResult Process(ContentItem item, Func<ContentPart, TField, DriverResult> effort) {
            var results = item.Parts
                    .SelectMany(part => part.Fields.OfType<TField>().Select(field => new { part, field }))
                    .Select(pf => effort(pf.part, pf.field));
            return Combined(results.ToArray());
        }

        public IEnumerable<ContentFieldInfo> GetFieldInfo() {
            var contentFieldInfo = new[] {
                new ContentFieldInfo {
                    FieldTypeName = typeof (TField).Name,
                    Factory = (partFieldDefinition, storage) => new TField {
                        PartFieldDefinition = partFieldDefinition,
                        Storage = storage,
                    }
                }
            };

            return contentFieldInfo;
        }


        protected virtual DriverResult Display(ContentPart part, TField field, string displayType, dynamic shapeHelper) { return null; }
        protected virtual DriverResult Editor(ContentPart part, TField field, dynamic shapeHelper) { return null; }
        protected virtual DriverResult Editor(ContentPart part, TField field, IUpdateModel updater, dynamic shapeHelper) { return null; }

        public ContentShapeResult ContentShape(string shapeType, Func<dynamic> factory) {
            return ContentShapeImplementation(shapeType, null, ctx => factory());
        }

        public ContentShapeResult ContentShape(string shapeType, string differentiator, Func<dynamic> factory) {
            return ContentShapeImplementation(shapeType, differentiator, ctx => factory());
        }

        public ContentShapeResult ContentShape(string shapeType, Func<dynamic, dynamic> factory) {
            return ContentShapeImplementation(shapeType, null, ctx => factory(CreateShape(ctx, shapeType)));
        }

        public ContentShapeResult ContentShape(string shapeType, string differentiator, Func<dynamic, dynamic> factory) {
            return ContentShapeImplementation(shapeType, differentiator, ctx => factory(CreateShape(ctx, shapeType)));
        }

        private ContentShapeResult ContentShapeImplementation(string shapeType, string differentiator, Func<BuildShapeContext, object> shapeBuilder) {
            return new ContentShapeResult(shapeType, Prefix, ctx => AddAlternates(shapeBuilder(ctx), differentiator)).Differentiator(differentiator);
        }

        private object AddAlternates(dynamic shape, string differentiator) {
            // automatically add shape alternates for shapes added by fields
            // [ShapeType__FieldName] for ShapeType-FieldName.cshtml templates
            // [ShapeType__PartName] for ShapeType-PartName.cshtml templates
            // [ShapeType__PartName__FieldName] for ShapeType-PartName-FieldName.cshtml templates

            // for fields on dynamic parts the part name is the same as the content type name
            // ex. Fields/Common.Text-Something.FirstName

            ShapeMetadata metadata = shape.Metadata;
            if (!string.IsNullOrEmpty(differentiator))
                metadata.Alternates.Add(metadata.Type + "__" + differentiator);

            ContentPart part = shape.ContentPart;
            if (part != null) {
                metadata.Alternates.Add(metadata.Type + "__" + part.PartDefinition.Name);
                if (!string.IsNullOrEmpty(differentiator))
                    metadata.Alternates.Add(metadata.Type + "__" + part.PartDefinition.Name + "__" + differentiator);
            }

            return shape;
        }

        private object CreateShape(BuildShapeContext context, string shapeType) {
            IShapeFactory shapeFactory = context.New;
            return shapeFactory.Create(shapeType);
        }

        [Obsolete]
        public ContentTemplateResult ContentFieldTemplate(object model) {
            return new ContentTemplateResult(model, null, Prefix).Location(Zone);
        }
        [Obsolete]
        public ContentTemplateResult ContentFieldTemplate(object model, string template) {
            return new ContentTemplateResult(model, template, Prefix).Location(Zone);
        }
        [Obsolete]
        public ContentTemplateResult ContentFieldTemplate(object model, string template, string prefix) {
            return new ContentTemplateResult(model, template, prefix).Location(Zone);
        }

        public CombinedResult Combined(params DriverResult[] results) {
            return new CombinedResult(results);
        }
    }
}