namespace Catalog.API
{
    public class CatalogSettings
    {
        public string PictureBaseUrl { get; set; } = default!;

        public string EventBusConnection { get; set; } = default!;  

        public bool UseCustomizationData { get; set; }

        public bool IsAzureStorageEnabled { get; set; }
    }
}
