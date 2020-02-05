using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PolicyServer.Runtime.Client;

namespace PolicyServer.Local
{
    public class PolicyDatabase : IPolicy
    {
        private PolicyDatabaseContext _dbc;

        public PolicyDatabase(PolicyDatabaseContext dbc)
        {
            _dbc = dbc;
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<Role> Roles
        {
            get { return _dbc.Roles.ToList(); }
        }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public List<Permission> Permissions
        {
            get { return _dbc.Permissions.ToList(); }
        }

        public Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var roles = Roles.Where(x => x.Evaluate(user)).Select(x => x.Name).ToArray();
            var permissions = Permissions.Where(x => x.Evaluate(roles)).Select(x => x.Name).ToArray();

            var result = new PolicyResult()
            {
                Roles = roles.Distinct(),
                Permissions = permissions.Distinct()
            };

            return Task.FromResult(result);
        }
    }
}