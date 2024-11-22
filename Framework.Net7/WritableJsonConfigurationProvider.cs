// <copyright file="WritableJsonConfigurationProvider.cs" company="Visual Software Systems Ltd.">Copyright (c) 2023 All rights reserved</copyright>

#if NET6_0_OR_GREATER && !WINDOWS
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;

namespace Vssl.Samples.Framework;

/// <summary>
/// Extends JsonConfigurationProvider to provide persistance.
/// </summary>
public class WritableJsonConfigurationProvider : JsonConfigurationProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WritableJsonConfigurationProvider"/> class
    /// </summary>
    /// <param name="source">The configuration source.</param>
    public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source)
    {
    }

    /// <summary>
    /// Override that persists the configuration.
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    public override void Set(string key, string value)
    {
        base.Set(key, value);

        string output = null;
        var fileInfo = base.Source.FileProvider.GetFileInfo(base.Source.Path);
        var fileFullPath = fileInfo.PhysicalPath;
        if (fileInfo.Exists)
        { 
            // Get Whole json file and change only passed key with passed value. It requires modification if you need to support change multi level json structure
            string json = File.ReadAllText(fileFullPath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[key] = value;
            output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
        }
        else
        {
            var index = new Dictionary<string, string>();
            index.Add(key, value);
            output = JsonConvert.SerializeObject(index, Newtonsoft.Json.Formatting.Indented);
        }

        File.WriteAllText(fileFullPath, output);
    }
}
#endif
