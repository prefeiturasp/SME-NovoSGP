using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using SME.SGP.Infra;

namespace SME.SGP.Api.Filtros
{
    public class DisposeConnectionFilter : IActionFilter, IAsyncActionFilter
    {
        private readonly ISgpContext sgpContext;

        public DisposeConnectionFilter(ISgpContext sgpContext)
        {
            this.sgpContext = sgpContext ?? throw new ArgumentNullException(nameof(sgpContext));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            sgpContext.Close();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            sgpContext.Open();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await sgpContext.OpenAsync();
            await next();
            await sgpContext.CloseAsync();
        }
    }
}
