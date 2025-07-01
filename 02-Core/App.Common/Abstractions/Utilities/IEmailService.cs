using System.Threading;
using System.Threading.Tasks;
using App.Common.Models;

namespace App.Common.Abstractions.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SendEmailAsync(EmailOptionsDto emailOptions, CancellationToken cancellationToken = default);
    }
}
