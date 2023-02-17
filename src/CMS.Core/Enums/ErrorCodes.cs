using System.ComponentModel;

namespace CMS.Core.Enums
{
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

        [Description("Bạn không có quyền truy cập.")]
        InvalidPermission = 999,
    }
}
