namespace AuthServer.BL.Models
{
    public class UpdateUserModel
    {
        public string GivenName { get; set; }
        
        public string FamilyName { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}