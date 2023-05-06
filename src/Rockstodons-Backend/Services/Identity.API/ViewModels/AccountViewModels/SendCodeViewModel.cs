using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity.API.ViewModels.AccountViewModels
{
    public class SendCodedViewModel
    {
        public string SelectedProvider { get; init; }

        public ICollection<SelectListItem> Providers { get; init; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }    
    }
}
