using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystem.Models
{
    public static class ModelBuilderExtensions
    {
        public static void ReservationTypeSeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReservationType>().HasData(
                new ReservationType
                {
                    Id=1,
                    Description = "Online Booking"
                },
                new ReservationType
                {
                    Id=2,
                    Description = "Walkin Booking"
                },
                new ReservationType
                {
                    Id=3,
                    Description = "Phone Booking"
                }

                );
        }
    }
}
