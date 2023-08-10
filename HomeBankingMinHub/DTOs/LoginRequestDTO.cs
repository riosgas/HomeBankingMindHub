using HomeBankingMindHub.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomeBankingMindHub.dtos
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
