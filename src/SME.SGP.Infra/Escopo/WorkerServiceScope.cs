using SME.SGP.Infra.Contexto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SME.SGP.Infra.Escopo
{
    public class WorkerServiceScope
    {
        static WorkerServiceScope()
        {
            TransientContexts = new ConcurrentDictionary<string, WorkerContext>();
            TransientServices = new ConcurrentDictionary<string, List<IDisposable>>();
        }

        public static ConcurrentDictionary<string, WorkerContext> TransientContexts { get; set; }
        public static ConcurrentDictionary<string, List<IDisposable>> TransientServices { get; set; }

        public static IDisposable AddTransientDisposableServices(IDisposable service)
        {
            if (service != null)
                if (TransientServices.ContainsKey(WorkerContext.ContextIdentifier))
                {
                    List<IDisposable> services = null;

                    if (TransientServices.TryGetValue(WorkerContext.ContextIdentifier, out services))
                    {
                        services.Add(service);
                        TransientServices[WorkerContext.ContextIdentifier] = services;
                    }
                }
                else
                    TransientServices.TryAdd(WorkerContext.ContextIdentifier, new List<IDisposable>(new[] { service }));

            return service;
        }

        public static void DestroyScope()
        {
            List<IDisposable> services = null;
            WorkerContext context = null;

            if (TransientServices.TryRemove(WorkerContext.ContextIdentifier, out services))
            {
                if (services != null)
                {
                    foreach (var item in services)
                        if (item != null)
                            item.Dispose();
                }
            }
            if (TransientContexts.TryRemove(WorkerContext.ContextIdentifier, out context))
                context.Dispose();
        }
    }
}