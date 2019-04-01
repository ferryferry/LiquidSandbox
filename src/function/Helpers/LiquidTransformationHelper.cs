using System;
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
            // Check if the json is a plain array
            if (json.StartsWith("[") && !json.Contains("{"))
            {
                return TransformJsonArray<List<object>>(json, liquidTemplate);
            }
            
            // Check if the json string is a json array
            if (json.StartsWith("["))
            {
                return TransformJsonArray<List<Dictionary<string, object>>>(json, liquidTemplate);
            }

            // If the json is a normal object transform the json object
            return TransformJsonObject(json, liquidTemplate);
        }

        private static string TransformJsonArray<T>(string jsonArray, string liquidTemplate)
        {
            var deserializedList =
                JsonConvert.DeserializeObject<T>(jsonArray, new DictionaryConverter());

            var item = new { content = deserializedList };
            var hash = Hash.FromAnonymousObject(item);

            return RenderTemplate(liquidTemplate, hash);
        }

        private static string TransformJsonObject(string jsonObject, string liquidTemplate)
        {
            var deserializedObject =
                JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObject, new DictionaryConverter());

            var hash = Hash.FromDictionary(deserializedObject);

            return RenderTemplate(liquidTemplate, hash);
        }

        private static string RenderTemplate(string liquidTemplate, Hash hash) 
        {
            var template = Template.Parse(liquidTemplate);
            Template.RegisterFilter(typeof(ExpirationCalculationFilter));
            Template.RegisterFilter(typeof(Alpha2CountryCodeFilter));
            return template.Render(hash);
        }
    }
}