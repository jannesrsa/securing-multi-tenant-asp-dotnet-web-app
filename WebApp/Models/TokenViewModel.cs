namespace WebApp.Models
{
    public class TokenViewModel
    {
        public string PlainToken { get; set; }
        public string TokenValue { get; set; }
        public object UserName { get; internal set; }
        public string TenantName { get; internal set; }
    }
}