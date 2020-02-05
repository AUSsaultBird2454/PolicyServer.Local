using System;
using Microsoft.EntityFrameworkCore;
using PolicyServer.Runtime.Client;

namespace PolicyServer.Local
{
    public class PolicyDatabaseOptions
    {
        /// <summary>
        /// Callback to configure the EF DbContext.
        /// </summary>
        /// <value>
        /// The configure database context.
        /// </value>
        public Action<DbContextOptionsBuilder<PolicyDatabaseContext>> ConfigureDbContext { get; set; }
    }
}