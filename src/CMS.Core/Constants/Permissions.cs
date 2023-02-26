using System.Collections.Generic;

namespace CMS.Core.Constants;

public static class Security
{
    public static class TodoPermission
    {
        public const string GetAllAsync = "Permissions.Todo.GetAllAsync";
        public const string GetByIdAsync = "Permissions.Todo.GetByIdAsync";
        public const string CreateAsync = "Permissions.Todo.CreateAsync";
        public const string UpdateAsync = "Permissions.Todo.UpdateAsync";
        public const string DeleteAsync = "Permissions.Todo.DeleteAsync";
    }

    public static class DetailNewsPermission
    {
        public const string GetAllAsync = "Permissions.DetailNews.GetAllAsync";
        public const string GetByIdAsync = "Permissions.DetailNews.GetByIdAsync";
        public const string CreateAsync = "Permissions.DetailNews.CreateAsync";
        public const string UpdateAsync = "Permissions.DetailNews.UpdateAsync";
        public const string DeleteAsync = "Permissions.DetailNews.DeleteAsync";
    }

    public static class CategoryNewsPermission
    {
        public const string GetAllAsync = "Permissions.CategoryNews.GetAllAsync";
        public const string GetByIdAsync = "Permissions.CategoryNews.GetByIdAsync";
        public const string CreateAsync = "Permissions.CategoryNews.CreateAsync";
        public const string UpdateAsync = "Permissions.CategoryNews.UpdateAsync";
        public const string DeleteAsync = "Permissions.CategoryNews.DeleteAsync";
    }

    public static List<string> AllPermissions = new List<string>
    {
        TodoPermission.GetAllAsync,
        TodoPermission.GetByIdAsync,
        TodoPermission.CreateAsync,
        TodoPermission.UpdateAsync,
        TodoPermission.DeleteAsync,
        DetailNewsPermission.GetAllAsync,
        DetailNewsPermission.GetByIdAsync,
        DetailNewsPermission.CreateAsync,
        DetailNewsPermission.UpdateAsync,
        DetailNewsPermission.DeleteAsync,
        CategoryNewsPermission.GetAllAsync,
        CategoryNewsPermission.GetByIdAsync,
        CategoryNewsPermission.CreateAsync,
        CategoryNewsPermission.UpdateAsync,
        CategoryNewsPermission.DeleteAsync
    };
}
