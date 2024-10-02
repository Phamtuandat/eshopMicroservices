namespace Catalog.API.DTOs
{
    public record StoreImageDTO
    {
        public Guid Id { get; }
        public IFormFile ImageFile { get; } 
        public DateTime UploadDate { get; } = DateTime.Now;
        public string Title { get; }

        public StoreImageDTO(Guid id, IFormFile imageFile, string title)
        {
            Id = id;
            ImageFile = imageFile;
            Title = title;
        }

        public override string ToString()
        {
            return $"StoreImageDTO {{ Id = {Id}, ImageFile = {(ImageFile != null ? ImageFile.FileName : "null")}, UploadDate = {UploadDate}}}";
        }
    }
}
