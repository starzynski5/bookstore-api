namespace BookStoreAPI.DTOs
{
    public class CreateBookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string CoverLink { get; set; }
        public int CategoryId { get; set; }
    }
}
