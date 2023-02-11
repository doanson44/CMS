using System.Threading.Tasks;

namespace CMS.Core.Services.Interfaces;

public interface ITokenClaimsService
{
    Task<string> GetTokenAsync(string userName);
}
