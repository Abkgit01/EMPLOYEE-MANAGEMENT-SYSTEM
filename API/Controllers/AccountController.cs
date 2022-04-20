using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.DTOs.UserDtos;
using API.IServices;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto request)
        {
            if(await UserExists(request.Username)) return BadRequest("Username already taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = request.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = request.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);
            
            if(user == null) return BadRequest("User not found");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            for(int i=0; i<ComputedHash.Length; i++)
            {
                if(ComputedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                Username = request.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string Username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == Username.ToLower());
        }
    }
}