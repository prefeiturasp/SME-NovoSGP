﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Api.Filtros
{
    public class DisposeConnectionFilterConsulta : IActionFilter, IAsyncActionFilter
    {
        private readonly ISgpContextConsultas sgpContextConsultas;

        public DisposeConnectionFilterConsulta(ISgpContextConsultas sgpContextConsultas)
        {
            this.sgpContextConsultas =
                sgpContextConsultas ?? throw new ArgumentNullException(nameof(sgpContextConsultas));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            sgpContextConsultas.Close();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            sgpContextConsultas.Open();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await sgpContextConsultas.OpenAsync();
            await next();
            await sgpContextConsultas.CloseAsync();
        }
    }
}