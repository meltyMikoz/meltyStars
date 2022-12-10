using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KuusouEngine
{
    public static class JsonDataHelper
    {
        public static void SaveDataAsJson(object data, string path)
        {
            string jsonPath = $"{Application.persistentDataPath}/{path}.json";
            string jsonStr = JsonConvert.SerializeObject(data);
            using (StreamWriter streamWriter = new StreamWriter(jsonPath, false, Encoding.UTF8))
            {
                streamWriter.Write(jsonStr);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        public static async Task SaveDataAsJsonAsync(object data, string path)
        {
            string jsonPath = $"{Application.persistentDataPath}/{path}.json";
            string jsonStr = JsonConvert.SerializeObject(data);
            using (StreamWriter streamWriter = new StreamWriter(jsonPath, false, Encoding.UTF8))
            {
                await streamWriter.WriteAsync(jsonStr);
                await streamWriter.FlushAsync();
                streamWriter.Close();
            }
        }
        public static TDataType GetDataFromJson<TDataType>(string path, bool fromRes)
        {
            if (fromRes)
            {
                TextAsset ta = AddressableAssetScheduler.Instance.LoadAsset<TextAsset>(path);
                return JsonConvert.DeserializeObject<TDataType>(ta.text);
            }
            string jsonPath = $"{Application.persistentDataPath}/{path}.json";
            string jsonStr;
            using (StreamReader streamReader = new StreamReader(jsonPath, Encoding.UTF8))
            {
                jsonStr = streamReader.ReadToEnd();
                streamReader.Close();
            }
            return JsonConvert.DeserializeObject<TDataType>(jsonStr);
        }
        public static async Task<TDataType> GetDataFromJsonAsync<TDataType>(string path, bool fromRes)
        {
            if (fromRes)
            {
                TextAsset ta = AddressableAssetScheduler.Instance.LoadAsset<TextAsset>(path);
                return JsonConvert.DeserializeObject<TDataType>(ta.text);
            }
            string jsonPath = $"{Application.persistentDataPath}/{path}.json";
            string jsonStr;
            using (StreamReader streamReader = new StreamReader(jsonPath, Encoding.UTF8))
            {
                jsonStr = await streamReader.ReadToEndAsync();
                streamReader.Close();
            }
            return JsonConvert.DeserializeObject<TDataType>(jsonStr);
        }
    }
}
