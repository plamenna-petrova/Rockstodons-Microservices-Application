namespace Identity.API.ViewModels.AccountViewModels
{
    public class ConsentInputModel
    {
        public string Button { get; init; }

        public IEnumerable<string> ScopesConsented { get; init; }

        public bool RememberConsent { get; init; }

        public string ReturnUrl { get; init; }
    }
}
