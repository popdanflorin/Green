using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Green.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string JsonStringify (this HtmlHelper helper, object data)
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {
                    var writer = new JsonTextWriter(sw) { Formatting = Formatting.None };
                    var serializer = JsonSerializer.Create(new JsonSerializerSettings());
                    serializer.Serialize(writer, data);
                    writer.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    using (var sr = new StreamReader(ms))
                    {
                        return sr.ReadToEnd().Replace("\\", "\\\\").Replace("'", "\'");
                    }
                }
            }
        }
    }
}