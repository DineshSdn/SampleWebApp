namespace App.Common.Abstractions.Security
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISecurityHashService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltText"></param>
        /// <returns></returns>
        string HashPassword(string password, string saltText);
    }
}
