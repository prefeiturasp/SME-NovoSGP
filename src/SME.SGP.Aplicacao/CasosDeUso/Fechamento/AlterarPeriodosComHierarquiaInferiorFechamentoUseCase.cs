using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarPeriodosComHierarquiaInferiorFechamentoUseCase : AbstractUseCase, IAlterarPeriodosComHierarquiaInferiorFechamento
    {
        private readonly IServicoPeriodoFechamento servicoPeriodoFechamento;

        public AlterarPeriodosComHierarquiaInferiorFechamentoUseCase(IMediator mediator, IServicoPeriodoFechamento servicoPeriodoFechamento) : base(mediator)
        {
            this.servicoPeriodoFechamento = servicoPeriodoFechamento;
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var fechamento = mensagem.ObterObjetoMensagem<PeriodoFechamento>();
            servicoPeriodoFechamento.AlterarPeriodosComHierarquiaInferior(fechamento);
            return true;
        }
    }
}
