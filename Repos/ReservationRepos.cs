using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using T4RMSSolution.Areas.Member.ViewModels;
using T4RMSSolution.Models;

namespace ReservationSystem.Repos
{
    public class ReservationRepos : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public ReservationRepos(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task AddReservation(Reservation r)
        {
            if (r != null)
            {
                await dbContext.Reservations.AddAsync(r);
                await dbContext.SaveChangesAsync();
                await dbContext.DisposeAsync();
            }
        }

        public async Task<bool> IsDateValid(DateTime date)
        {
            var s = dbContext.Sittings;

            var ss =await s.Where(s => s.Start >= DateTime.Now.AddHours((24 - DateTime.Now.Hour)) &&
                                  s.Start <= DateTime.Now.AddDays(13).AddHours((24 - DateTime.Now.Hour)))
                .Where(s => s.Start.Date == date.Date)
                .FirstOrDefaultAsync(s => s.End.AddHours(-1).Hour >=
                    date.Hour && s.Start.Hour <= date.Hour);
            if (ss!=null)
            {
                return true;
            }

            return false;

        }

        public async Task<Sitting> SittingByDate(DateTime date)
        {
            var s = dbContext.Sittings;

            var ss = await s.Where(s => s.Start >= DateTime.Now.AddHours((24 - DateTime.Now.Hour)) &&
                                   s.Start <= DateTime.Now.AddDays(13).AddHours((24 - DateTime.Now.Hour)))
                .Where(s => s.Start.Date == date.Date)
                .FirstOrDefaultAsync(s => s.End.AddHours(-1).Hour >=
                    date.Hour && s.Start.Hour <= date.Hour);
            return ss;
        }
      
        public async Task<IEnumerable<Sitting>> SittingCollection()
        {
            return dbContext.Sittings.OrderBy(s => s.Start);

        }

        public async Task<Sitting> Sitting(int id)
        {
            return await dbContext.Sittings.FirstOrDefaultAsync(s => s.SittingId == id);
        }
        public async Task<bool> PressTheButton(Dictionary<int, int[]> t)
        {
            if (!await dbContext.Tables.AnyAsync()) return false;
            foreach (var i in t)
            {
                var x = await Sitting(i.Key);
                if (i.Value.Length == 0 && x.Status)
                {
                    var s = new Sitting();
                    s = x;
                    s.Status = false;
                    dbContext.Attach(s);
                    await dbContext.SaveChangesAsync();
                }
                else if (!x.Status && i.Value.Length != 0)
                {
                    var s = new Sitting();
                    s = x;
                    s.Status = true;
                    dbContext.Attach(s);
                    await dbContext.SaveChangesAsync();
                }
            }
            return true;

        }
        public async Task<Dictionary<int, int[]>> TablesCollection(DateTime date)
        {
            var s = await dbContext.Sittings.Where(t => t.Start.Date == date.Date).ToArrayAsync();
            var data = new Dictionary<int, int[]>();
            for (int i = 0; i < s.Count(); i++)
            {
                var reser = dbContext.Reservations.Where(r => r.SittingId == s[i].SittingId&& r.ReservationTypeId == 1)
                    .Where(r=>r.ReservationStatusId!=1&& r.ReservationStatusId != 3&& r.ReservationStatusId != 5).ToList();
                
                data.Add(s[i].SittingId, await dbContext.Tables.Select(t => t.Id)
                    .Except(dbContext.Reservations
                        .Where(r => r.SittingId == s[i].SittingId)
                        .Where(r=>r.ReservationStatusId!=3&&r.ReservationStatusId!=5)
                        .Select(r => r.TableId)).ToArrayAsync());
            }
            return data;
        }
        public async Task<Customer> CustomerValidation(string firstName, string lastname, string phone, string email)
        {

            var c = await dbContext.Customers.Where(c => c.Email == email).FirstOrDefaultAsync();
            if (c == null)
            {
                var newC = new Customer
                {
                    FirstName = firstName,
                    LastName = lastname,
                    PhoneNumber = phone,
                    Email = email
                };
                await dbContext.Customers.AddAsync(newC);
                await dbContext.SaveChangesAsync();
                return newC;
            }

            return c;
        }

        public async Task<IEnumerable<ReservationType>> RType()
        {

            return await dbContext.ReservationTypes.AsNoTracking().ToListAsync();
        }

        public async Task<Reservation> GetReservation(int id)
        {
            var r = await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            var c = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == r.CustomerId);
            r.Customer = c;
            return r;
        }


        public async Task EditReservation(Reservation r)
        {
            dbContext.Reservations.Update(r);
            await dbContext.SaveChangesAsync();
        }

        public async Task Confirm(int id)
        {
            var r = await dbContext.Reservations.FirstOrDefaultAsync(e => e.Id == id);
            r.ReservationStatusId = 2;
            //sorry.. hardcoded, try to improve the loading speed
            dbContext.Reservations.Attach(r);
            await dbContext.SaveChangesAsync();
        }

        public async Task Cancel(int id)
        {
            var r = await dbContext.Reservations.FirstOrDefaultAsync(e => e.Id == id);
            r.ReservationStatusId = 3;
            //sorry.. hardcoded again
            dbContext.Reservations.Attach(r);
            await dbContext.SaveChangesAsync();
        }
        public async Task<int> Seated(int id)
        {
            var r = await dbContext.Reservations.FirstOrDefaultAsync(e => e.Id == id);
            r.ReservationStatusId = 4;
            //sorry.. hardcoded again
            dbContext.Reservations.Attach(r);
            await dbContext.SaveChangesAsync();
            return r.SittingId;
        }
        public async Task<int> Complete(int id)
        {
            var r = await dbContext.Reservations.FirstOrDefaultAsync(e => e.Id == id);
            r.ReservationStatusId = 5;
            //sorry.. hardcoded again
            dbContext.Reservations.Attach(r);
            await dbContext.SaveChangesAsync();
            return r.SittingId;
        }
        public async Task<IActionResult> DeleteReservation(int Id)
        {
            var r = await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == Id);
            dbContext.Remove(r);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("ReservationIndex");
        }
        public async Task<IEnumerable<Reservation>> GetReservations()
        {
            var rList = await dbContext.Reservations.AsNoTracking().ToListAsync();
            var cList = await dbContext.Customers.ToListAsync();
            var sList = await dbContext.Sittings.ToListAsync();
            var rTypeList = await dbContext.ReservationTypes.ToListAsync();
            var status = await Status();
            var table = await dbContext.Tables.ToListAsync();

            for (int i = 0; i < rList.Count; i++)
            {
                for (int j = 0; j < cList.Count; j++)
                {
                    if (rList[i].CustomerId == cList[j].Id)
                    {
                        rList[i].Customer = cList[j];
                    }
                }

                for (int k = 0; k < sList.Count; k++)
                {
                    if (rList[i].SittingId == sList[k].SittingId)
                    {
                        rList[i].Sitting = sList[k];
                    }
                }

                for (int l = 0; l < rTypeList.Count; l++)
                {
                    if (rList[i].ReservationTypeId == rTypeList[l].Id)
                    {
                        rList[i].ReservationType = rTypeList[l];
                    }
                }

                for (int s = 0; s < status.Count(); s++)
                {
                    if (rList[i].ReservationStatusId == status.ToArray()[s].Id)
                    {
                        rList[i].Status = status.ToArray()[s];
                    }
                }
                for (int t = 0; t < table.Count(); t++)
                {
                    if (rList[i].TableId == table[t].Id)
                    {
                        rList[i].Table = table[t];
                    }
                }

            }

            return rList;
        }
        public async Task<IEnumerable<Reservation>> GetReservations(int id)
        {
            var rList = await dbContext.Reservations.Where(r=>r.SittingId==id).AsNoTracking().ToListAsync();
            var cList = await dbContext.Customers.ToListAsync();
            var sList = await dbContext.Sittings.ToListAsync();
            var rTypeList = await dbContext.ReservationTypes.ToListAsync();
            var status = await Status();
            var table = await dbContext.Tables.ToListAsync();

            for (int i = 0; i < rList.Count; i++)
            {
                for (int j = 0; j < cList.Count; j++)
                {
                    if (rList[i].CustomerId == cList[j].Id)
                    {
                        rList[i].Customer = cList[j];
                    }
                }

                for (int k = 0; k < sList.Count; k++)
                {
                    if (rList[i].SittingId == sList[k].SittingId)
                    {
                        rList[i].Sitting = sList[k];
                    }
                }

                for (int l = 0; l < rTypeList.Count; l++)
                {
                    if (rList[i].ReservationTypeId == rTypeList[l].Id)
                    {
                        rList[i].ReservationType = rTypeList[l];
                    }
                }

                for (int s = 0; s < status.Count(); s++)
                {
                    if (rList[i].ReservationStatusId == status.ToArray()[s].Id)
                    {
                        rList[i].Status = status.ToArray()[s];
                    }
                }
                for (int t = 0; t < table.Count(); t++)
                {
                    if (rList[i].TableId == table[t].Id)
                    {
                        rList[i].Table = table[t];
                    }
                }

            }

            return rList;
        }
        public async Task EditCustomer(Customer c)
        {
            if (c != null)
            {
                dbContext.Customers.Update(c);
                await dbContext.SaveChangesAsync();
            }

        }

        public async Task AddingTables(int quantity, string location, int tableCapacity)
        {

            if (quantity != 0)
            {
                int t = 0, c = 1;
                var tb = dbContext.Tables
                    .Where(t => t.Location.Equals(location));
                if (await tb.AnyAsync())
                {
                    t = await tb.Select(t => t.SubId).MaxAsync();

                    if (c < t)
                    {
                        c = t + 1;
                    }
                    for (int i = 0; i < quantity; i++)
                    {
                        await dbContext.Tables.AddAsync(new Table
                        {
                            SubId = c++,
                            Capacity = tableCapacity,
                            Location = location,

                        }).ConfigureAwait(true);
                    }
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    for (int i = 0; i < quantity; i++)
                    {
                        await dbContext.Tables.AddAsync(new Table
                        {
                            SubId = c++,
                            Capacity = tableCapacity,
                            Location = location,

                        }).ConfigureAwait(true);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

        }

        public async Task<IEnumerable<Table>> Tables()
        {
            return await dbContext.Tables.ToListAsync();
        }
        public async Task<Table> Table(int id)
        {
            return await dbContext.Tables.FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<Dictionary<Sitting, List<Table>>> DynamicTableOutput(Dictionary<int, int[]> model)
        {
            var output = new Dictionary<Sitting, List<Table>>();

            var lt = await Tables();
            foreach (var item in model)
            {
                var tables = new List<Table>();
                foreach (var ele in item.Value)
                {
                    tables.Add(await Table(ele));
                }
                output.Add(await Sitting(item.Key), tables);

            }

            return output;
        }


        public async Task AddReservationStatus(ReservationStatus r)
        {

            await dbContext.ReservationStatuses.AddAsync(r);

            await dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<ReservationStatus>> Status()
        {
            return await dbContext.ReservationStatuses.ToListAsync();
        }
        public async Task<List<CustomerViewModel>> GetCustomerReservations(int id)
        {
            
            var reservations = await dbContext.Reservations.Where(r => r.CustomerId == id).ToListAsync();
            var res = new List<CustomerViewModel>();
            var statuses = await dbContext.ReservationStatuses.ToListAsync();
            var types = await dbContext.ReservationTypes.ToListAsync();
            if (reservations != null)
            {
                var i = 1;
                foreach (var r in reservations)
                {
                    var status = statuses.Where(s => s.Id == r.ReservationStatusId).FirstOrDefault();
                    var type = types.Where(t => t.Id == r.ReservationTypeId).FirstOrDefault();
                    var reservation = new CustomerViewModel()
                    {
                        ReservationId = i,
                        Date = r.DateTime,
                        Guest = r.Guests,
                        Status = status.Description,
                        Type = type.Description

                    };
                    res.Add(reservation);
                    i++;
                }
            }
            return res;
        }
         public async Task<List<Sitting>> SittingModels()
        {
            var sittings = await dbContext.Sittings.Where(s => s.Status == true).ToListAsync();
            return sittings;
        }
        
    }
}
