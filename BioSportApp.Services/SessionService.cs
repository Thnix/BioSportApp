
using BioSportApp.Domain.Core;

namespace BioSportApp.Services
{
    public class SessionService
    {
        public User? CurrentUser { get; private set; }

        public SessionService()
        {

        }

        public void Login(User user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}
