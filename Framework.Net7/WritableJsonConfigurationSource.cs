// <copyright file="WritableJsonConfigurationSource.cs" company="Visual Software Systems Ltd.">Copyright (c) 2023 All rights reserved</copyright>

#if NET6_0_OR_GREATER && !WINDOWS

namespace Vssl.Samples.Framework;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Extends JsonConfigurationSource to provide persistance.
/// </summary>
public class WritableJsonConfigurationSource : JsonConfigurationSource
{
    /// <summary>
    /// Build with the extended provider.
    /// </summary>
    /// <param name="builder">The builder</param>
    /// <returns>The provider</returns>
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        this.EnsureDefaults(builder);
        return (IConfigurationProvider)new WritableJsonConfigurationProvider(this);
    }
}
#endif
