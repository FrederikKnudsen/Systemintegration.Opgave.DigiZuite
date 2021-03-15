using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Systemintegration.Opgave.DigiZuite.Utilities
{
    public class RoutingKey
    {
        private Dictionary<string, string> keyValuePairs;
        public RoutingKey()
        {
            keyValuePairs = new Dictionary<string, string>()
            {
                {".mov", "movie.mov"},
                {".mp4", "movie.mp4"},
                {".avi", "movie.avi"},
                {".png", "picture.png"},
                {".jpeg", "picture.jpeg"}
            };
        }
        public string GetRoutingKey(string type)
        {
            if (!keyValuePairs.ContainsKey(type))
                return "notsupported";

            return keyValuePairs[type];
        }

        public List<string> GetTypes()
        {
            return keyValuePairs.Keys.ToList();
        }
    }
}
