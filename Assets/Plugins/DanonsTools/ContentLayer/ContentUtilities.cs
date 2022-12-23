using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DanonsTools.ContentLayer
{
    public static class ContentUtilities
    {
        public static IEnumerable<string> GetAssetAddressesInType(in Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string) x.GetRawConstantValue())
                .ToArray();
        }
    }
}