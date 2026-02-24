namespace BookStoreAPI.DTOs
{
    public class CategoryResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookInCategoryDTO> Books { get; set; } = new List<BookInCategoryDTO>();
    }
}
