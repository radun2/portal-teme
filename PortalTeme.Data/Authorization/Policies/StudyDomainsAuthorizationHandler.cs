﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using PortalTeme.Common.Authorization;
using System.Threading.Tasks;

namespace PortalTeme.Data.Authorization.Policies {
    public class StudyDomainsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement> {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement) {

            switch (requirement.Name) {
                case nameof(Operations.ViewDomains):
                    var canView = context.User.IsInRole(AuthorizationConstants.AdministratorRoleName) ||
                    context.User.IsInRole(AuthorizationConstants.ProfessorRoleName);
                    if (canView)
                        context.Succeed(requirement);
                    break;

                case nameof(Operations.EditDomains):
                    var isAdmin = context.User.IsInRole(AuthorizationConstants.AdministratorRoleName);
                    if (isAdmin)
                        context.Succeed(requirement);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
