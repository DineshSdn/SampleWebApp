using System.Collections.Generic;
using System.Security.Claims;

namespace App.Common.Abstractions.Security
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string GenerateTokenFromClaims(IEnumerable<Claim> claims);
    }
}
