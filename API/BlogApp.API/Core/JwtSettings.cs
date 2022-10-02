namespace BlogApp.API.Core
{
    public class JwtSettings
    {
        public int Minutes { get; set; }
        public int RefreshTokenMinutes { get; set; }
        public string Issuer { get; set; } 
        public string AccessSecretKey { get; set; }
        public string RefreshSecretKey { get; set; }
    }
}
