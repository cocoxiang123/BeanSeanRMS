using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ReservationSystem.Models;
using T4RMSSolution.Models;

namespace ReservationSystem.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Sitting> Sittings { get; set; }
     
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<ReservationType> ReservationTypes { get; set; }

        public DbSet<SittingType> SittingTypes { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }
       public DbSet<Customer> Customers { get; set; }
       public DbSet<Table> Tables { get; set; }
       public DbSet<ReservationStatus> ReservationStatuses { get; set; }

    }
}


        
