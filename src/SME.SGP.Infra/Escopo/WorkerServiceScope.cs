using SME.SGP.Infra.Contexto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SME.SGP.Infra.Escopo
{
    public class WorkerServiceScope
    {
        static WorkerServiceScope()
        {
            TransientContexts = new ConcurrentDictionary<string, WorkerContext>();
            TransientServices = new ConcurrentDictionary<string, List<IDisposable>>();
        }

        public static ConcurrentDictionary<string, List<IDisposable>> TransientServices { get; set; }
        public static ConcurrentDictionary<string, WorkerContext> TransientContexts { get; set; }


        public static IDisposable AddTransientDisposableServices(IDisposable service)
        {
            if (service != null)
                if (TransientServices.ContainsKey(Thread.CurrentThread.Name))
                {
                    List<IDisposable> services = null;

                    if (TransientServices.TryGetValue(Thread.CurrentThread.Name, out services))
                    {
                        services.Add(service);
                        TransientServices[Thread.CurrentThread.Name] = services;
                    }
                }
                else
                    TransientServices.TryAdd(Thread.CurrentThread.Name, new List<IDisposable>(new[] { service }));

            return service;
        }

        public static void DestroyScope()
        {
            List<IDisposable> services = null;
            WorkerContext context = null;

            if (TransientServices.TryRemove(Thread.CurrentThread.Name, out services))
            {
                foreach (var item in services)
                    if (item != null)
                        item.Dispose();

            }
            if (TransientContexts.TryRemove(Thread.CurrentThread.Name, out context))
                context.Dispose();
        }
    }
}
