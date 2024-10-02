namespace Catalog.API.Exceptions
{
    public class ImageNotfountException : NotFoundException
    {
        public ImageNotfountException(string imageId) : base($"Can not found the image: {imageId}")
        {
        }
    }
}
