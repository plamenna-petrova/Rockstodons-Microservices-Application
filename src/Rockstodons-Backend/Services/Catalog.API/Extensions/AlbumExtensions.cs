using Catalog.API.Data.Models;

namespace Catalog.API.Controllers
{
    public static class AlbumExtensions
    {
        public static void FillAlbumUrl(this Album album, string pictureBaseUrl, bool isAzureStorageEnabled)
        {
            if (album != null)
            {
                album.PictureUrl = isAzureStorageEnabled
                    ? pictureBaseUrl + album.PictureFileName
                    : pictureBaseUrl.Replace("[0]", album.Id.ToString());
            }
        }
    }
}
