﻿using System.Collections.Generic;

namespace CMS.IdentityUserApi.ViewModels;

public class PermissionViewModel
{
    public string RoleName { get; set; }
    public IList<RoleClaimsViewModel> RoleClaims { get; set; }
}

public class RoleClaimsViewModel
{
    public string Type { get; set; }
    public string Value { get; set; }
    public bool Selected { get; set; }
}
