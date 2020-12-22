using Microsoft.AspNetCore.Mvc.Filters;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Api.Filtros
{
    public class DisposeConnectionFilter : IActionFilter
    {
        private readonly ISgpContext sgpContext;

        public DisposeConnectionFilter(ISgpContext sgpContext)
        {
            this.sgpContext = sgpContext ?? throw new ArgumentNullException(nameof(sgpContext));
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (sgpContext != null && sgpContext.State == System.Data.ConnectionState.Open)
            {
                sgpContext.Close();
                sgpContext.Dispose();
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
