﻿using Newtonsoft.Json;

namespace PCPartsStore.Extensions;

public static class SessionExtensions
{
    public static T? GetComplexData<T>(this ISession session, string key)
    {
        var data = session.GetString(key);
        if (data == null)
        {
            return default;
        }
        return JsonConvert.DeserializeObject<T>(data);
    }

    public static void SetComplexData(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
    
}