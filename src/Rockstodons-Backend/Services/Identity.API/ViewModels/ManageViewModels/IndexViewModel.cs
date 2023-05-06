using Microsoft.AspNetCore.Identity;

namespace Identity.API.ViewModels.ManageViewModels
{
    public class IndexViewModel
    {
        public bool HasPassword { get; init; }

        public IList<UserLoginInfo> Logins { get; init; }

        public bool TwoFactor { get; init; }
         
        public bool BrowserRemembered { get; init; }
    }
}
