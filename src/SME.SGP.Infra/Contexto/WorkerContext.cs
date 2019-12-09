using Microsoft.AspNetCore.Http;
using SME.SGP.Infra.Escopo;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SME.SGP.Infra.Contexto
{
    public class WorkerContext : ContextoBase, IDisposable
    {
        public WorkerContext()
        {
            Variaveis = new Dictionary<string, object>();

            WorkerContext contextoTransiente;

            if (!string.IsNullOrWhiteSpace(Thread.CurrentThread.Name))
            {
                if (WorkerServiceScope.TransientContexts.TryGetValue(Thread.CurrentThread.Name, out contextoTransiente))
                    AtribuirContexto(contextoTransiente);
            }
        }

        public override IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto)
        {
            Variaveis = contexto.Variaveis;
            return this;
        }

        public void Dispose()
        {
            Variaveis.Clear();
        }
    }

    public class NoHttpContext : IHttpContextAccessor
    {
        public HttpContext HttpContext { get => null; set { } }
    }
}
