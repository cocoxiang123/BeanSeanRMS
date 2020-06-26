using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Models;
using ReservationSystem.Repos;
using ReservationSystem.ViewModels;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using T4RMSSolution.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace T4RMSSolution.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly SignInManager<IdentityUser> _signInManager;
        private IConfiguration _config;
        public ReservationAPIController(ApplicationDbContext dbContext, SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            this._dbContext = dbContext;
            this._signInManager = signInManager;
            _config = config;
        }

        [Route("CustomerReservations")]
        [HttpGet]
        [EnableCors("Policy1")]
        public async Task<IActionResult> ReservationByEmail()
        {
            var email = User.Claims.Select(c=>c.Value).FirstOrDefault();
            var result = await _dbContext.Reservations
                .Where(r =>r.Customer.Email == email)
                .Select(r => new {
                    r.Id,
                    r.Notes,
                    r.Guests,
                    r.Customer.FirstName,
                    r.Status.Description,
                    r.DateTime,
                    ReservationType = r.ReservationType.Description,
                    SittingType = r.Sitting.SittingType.Description
                })
                .ToListAsync();

            if(result.Count == 0)
            {
                var error = new
                {
                    Response = "false",
                    Error = "Reservation not found!"

                };
                return Ok(error);
            }


            return Ok(result);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Authentication(string email,string password)
        {    
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            var result = await _signInManager.PasswordSignInAsync(email, password, true, false);
            if (result.Succeeded)
            {
                var userModel = new UserModel{ Email = email,Password=password };
                var tokenString = GenerateJSONWebToken(userModel);
                var data = new { token = tokenString };
                return Ok(data);
            }
            else
            {
                return Ok("Login information not valid");
            }
         
        }
        [Route("booking")]
        [HttpPost]
        public async Task<IActionResult> Booking(MobileReservationModel reservationObject)
        {
            var reservation = reservationObject;
            var t = TimeZoneInfo.ConvertTimeFromUtc(reservation.ReservationTime, TimeZoneInfo.Local);
            var ReservationType =await _dbContext.ReservationTypes.FirstOrDefaultAsync(n => n.Description.ToLower().Contains("phone"));
            var sitting =await _dbContext.Sittings.Where(s => s.Start.Date == t.Date).FirstOrDefaultAsync(s => s.Start.Hour <= t.Hour && s.End.Hour >= t.Hour);
            var Status =await _dbContext.ReservationStatuses.FirstOrDefaultAsync(n => n.Description.ToLower().Contains("pending"));
            var customer = await _dbContext.Customers.Where(c => c.Email == reservation.Email).FirstOrDefaultAsync();

   
            if (sitting !=null)
            {
                var r = new Reservation
                {
                    SittingId = sitting.SittingId,
                    ReservationTypeId = ReservationType.Id,
                    ReservationStatusId = Status.Id,
                    Customer = customer,
                    CustomerId = customer.Id,
                    Guests = reservation.Guests,
                    DateTime = t,
                    TableId =2,

                };

                await _dbContext.Reservations.AddAsync(r);
                await _dbContext.SaveChangesAsync();
                return Ok("ok");
             }
            else
            {
                return NotFound();
            }
               

            
        }

        private string GenerateJSONWebToken(UserModel userModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email,userModel.Email), 
                
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }
    }
}