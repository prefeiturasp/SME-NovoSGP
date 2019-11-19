using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SME.SGP.Hangfire
{
    public class ContextFilterAttribute : JobFilterAttribute,
    IClientFilter, IServerFilter /*, IElectStateFilter, IApplyStateFilter*/
    {
        public static IServiceCollection RegistreredServices { get; set; }

        public void OnCreated(CreatedContext filterContext)
        {
            // Não preciso fazer nada aqui
        }

        public void OnCreating(CreatingContext filterContext)
        {
            IContextoAplicacao contexto = ObterContexto();
            if (contexto != null)
            {
                var contextoTransiente = new WorkerContext();
                contextoTransiente.AtribuirContexto(contexto);
                filterContext.SetJobParameter("contextoAplicacao", JsonConvert.SerializeObject(contextoTransiente));
            }
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            WorkerContext.TransientContexts.Remove(Thread.CurrentThread.Name);
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            var contextoTransiente = JsonConvert.DeserializeObject<WorkerContext>(filterContext.GetJobParameter<string>("contextoAplicacao"));
            WorkerContext.TransientContexts.Add(Thread.CurrentThread.Name, contextoTransiente);

        }

        private IContextoAplicacao ObterContexto()
        {
            var provider = RegistreredServices.BuildServiceProvider();
            return provider.GetService<IContextoAplicacao>();
        }
    }
}
