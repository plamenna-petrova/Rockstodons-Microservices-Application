using System.Globalization;

namespace Catalog.API.Exceptions
{
    public class CatalogAPIException : Exception
    {
        public CatalogAPIException() : base()
        {
            
        }

        public CatalogAPIException(string message)
            : base(message) 
        {
            
        }

        public CatalogAPIException(string message, params object[] args)
            : base (string.Format(CultureInfo.CurrentCulture, message, args))
        {
            
        }

        public CatalogAPIException(string message, Exception innerException)
            : base(message, innerException) 
        {
            
        }
    }
}
