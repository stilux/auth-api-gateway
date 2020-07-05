namespace AuthServer.Models
{
    public class UserDto
    {
        public long Id  { get; set; }
        
        public string UserName { get; set; }
        
        public string GivenName { get; set; }
        
        public string FamilyName { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}