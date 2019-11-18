using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Contexto
{
    public class NoHttpContext : IHttpContextAccessor
    {
        public HttpContext HttpContext { get => null; set { } }
    }
}
