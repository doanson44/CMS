using System.ComponentModel;

namespace CMS.Core.Enums;

public enum ErrorCodes
{
    [Description("Success")]
    Success = 0,

    [Description("Bad Request")]
    BadRequest = 1,

    [Description("Invalid Model")]
    InvalidModel = 2,

    [Description("Entity is archived")]
    EntityIsArchived = 3,

    [Description("Internal Server Error")]
    InternalServerError = 4,

    [Description("Data not found")]
    EntityNotFound = 5,

    [Description("Todo item not found")]
    TodoNotFound = 6,

    [Description("Todo item exits")]
    TodoExist = 7,

    [Description("Loại tin tức này đã tồn tại.")]
    CategoryNewsExist = 8,

    [Description("Không tìm thấy loại tin tức này.")]
    CategoryNewsNotFound = 9,

    [Description("Bạn không có quyền xóa loại tin tức này.")]
    NotPermissionDeleteCategoryNews = 10,

    [Description("Tin tức này không tồn tại.")]
    DetailNewsNotFound = 11,

    [Description("Bạn không có quyền truy cập.")]
    InvalidPermission = 999,
}
