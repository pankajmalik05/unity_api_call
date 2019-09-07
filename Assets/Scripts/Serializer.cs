using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

// class to serialize response in json object and map to classes 
public class Serializer
{
    public static JsonSerializerSettings FULL_SETTINGS;

    static Serializer()
    {
        FULL_SETTINGS = new JsonSerializerSettings();
        FULL_SETTINGS.TypeNameHandling = TypeNameHandling.All;
        FULL_SETTINGS.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
    }

    public static T Deserialize<T>(string objectStr)
    {
        return JsonConvert.DeserializeObject<T>(objectStr);
    }

    public static string Serialize(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public static string Serialize(object obj, bool ignoreNull)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include;

        return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
    }

    public static string Serialize(object obj, bool ignoreNull, bool includeType)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.TypeNameHandling = includeType ? TypeNameHandling.All : TypeNameHandling.None;
        settings.NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include;

        return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
    }

    public static string Serialize(object obj, JsonSerializerSettings settings)
    {
        return JsonConvert.SerializeObject(obj, settings);
    }

    public static T Deserialize<T>(string json, JsonSerializerSettings settings)
    {
        return JsonConvert.DeserializeObject<T>(json, settings);
    }


    public static T Deserialize<T>(string json, bool ignoreNull)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include;

        return JsonConvert.DeserializeObject<T>(json, settings);
    }

}
