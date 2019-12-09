using Microsoft.AspNetCore.Http;
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
                if (TransientContexts.TryGetValue(Thread.CurrentThread.Name, out contextoTransiente))
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
            List<ISgpContext> contexts = null;

            if (TransientDatabaseContexts.TryRemove(Thread.CurrentThread.Name, out contexts))
            {
                foreach (var item in contexts)
                    if (item != null)
                        item.Dispose();

            }
        }

        public static ConcurrentDictionary<string, WorkerContext> TransientContexts { get; set; }
        public static ConcurrentDictionary<string, List<ISgpContext>> TransientDatabaseContexts { get; set; }

        static WorkerContext()
        {
            TransientContexts = new ConcurrentDictionary<string, WorkerContext>();
            TransientDatabaseContexts = new ConcurrentDictionary<string, List<ISgpContext>>();
        }

        public static ISgpContext AddTransienteDatabaseContext(ISgpContext context)
        {
            if (context != null)
                if (TransientDatabaseContexts.ContainsKey(Thread.CurrentThread.Name))
                {
                    List<ISgpContext> contexts = null;

                    if (TransientDatabaseContexts.TryGetValue(Thread.CurrentThread.Name, out contexts))
                    {
                        contexts.Add(context);
                        TransientDatabaseContexts[Thread.CurrentThread.Name] = contexts;
                    }
                }
                else
                    TransientDatabaseContexts.TryAdd(Thread.CurrentThread.Name, new List<ISgpContext>(new[] { context }));

            return context;
        }
    }

    public class NoHttpContext : IHttpContextAccessor
    {
        public HttpContext HttpContext { get => null; set { } }
    }
}
