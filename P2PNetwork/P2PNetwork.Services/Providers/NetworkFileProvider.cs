using Newtonsoft.Json;
using P2PNetwork.Services.Utils;
using System;
using System.IO;

namespace P2PNetwork.Services.Providers
{
    public static class NetworkFileProvider<T>
    {
        public static T GetModel(string path)
        {
            T model;
            var content = File.ReadAllText(path);

            if (content == string.Empty || content == null)
            {
                model = Activator.CreateInstance<T>();

                File.WriteAllText(path, JsonConvert.SerializeObject(model));
            }
            else
            {
                model = JsonConvert.DeserializeObject<T>(content);
            }

            return model;
        }
        
        public static void SetModel(string path, T model)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(model));
        }
    }
}
