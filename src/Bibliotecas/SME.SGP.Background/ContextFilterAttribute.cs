using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using SME.Background.Core;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Escopo;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Hangfire
{
    public class ContextFilterAttribute : JobFilterAttribute,
    IClientFilter, IServerFilter
    {
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
                filterContext.SetJobParameter("contextoAplicacao", contextoTransiente);
            }
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            WorkerServiceScope.DestroyScope();
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            var contextoTransiente = filterContext.GetJobParameter<WorkerContext>("contextoAplicacao");
            WorkerServiceScope.TransientContexts.TryAdd(WorkerContext.ContextIdentifier, contextoTransiente);

        }

        private IContextoAplicacao ObterContexto()
        {
            var provider = Orquestrador.Provider;
            return provider.GetService<IContextoAplicacao>();
        }
    }
}
