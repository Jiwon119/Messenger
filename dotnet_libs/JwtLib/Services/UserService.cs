using JwtLib.Entities;
using JwtLib.Helpers;
using JwtLib.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Athenticate;

namespace JwtLib.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest request);
        IEnumerable<User> GetAll();
    }

    // Life Cycle: Transient
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        public readonly AppSettings _appSettings = new AppSettings()
        {
            Secret = Environment.GetEnvironmentVariable("SECRET_KEY")
        };

        private DateTime _curTime = DateTime.Now;

        public AuthenticateResponse Authenticate(AuthenticateRequest request)
        {
            // Create OTP
            OTP _otp = new OTP(_appSettings.Secret);
            Console.WriteLine($"Otp: {_otp.GetOtp()}, DateTime: {_curTime}");

            // Otp Authenticate
            if (_otp.GetOtp() != request.OTP)
                return null;

            // User Create
            User user = CreateUser(request);

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for ~ days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = user.Username,
                Issuer = user.Company,
                IssuedAt = _curTime,
                NotBefore = _curTime,
                TokenType = user.TokenType,
                Expires = _curTime.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            };
            // create Token about tokenDescriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            using (var ctx = new JwtDbContext())
            {
                if(!ctx.Users.IsNullOrEmpty())
                    users = ctx.Users.ToList();
            }
            return users;
        }

        public User CreateUser(AuthenticateRequest request)
        {
            User newUser = new User()
            {
                Company = request.Company,
                Department = request.Department,
                TokenType = request.TokenType,
                Username = request.UserName,
                IssuedDate = _curTime.ToString("yyyyMMdd HHmm"),
                ExpireDate = _curTime.AddDays(7).ToString("yyyyMMdd HHmm"),
            };

            using (var ctx = new JwtDbContext())
            {
                User user = ctx.Users.Where(e => e.Username == newUser.Username && e.Company == newUser.Company && e.Department == newUser.Department && e.TokenType == newUser.TokenType)
                                     .FirstOrDefault();

                // if user exist, update IssuedDate, ExpiredDate
                if (user == null)
                {
                    ctx.Users.Add(newUser);
                }
                else
                {
                    Console.WriteLine($"Update User {user.Username}");
                    user.IssuedDate = newUser.IssuedDate;
                    user.ExpireDate = newUser.ExpireDate;
                }
                
                ctx.SaveChanges();
            }

            return newUser;
        }
    }
}
