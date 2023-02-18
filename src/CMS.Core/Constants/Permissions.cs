using System.Collections.Generic;

namespace CMS.Core.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static class TodoModule
        {
            public const string GetAllAsync = "Permissions.Todo.GetAllAsync";
            public const string GetByIdAsync = "Permissions.Todo.GetByIdAsync";
            public const string CreateAsync = "Permissions.Todo.CreateAsync";
            public const string UpdateAsync = "Permissions.Todo.UpdateAsync";
            public const string DeleteAsync = "Permissions.Todo.DeleteAsync";
        }
    }
}
