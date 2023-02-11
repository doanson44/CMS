using CMS.Core;
using CMS.Core.Enums;
using CMS.Core.Extensions;
using CMS.Core.Settings;
using CMS.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CMS.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BaseController : Controller
    {
        [DebuggerStepThrough]
        protected IActionResult Success()
        {
            return Ok(new BaseResponseModel
            {
                Code = ErrorCodes.Success,
                Message = ErrorCodes.Success.GetDescription()
            });
        }

        [DebuggerStepThrough]
        protected IActionResult Success<T>(T data)
        {
            return Ok(new BaseResponseModel<T>
            {
                Code = ErrorCodes.Success,
                Message = ErrorCodes.Success.GetDescription(),
                Data = data
            });
        }

        [DebuggerStepThrough]
        protected IActionResult Success<T>(ResponseContext<T> context)
        {
            return Ok(new BaseResponseModel<T>
            {
                Code = ErrorCodes.Success,
                Message = context.Message,
                Data = context.Data
            });
        }

        [DebuggerStepThrough]
        protected IActionResult Success<T>(string message)
        {
            return Ok(new BaseResponseModel<T>
            {
                Code = ErrorCodes.Success,
                Message = message,
            });
        }

        [DebuggerStepThrough]
        protected IActionResult Failure()
        {
            return Ok(new BaseResponseModel
            {
                Code = ErrorCodes.BadRequest,
                Message = ErrorCodes.BadRequest.GetDescription()
            });
        }

        [DebuggerStepThrough]
        protected IActionResult Failure(string message)
        {
            return Ok(new BaseResponseModel
            {
                Code = ErrorCodes.BadRequest,
                Message = message
            });
        }

        [DebuggerStepThrough]
        protected IActionResult Failure(ErrorCodes errorCode)
        {
            return Ok(new BaseResponseModel
            {
                Code = errorCode,
                Message = errorCode.GetDescription()
            });
        }

        [DebuggerStepThrough]
        protected IActionResult Failure(ErrorCodes errorCode, string message)
        {
            return Ok(new BaseResponseModel
            {
                Code = errorCode,
                Message = message
            });
        }

        [DebuggerStepThrough]
        protected IActionResult Done<T>(ResponseContext<T> context)
        {
            if (context.IsSuccess)
            {
                return Success(context);
            }

            if (!string.IsNullOrEmpty(context.Message))
            {
                return Failure(context.StatusCode, context.Message);
            }
            return Failure(context.StatusCode);
        }

        [DebuggerStepThrough]
        protected IActionResult InvalidModel()
        {
            return base.Ok(new BaseResponseModel
            {
                Code = ErrorCodes.InvalidModel,
                Message = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)))
            });
        }


        [DebuggerStepThrough]
        protected string GetCurrentFullName()
        {
            if (User != null)
                return User.FindFirst("FullName").Value;

            return string.Empty;
        }

        [DebuggerStepThrough]
        protected string GetCurrentUserName()
        {
            if (User != null)
                return User.FindFirst(JwtRegisteredClaimNames.UniqueName).Value;

            return string.Empty;
        }

        [DebuggerStepThrough]
        protected int GetCurrentUserId()
        {
            var userId = int.Parse(User?.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);
            return userId;
        }

        protected Guid CurrentUserId
        {
            get
            {
                if (Guid.TryParse(GetCurrentUserId().ToString(), out var id))
                    return id;

                return Guid.Empty; // anynomous action
            }
        }

        [DebuggerStepThrough]
        protected string GetCurrentEmployeeId(ClaimsPrincipal principal = null)
        {
            var empId = string.Empty;
            if (principal != null)
            {
                empId = principal.FindFirst(ClaimTypes.Name)?.Value;
            }
            else if (User != null)
            {
                empId = User.FindFirst(ClaimTypes.Name)?.Value;
            }

            return empId;
        }

        [DebuggerStepThrough]
        protected RequestContext<T> CreateContext<T>(T data)
        {
            return new RequestContext<T>(data, CurrentUserId, HttpContext.RequestAborted);
        }

        protected static bool ValidateToken(string token, JwtTokenSetting tokenSetting, out ClaimsPrincipal principal)
        {
            principal = null;
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = tokenSetting.Issuer,
                ValidateAudience = true,
                ValidAudience = tokenSetting.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSetting.SecretKey))
            };

            var validator = new JwtSecurityTokenHandler();
            if (!validator.CanReadToken(token))
                return false;

            try
            {
                principal = validator.ValidateToken(token, parameters, out var securityToken);
            }
            catch (SecurityTokenException) { }

            return principal != null;
        }

        protected static string GetMediaContentType(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var extension = fileInfo.Extension.ToLower().Replace(".", "");
            switch (extension)
            {
                case "pdf":
                    return "application/pdf";
                case "jpg":
                case "png":
                case "jpeg":
                    return $"image/{extension}";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
