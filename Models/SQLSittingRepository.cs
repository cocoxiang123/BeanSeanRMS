
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystem.Models
{
    public class SQLSittingRepository : ISittingRepository
    {
        private readonly ApplicationDbContext context;

        public SQLSittingRepository(ApplicationDbContext context)
        {
            this.context = context;

        }
        public void AddSitting(Sitting sitting)
        {
            var days = (sitting.End - sitting.Start).TotalDays;
            for (var i = 0; i < days ; i++)
            {
                DateTime newDateTime = new DateTime(sitting.Start.AddDays(i).Year, sitting.Start.AddDays(i).Month, sitting.Start.AddDays(i).Day) + sitting.End.TimeOfDay;
                var newsitting = new Sitting()
                {
                    Start = sitting.Start.AddDays(i),         
                    End = newDateTime,
                    Capacity = sitting.Capacity,
                    SittingTypeId = sitting.SittingTypeId,
                    SittingType = sitting.SittingType,
                    Status = sitting.Status
                };
                context.Sittings.Add(newsitting);
                context.SaveChanges();
            }                              
            }

         

        public Sitting Delete(Sitting sitting)
        {

                context.Sittings.Remove(sitting);
                context.SaveChanges();
            
            return sitting;
        }

        public List<Sitting> GetAllSittings()
        {   
            return context.Sittings.ToList();
        }

        public Sitting GetSitting(int sittingId)
        {
            var sitting = GetSittingWithType().Find(e => e.SittingId == sittingId);
            return sitting;
        }

        public Sitting Update(Sitting updateSitting)


        { 
            var _sitting = context.Sittings.Attach(updateSitting);
            _sitting.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return updateSitting;
        }
        public List<Sitting> GetSittingWithType()


        {
            var sittings = GetAllSittings();
            foreach (Sitting sitting in sittings)
            {
                var id = sitting.SittingTypeId;
                var SittingTypes = context.SittingTypes.ToList();

                foreach (SittingType sittingType in SittingTypes)
                {
                    if (sitting.SittingTypeId == sittingType.Id)
                    {
                        sitting.SittingType = sittingType;
                    }
                }

            }
            //order sitting list in date order
            sittings = sittings.OrderBy(e => e.Start).ToList();

            return sittings;
        }

        public void DeleteSittingItem(int sittingTypeId)
        {
            var sittings = GetAllSittings().Where(x => x.SittingTypeId == sittingTypeId).ToList();
            foreach(Sitting sitting in sittings)
            {
                context.Sittings.Remove(sitting);
            }

            context.SaveChanges();
        }

        public bool ValidateDate(Sitting a)
        {
            var sittingItems = GetSittingWithType().Where(e => e.Start.Date == DateTime.Now.Date).ToList();
            var validateItem = true;

            foreach (var s in sittingItems)
            {

                if (a.End <= a.Start ||a.Start >= s.End)
                {
                    return validateItem;
                }
                else
                {
                    validateItem = false;
                    return validateItem;
                }
            }
            return validateItem;
        }
    }
}
