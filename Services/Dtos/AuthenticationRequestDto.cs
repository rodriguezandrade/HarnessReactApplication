namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class AuthenticationRequestDto
    {
        public string ServerID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientID { get; set; }
        public string TenantID { get; set; }
        public string ObjectID { get; set; }
        public string AuthAAD { get; set; }
        public string AuthProtocol { get; set; }
        public string AuthServer { get; set; }
        public string AccessToken { get; set; }
        public string CaseNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NumberOfResults { get; set; }
        public string PageNumber { get; set; }
        public string SearchField { get; set; }
        public string SearchFilter { get; set; }
        public string SearchSort { get; set; }
        public string SearchText { get; set; }
    }
}
