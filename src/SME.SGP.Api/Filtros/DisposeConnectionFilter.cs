using Microsoft.AspNetCore.Mvc.Filters;
using SME.SGP.Dominio;
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

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (sgpContext.NaoEhNulo() && sgpContext.State == System.Data.ConnectionState.Open)
            {
                sgpContext.Close();
                sgpContext.Dispose();
            }

        }
    }
}
