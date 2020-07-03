using Microsoft.AspNetCore.Http;
using SME.SGP.Infra.Escopo;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SME.SGP.Infra.Contexto
{
    public class WorkerContext : ContextoBase, IDisposable
    {
        public WorkerContext()
        {
            Variaveis = new Dictionary<string, object>();

            WorkerContext contextoTransiente;

            if (!string.IsNullOrWhiteSpace(WorkerContext.ContextIdentifier))
            {
                if (WorkerServiceScope.TransientContexts.TryGetValue(WorkerContext.ContextIdentifier, out contextoTransiente))
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

        public static string ContextIdentifier
        {
            get
            {
                return Thread.CurrentThread.ManagedThreadId.ToString();
            }
        }

        public override void AdicionarVariaveis(IDictionary<string, object> variaveis)
        {
            Variaveis = variaveis;
        }
    }

    public class NoHttpContext : IHttpContextAccessor
    {
        public HttpContext HttpContext { get => null; set { } }
    }
}
