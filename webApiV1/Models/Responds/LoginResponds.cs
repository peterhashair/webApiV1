using System;
namespace webApiV1.Models.Responses
{
    public class LoginResponse
    {
        public LoginResponse(string token, string id, string email)
        {
            Token = token;
            Id = id;
            Email = email;
        }

        public string Token { get; private set; }

        public string Id { get; private set; }

        public string Email { get; private set; }
    }
}
