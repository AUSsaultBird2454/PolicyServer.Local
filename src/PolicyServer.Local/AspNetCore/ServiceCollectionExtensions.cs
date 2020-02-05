// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PolicyServer.Local;
using PolicyServer.Runtime.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Helper class to configure DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the policy server client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static PolicyServerBuilder AddFilePolicyServerClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IPolicy>(configuration);
            services.AddTransient<IPolicyServerRuntimeClient, PolicyServerRuntimeClient>();
            services.AddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<PolicyFile>>().Value);

            return new PolicyServerBuilder(services);
        }
        /// <summary>
        /// Adds the policy server client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static PolicyServerBuilder AddDatabasePolicyServerClient(this IServiceCollection services)
        {
            services.AddTransient<IPolicyServerRuntimeClient, PolicyServerRuntimeClient>();
            services.AddScoped(provider => provider.GetRequiredService<PolicyDatabase>());

            return new PolicyServerBuilder(services);
        }
    }
}