namespace web.Models
{
    public class Zvrst
    {
        public int ZvrstID { get; set; }
        public String? ImeZvrsti { get; set; }
        public ICollection<Knjiga>? Knjige { get; set; }
    }
}