using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Catalog.API.DTOs.Blob;
using Catalog.API.Services.Data.Interfaces;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Catalog.API.Services.Data.Implementation
{
    public class FileStorageService: IFileStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly string _azureStorageConnectionString;
        private readonly ILogger<FileStorageService> _logger;

        public FileStorageService(IConfiguration configuration, ILogger<FileStorageService> logger)
        {
            _configuration = configuration;
            _azureStorageConnectionString = _configuration["AzureStorage:BlobConnectionString"];
            _logger = logger;
        }

        public async Task<List<BlobDTO>> GetFilesList(string azureStorageContainerName)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(
                _azureStorageConnectionString, azureStorageContainerName
            );

            List<BlobDTO> blobFiles = new();

            await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
            {
                string uri = blobContainerClient.Uri.ToString();
                var fileName = blobItem.Name;
                var fullUri = $"{uri}/{fileName}";

                blobFiles.Add(new BlobDTO
                {
                    Uri = fullUri,
                    Name = fileName,
                    ContentType = blobItem.Properties.ContentType,
                });
            }

            return blobFiles;
        }

        public async Task<BlobResponseDTO> UploadImageAsync(IFormFile blobFile, string azureStorageContainerName)
        {
            BlobResponseDTO blobResponseDTO = new();

            BlobContainerClient blobContainerClient = new BlobContainerClient(
                _azureStorageConnectionString, azureStorageContainerName
            );

            try
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobFile.FileName);

                var extension = Path.GetExtension(blobFile.FileName);
                var imageEncoder = GetImageEncoder(extension);

                using (var stream = blobFile.OpenReadStream())
                {
                    using var memoryStream = new MemoryStream();
                    using Image image = Image.Load(stream);
                    image.Mutate(i => i.Resize(320, 320));
                    image.Save(memoryStream, imageEncoder);
                    memoryStream.Position = 0;
                    var extensionForContentType = extension.Replace(".", string.Empty);
                    var blobHttpHeader = new BlobHttpHeaders 
                    { 
                        ContentType = $"image/{extensionForContentType}" 
                    };

                    await blobClient.UploadAsync(
                        memoryStream, 
                        new BlobUploadOptions { HttpHeaders = blobHttpHeader }
                    );
                }

                blobResponseDTO.Status = $"File {blobFile.FileName} uploaded successfully";
                blobResponseDTO.Error = false;
                blobResponseDTO.BlobDTO.Uri = blobClient.Uri.AbsoluteUri;
                blobResponseDTO.BlobDTO.Name = blobClient.Name;
            }
            catch (RequestFailedException requestFailedException)
               when(requestFailedException.ErrorCode == BlobErrorCode.BlobAlreadyExists) 
            {
                _logger.LogError($"File with name {blobFile.FileName} already exists in the container. " +
                    $"Set another name to store the file in the container :" +
                    $"'{azureStorageContainerName}.'");
                blobResponseDTO.Status = $"The file with name {blobFile.FileName} already exists. " +
                    $"Please use another name to store your file.";
                blobResponseDTO.Error = true;
                return blobResponseDTO;
            }
            catch (RequestFailedException requestFailedException)
            {
                _logger.LogError($"Unhandled Exception. ID: {requestFailedException.StackTrace} - " +
                    $"Message: {requestFailedException.Message}");
                blobResponseDTO.Status = $"Unexpected error: {requestFailedException.StackTrace}. " +
                    $"Check log with StackTrace ID.";
                blobResponseDTO.Error = true;
                return blobResponseDTO;
            }

            return blobResponseDTO;
        }


        public async Task<BlobResponseDTO> UploadMP3FileAsync(IFormFile blobFile, string azureStorageContainerName)
        {
            BlobResponseDTO blobResponseDTO = new();

            BlobContainerClient blobContainerClient = new BlobContainerClient(
                _azureStorageConnectionString, azureStorageContainerName
            );

            try
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobFile.FileName);

                var extension = Path.GetExtension(blobFile.FileName);

                await using (Stream? data = blobFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(data);
                }

                blobResponseDTO.Status = $"File {blobFile.FileName} uploaded successfully";
                blobResponseDTO.Error = false;
                blobResponseDTO.BlobDTO.Uri = blobClient.Uri.AbsoluteUri;
                blobResponseDTO.BlobDTO.Name = blobClient.Name;
            }
            catch (RequestFailedException requestFailedException)
               when (requestFailedException.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {blobFile.FileName} already exists in the container. " +
                    $"Set another name to store the file in the container :" +
                    $"'{azureStorageContainerName}.'");
                blobResponseDTO.Status = $"The file with name {blobFile.FileName} already exists. " +
                    $"Please use another name to store your file.";
                blobResponseDTO.Error = true;
                return blobResponseDTO;
            } 
            catch (RequestFailedException requestFailedException)
            {
                _logger.LogError($"Unhandled Exception. ID: {requestFailedException.StackTrace} - " +
                    $"Message: {requestFailedException.Message}");
                blobResponseDTO.Status = $"Unexpected error: {requestFailedException.StackTrace}. " +
                    $"Check log with StackTrace ID.";
                blobResponseDTO.Error = true;
                return blobResponseDTO;
            }

            return blobResponseDTO;
        }

        public async Task<BlobDTO> DownloadAsync(string blobFileName, string azureStorageContainerName)
        {
            BlobContainerClient blobContainerClient = new(
                _azureStorageConnectionString, azureStorageContainerName
            );

            try
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobFileName);

                if (await blobClient.ExistsAsync())
                {
                    var data = await blobClient.OpenReadAsync();
                    Stream blobContent = data;

                    var downloadableContent = await blobClient.DownloadContentAsync();

                    string fileName = blobFileName;
                    string contentType = downloadableContent.Value.Details.ContentType;

                    return new BlobDTO
                    {
                        Content = blobContent,
                        Name = fileName,
                        ContentType = contentType
                    };
                }
            }
            catch (RequestFailedException requestFailedException)
                when(requestFailedException.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                _logger.LogError($"File {blobFileName} was not found.");
            }

            return null!;
        }

        public async Task<BlobResponseDTO> DeleteAsync(string blobFileName, string azureStorageContainerName)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(
                _azureStorageConnectionString, azureStorageContainerName
            );

            BlobClient blobClient = blobContainerClient.GetBlobClient(blobFileName);

            try
            {
                await blobClient.DeleteAsync();
            }
            catch (RequestFailedException requestFailedException)
                when (requestFailedException.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                _logger.LogError($"File {blobFileName} was not found");

                return new BlobResponseDTO
                {
                    Error = true,
                    Status = $"File with name {blobFileName} not found."
                };
            }

            return new BlobResponseDTO
            {
                Error = false,
                Status = $"File: {blobFileName} has been successfully deleted."
            };
        }

        private IImageEncoder GetImageEncoder(string extension)
        {
            IImageEncoder imageEncoder = null!;
            extension = extension.Replace(".", string.Empty);
            bool isImageExtensionSupported = Regex.IsMatch(extension, "gif|png|jpe?g", RegexOptions.IgnoreCase);

            if (isImageExtensionSupported)
            {
                switch (extension.ToLower())
                {
                    case "png":
                        imageEncoder = new PngEncoder();
                        break;
                    case "jpg":
                        imageEncoder = new JpegEncoder();
                        break;
                    case "jpeg":
                        imageEncoder = new JpegEncoder();
                        break;
                    case "gif":
                        imageEncoder = new GifEncoder();
                        break;
                    default:
                        break;
                }
            }

            return imageEncoder;
        }
    }
}
