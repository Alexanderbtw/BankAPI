using BankAPI.Database;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace BankAPI.Loggers
{
    public class AuditAttribute : ActionFilterAttribute
    {
        readonly Lazy<Audit> audit = new();

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            StringBuilder data = new StringBuilder();
            foreach (var item in context.ActionArguments)
            {
                data.Append(item.Value);
            }

            audit.Value.AuditId = Guid.NewGuid();
            audit.Value.IPAdress = request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            audit.Value.RequestData = data.ToString();
            audit.Value.AreaAccessed = request.GetDisplayUrl();
            audit.Value.Method = request.Method;
            audit.Value.Timestamp = DateTime.UtcNow;

            return base.OnActionExecutionAsync(context, next);
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            audit.Value.ResultCode = context.HttpContext.Response.StatusCode.ToString();

            using var DbContext = context.HttpContext.RequestServices.GetService<AuditContext>();

            DbContext?.AuditRecords.Add(audit.Value);
            DbContext?.SaveChanges();

            return base.OnResultExecutionAsync(context, next);
        }
    }
}
