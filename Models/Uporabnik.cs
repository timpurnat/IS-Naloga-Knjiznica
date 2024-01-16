
namespace web.Models
{
    public class Uporabnik
    {
        public int UporabnikId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get;set; }
        public String? PriimekIme{get{
            return LastName + " " +FirstName.Substring(0,1)+".";
        }}
        
        public ICollection<Rezervacija>? Rezervacije { get; set; }
        

    }
}