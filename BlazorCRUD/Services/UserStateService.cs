using BlazorCRUD.Models;

namespace BlazorCRUD.Services
{
    public class UserStateService
    {
        public User CurrentUser { get; private set; }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }
    }
}
