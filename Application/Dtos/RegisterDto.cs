using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class RegisterDto
    {
        public required string Name { get; set; }

        public required string Nickname { get; set; }

        public required string Email { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }

        public required string ConfirmPassword { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public required IFormFile Photo { get; set; }

        }
    }
