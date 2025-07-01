using System.Threading.Tasks;

namespace App.Common.Abstractions.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPdfDocumentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headercontent"></param>
        /// <param name="bodycontent"></param>
        /// <param name="footercontent"></param>
        /// <returns></returns>
        Task<byte[]> GeneratePdf(string headercontent, string bodycontent, string footercontent);
    }
}
