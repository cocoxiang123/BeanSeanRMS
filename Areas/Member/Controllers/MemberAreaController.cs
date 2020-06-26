using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Models;
using ReservationSystem.Repos;

namespace ReservationSystem.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = "Member")]
    public class MemberAreaController : ReservationRepos
    {
        //add fields for context and user manager
        //protected readonly UserManager<IdentityUser> _userManager;
        //protected readonly ApplicationDbContext _context;
        //protected async Task<IdentityUser> GetIdentityUserAsync()
        //{
        //    var u = User;
        //    var user = await _userManager.FindByEmailAsync(User.Identity.Name);
        //    return user;
        //}

        //protected async Task<Models.Person> GetMemberAsync()
        //{
        //var user = await GetIdentityUserAsync();
        //var person = await _context.People.FirstOrDefaultAsync(p => p.Id.Equals(user.Id));
        //return person;
        //}

        public MemberAreaController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager) : base(dbContext, userManager)
        {
        }
    }
}