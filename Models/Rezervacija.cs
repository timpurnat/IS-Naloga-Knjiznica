using System.Collections;
using System.ComponentModel.DataAnnotations;


namespace web.Models
{
    public class Rezervacija
    {
        public int RezervacijaId { get; set; }
         [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy}", ApplyFormatInEditMode = true)]
        public DateTime datumPrevzema { get; set; }
         [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy}", ApplyFormatInEditMode = true)]
        public DateTime datumZapadlosti { get{return datumPrevzema.AddDays(7);} set{} }
        
        //public ICollection<Knjiga>? Knjige {get; set; }
        public Knjiga? Knjiga {get; set;}
        public int KnjigaId {get; set;}
    public DateTime? DateEdited  {get; set;}
        public DateTime? DateCreated  {get; set;}
        public ApplicationUser? Owner {get; set;}


    }
}