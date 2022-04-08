﻿using WEBBACK2.Models.Auth;
using WEBBACK2.Models.Data;
using WEBBACK2.Models.UserDir;
using System.Web.Helpers;
using WEBBACK2.Exceptions;

namespace WEBBACK2.Services
{

    public interface IAuthService
    {
        public Task Register(RegisterDto model);
        public Task Login(LoginDto model);
        public Task Logout(string name);

        public Task SaveToken(string name, string refreshToken, DateTime refreshTokenExpiryTime);
        public Task UpdateToken(string name, string refreshToken);
    }
    public class AuthService: IAuthService
    {

        private readonly ApplicationDbContext _context;
        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }


        public Task Login(LoginDto model)
        {
            User user = _context.Users.Where(x=> x.userName == model.userName).FirstOrDefault();
            if (user is null)
            {
                throw new ValidationException("Wrong username or password");
            }
            else
            {
                if (Crypto.VerifyHashedPassword(user.password, model.password))
                {
                    return Task.CompletedTask;
                }
                else throw new ValidationException("Wrong username or password");
            }
        }


        public async Task Register(RegisterDto model)
        {
            await _context.Users.AddAsync(new User
            {
                userId = 0,
                userName = model.username,
                roleId = 1,
                name = model.name,
                surname = model.surname,
                password = Crypto.HashPassword(model.password)
            }) ;
            await _context.SaveChangesAsync();
        }

        public async Task Logout(string name)
        {
            if (name is null)
            {
                throw new ValidationException("Bad username");
            }
            User user = _context.Users.Where(x => x.userName == name).FirstOrDefault();

            user.RefreshToken = null;
            await _context.SaveChangesAsync();
        }


        public async Task SaveToken(string name,string refreshToken,DateTime refreshTokenExpiryTime)
        {
            User user = _context.Users.Where(x => x.userName == name).FirstOrDefault();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateToken(string name, string refreshToken)
        {
            User user = _context.Users.Where(x => x.userName == name).FirstOrDefault();

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new ValidationException("Invalid access token or refresh token");
            }

            user.RefreshToken = refreshToken;
            await _context.SaveChangesAsync();
        }



    }
}
