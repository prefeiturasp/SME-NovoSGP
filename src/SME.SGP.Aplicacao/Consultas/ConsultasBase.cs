using Microsoft.AspNetCore.Http;
using SME.SGP.Infra;
using System;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public abstract class ConsultasBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ConsultasBase(IHttpContextAccessor httpContext)
        {
            this.httpContextAccessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        }

        public Paginacao Paginacao
        {
            get
            {
                var numeroPaginaQueryString = httpContextAccessor.HttpContext.Request.Query["NumeroPagina"];
                var numeroRegistrosQueryString = httpContextAccessor.HttpContext.Request.Query["NumeroRegistros"];

                if (numeroPaginaQueryString.Count == 0 || numeroRegistrosQueryString.Count == 0)
                    return new Paginacao(0, 0);

                var numeroPagina = int.Parse(numeroPaginaQueryString.FirstOrDefault());
                var numeroRegistros = int.Parse(numeroRegistrosQueryString.FirstOrDefault());

                return new Paginacao(numeroPagina, numeroRegistros);
            }
        }
    }
}