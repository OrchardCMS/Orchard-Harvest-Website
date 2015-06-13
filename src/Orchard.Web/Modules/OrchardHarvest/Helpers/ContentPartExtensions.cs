using System.Linq;
using Orchard.ContentManagement;

namespace OrchardHarvest.Helpers {
    public static class ContentPartExtensions {
        public static T Field<T>(this ContentPart part, string fieldName) where T:ContentField {
            return (T)part.Fields.Single(x => x.Name == fieldName);
        }
    }
}