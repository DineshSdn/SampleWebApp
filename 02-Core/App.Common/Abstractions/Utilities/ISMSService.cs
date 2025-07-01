using System.Threading;
using System.Threading.Tasks;
using App.Common.Models;

namespace App.Common.Abstractions.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISMSService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SendSMSAsync(SMSOptionsDto options, CancellationToken cancellationToken = default);
    }
}
