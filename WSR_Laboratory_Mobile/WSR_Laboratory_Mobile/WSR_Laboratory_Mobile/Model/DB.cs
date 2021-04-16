using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WSR_Laboratory_Mobile.Model
{
    public class DB: DbContext
    {
        public DB ()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        private static DB Context;

        public static DB GetContext()
        {
            if (Context == null)
            {
                Context = new DB();
            }

            return Context;
        }

        public DbSet<News> News { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Laboratory.db");
            optionsBuilder.UseSqlite($"Filename={path}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Stream stream = GetType().Assembly.GetManifestResourceStream("WSR_Laboratory_Mobile.Import.News.json");
            StreamReader Reader = new StreamReader(stream);
            string News = Reader.ReadToEnd();
            List<News> NewsList = JsonConvert.DeserializeObject<List<News>>(News);
            int i = 1;
            NewsList.ForEach(p => {
                p.Id = i;
                i++;
            });
            modelBuilder.Entity<News>().HasData(NewsList);

            stream = GetType().Assembly.GetManifestResourceStream("WSR_Laboratory_Mobile.Import.usersMobile.txt");
            Reader = new StreamReader(stream);
            string Users = Reader.ReadToEnd();
            string[] usersArray = Users.Split('\n');
            List<User> UsersList = new List<User>();
            foreach (string user in usersArray)
            {
                string[] UserData = user.Replace("\r", "").Split('\t');
                if (UserData.Length <=0 )
                {
                    continue;
                }

                User User = new User()
                {
                    Id = int.Parse(UserData[0]),
                    Name = UserData[1],
                    Login = UserData[2],
                    Password = UserData[3]
                };

                UsersList.Add(User);
            }

            if (UsersList.Count > 0)
            {
                modelBuilder.Entity<User>().HasData(UsersList);
            }


            stream = GetType().Assembly.GetManifestResourceStream("WSR_Laboratory_Mobile.Import.services.txt");
            Reader = new StreamReader(stream, Encoding.UTF8);
            string Services = Reader.ReadToEnd();
            string[] ServicesArray = Services.Split('\n');
            List<Service> ServicesList = new List<Service>();
            foreach (string service in ServicesArray)
            {
                string[] ServiceData = service.Replace("\r", "").Split('\t');
                if (ServiceData.Length <= 0)
                {
                    continue;
                }

                Service Service = new Service()
                {
                    Id = int.Parse(ServiceData[0]),
                    Name = ServiceData[1],
                    Price = double.Parse(ServiceData[2])
                };

                ServicesList.Add(Service);
            }

            if (ServicesList.Count > 0)
            {
                modelBuilder.Entity<Service>().HasData(ServicesList);
            }
        }
    }
}
