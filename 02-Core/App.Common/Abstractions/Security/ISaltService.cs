namespace App.Common.Abstractions.Security
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISaltService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GenerateSalt();
    }
}
