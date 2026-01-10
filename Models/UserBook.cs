namespace BookStoreAPI.Models
{
    public class UserBook
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Progress { get; set; } // 1-100
        public DateTime LastTimeRead { get; set; }
    }
}
