namespace web.Models
{
    public class Avtor
    {
        public int AvtorID { get; set; }
        public String? Ime { get; set; }
        public String? Priimek { get; set; }
        public String? PriimekIme{get{
            return Priimek + " " +Ime.Substring(0,1)+".";
        }}

        public ICollection<Knjiga>? Knjige { get; set; }
    }
}