using System.Collections.Generic;
using DotLiquid;
using Newtonsoft.Json;

namespace MasterData.Repositories.Helpers
{
    public static class LiquidTransformationHelper
    {
        /// <summary>
        /// Transforms the given Json (can be a json object or a json array) to the given liquid template
        /// If provided a json array, the top level object for loops etc. is called "content".
        /// Example: {% for item in content %}
        /// </summary>
        /// <param name="json">A json object or array</param>
        /// <param name="liquidTemplate">A liquid template</param>
        /// <returns>Transformed string</returns>
        public static string Transform(string json, string liquidTemplate)
        {
            // Check if the jsonArray string is a jsonArray array
            if (json.StartsWith("["))
            {
                return TransformJsonArray(json, liquidTemplate);
            }

            // If the json is a normal object transform the json object
            return TransformJsonObject(json, liquidTemplate);
        }

        private static string TransformJsonArray(string jsonArray, string liquidTemplate)
        {
            var deserializedList =
                JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonArray, new DictionaryConverter());
            
            var item = new {content = deserializedList};
            
            var templateExample2 = Template.Parse(liquidTemplate);
            return templateExample2.Render(Hash.FromAnonymousObject(item));
        }

        private static string TransformJsonObject(string jsonObject, string liquidTemplate)
        {
            var deserializedObject =
                JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObject, new DictionaryConverter());
            
            var templateExample1 = Template.Parse(liquidTemplate);
            return templateExample1.Render(Hash.FromDictionary(deserializedObject));
        }
    }
}