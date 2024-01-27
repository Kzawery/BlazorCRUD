using BlazorCRUD.Models;
using System.Threading.Tasks;
using static BlazorCRUD.Pages.Register;

namespace BlazorCRUD.Services
{
    public interface IUserService
    {
        Task<(bool Success, string ErrorMessage)> RegisterUser(RegisterModel model);

        Task<(bool Success, string ErrorMessage, User User)> AuthenticateUser(string email, string password);
    }
}
