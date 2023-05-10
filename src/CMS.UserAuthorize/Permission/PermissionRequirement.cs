﻿using Microsoft.AspNetCore.Authorization;

namespace CMS.IdentityUserLib.Permission;

internal class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; private set; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}