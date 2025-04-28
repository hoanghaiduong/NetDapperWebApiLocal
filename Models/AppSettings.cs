

namespace NetDapperWebApi_local.Models
{
    public class AppSettings
    {
         public string TokenSecret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expiration {get;set;}
    }
}