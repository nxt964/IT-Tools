namespace ITtools_clone.Models
{
    public class Tool
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool PremiumRequired { get; set; }
        public string PathTool { get; set; }
        public int? CategoryId { get; set; }
    }
}
