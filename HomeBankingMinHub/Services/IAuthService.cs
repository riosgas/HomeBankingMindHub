using System.Threading.Tasks;

namespace HomeBankingMindHub.Services
{
    public interface IAuthService
    {
        Task<bool> Login(string email, string password);
        Task Logout();
        string UserAuthenticated();
    }
}
