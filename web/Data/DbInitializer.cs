using web.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

         

            var zvrsti = new Zvrst[]
            {
                new Zvrst { ImeZvrsti = "Tragedija" },
                new Zvrst { ImeZvrsti = "Drama" },
                new Zvrst {ImeZvrsti = "Komedija" },
                new Zvrst { ImeZvrsti = "Romani" },
                new Zvrst { ImeZvrsti = "Povest" },
            };

            foreach (var zvrst in zvrsti)
            {
                context.Zvrsti.Add(zvrst);
            }
context.SaveChanges();
            var avtorji = new Avtor[]
            {
                new Avtor { Ime = "Primoz", Priimek = "Jurman" },
                new Avtor { Ime = "France",  Priimek = "Preseren"},
                new Avtor { Ime = "Franc" ,  Priimek = "Kafka"},
                new Avtor { Ime = "Janez",   Priimek = "Jansa"},
                new Avtor { Ime = "Rockstar",   Priimek = "Games"}
                
            };

            foreach (var avtor in avtorji)
            {
                context.Avtorji.Add(avtor);
            }
context.SaveChanges();
            var kategorije = new Kategorija[]
            {
                 new Kategorija { imeKategorije = "Knjiga" },
                 new Kategorija { imeKategorije = "CD/DVD" },
                 new Kategorija { imeKategorije = "Strip" },
                 new Kategorija { imeKategorije = "Igra" },
                 new Kategorija { imeKategorije = "Digitalno" },
             };


             foreach (var kategorij in kategorije)
            {
                 context.Kategorija.Add(kategorij);
             }

             context.SaveChanges();


                

            //knjiga
            if (context.Knjige.Any())
            {
                return;   // DB has been seeded
            }

            var knjige = new Knjiga[]
            {
                new  Knjiga{ Naslov = "The Catcher in the Rye", ZvrstID = 1, AvtorID = 1, Ocena = 4, KategorijaID = 1},
                new  Knjiga{  Naslov = "To Kill a Mockingbird", ZvrstID = 3, AvtorID = 2, Ocena = 5, KategorijaID = 1 },
                new  Knjiga{ Naslov = "The Great Gatsby", ZvrstID = 4, AvtorID = 4, Ocena = 3, KategorijaID = 1 },
                new  Knjiga{ Naslov = "The Da Vinci Code", ZvrstID = 2, AvtorID = 1, Ocena = 5, KategorijaID = 1 },
                new  Knjiga{  Naslov = "The Alchemist", ZvrstID = 1, AvtorID = 5, Ocena = 6 , KategorijaID = 1},
                new  Knjiga{ Naslov = "The Girl with the Dragon Tattoo", ZvrstID = 5, AvtorID = 1, Ocena = 7, KategorijaID = 1 },
                new  Knjiga{  Naslov = "Grand teft auto V", ZvrstID = 2, AvtorID = 5, Ocena = 4 , KategorijaID = 4},
                new  Knjiga{ Naslov = "The Hunger Games", ZvrstID = 2, AvtorID = 3, Ocena = 8 , KategorijaID = 2}
               
            };

            foreach (var izdelki in knjige)
            {
                context.Knjige.Add(izdelki);
            }
            context.SaveChanges();
var roles = new IdentityRole[] {
                new IdentityRole{Id="1", Name="Administrator"},
                new IdentityRole{Id="2", Name="Manager"},
            };
        
            foreach (IdentityRole r in roles)
            {
                context.Roles.Add(r);
            }

            var user = new ApplicationUser
            {
                FirstName = "Bob",
                LastName = "Dilon",
                City = "Ljubljana",
                Email = "timpurnat@gmail.com",
                NormalizedEmail = "timpurnat@gmail.com",
                UserName = "timpurnat@gmail.com",
                NormalizedUserName = "timpurnat@gmail.com",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user,"Testni123!");
                user.PasswordHash = hashed;
                context.Users.Add(user);

            }

            context.SaveChanges();


            var UserRoles = new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>{RoleId = roles[0].Id, UserId=user.Id},
                new IdentityUserRole<string>{RoleId = roles[1].Id, UserId=user.Id},
            };

            foreach (IdentityUserRole<string> r in UserRoles)
            {
                context.UserRoles.Add(r);
            }


       

            context.SaveChanges();
        }
    }
}
