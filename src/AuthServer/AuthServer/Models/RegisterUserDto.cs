﻿namespace AuthServer.Models
{
    public class RegisterUserDto
    {
        public string UserName { get; set; }
        
        public string Password { get; set; }
        
        public string GivenName { get; set; }
        
        public string FamilyName { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}