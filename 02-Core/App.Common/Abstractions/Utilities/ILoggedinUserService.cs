namespace App.Common.Abstractions.Utilities
{
    public interface ILoggedinUserService
    {
        long UserId { get; }
        string IpAddress { get; }
    }
}
