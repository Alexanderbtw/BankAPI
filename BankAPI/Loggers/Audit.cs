using BankAPI.Entities;

namespace BankAPI.Loggers
{
    public class Audit
    {
        public Guid AuditId { get; set; }
        public string IPAdress { get; set; }
        public string RequestData { get; set; }
        public string AreaAccessed { get; set; }
        public string Method { get; set; }
        public string ResultCode { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
