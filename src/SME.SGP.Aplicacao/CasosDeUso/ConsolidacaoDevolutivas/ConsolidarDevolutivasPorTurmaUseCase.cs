using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarDevolutivasPorTurmaUseCase : AbstractUseCase, IConsolidarDevolutivasPorTurmaUseCase
    {
        public ConsolidarDevolutivasPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroDevolutivaTurmaDTO>();

            var diarioBordoTurma = await mediator.Send(new ObterDiariosDeBordoComDevolutivasPorAnoLetivoTurmaQuery(filtro.TurmaId, filtro.AnoLetivo));
                
            if (diarioBordoTurma.NaoEhNulo())
            {
                var consolidacaoDevolutiva = await mediator.Send(new ObterConsolidacaoDevolutivasPorTurmaIdQuery(filtro.TurmaId)) ?? new ConsolidacaoDevolutivas();

                consolidacaoDevolutiva.QuantidadeRegistradaDevolutivas = diarioBordoTurma.QtdeRegistradaDevolutivas;
                consolidacaoDevolutiva.QuantidadeEstimadaDevolutivas = 0;
                consolidacaoDevolutiva.TurmaId = diarioBordoTurma.TurmaId;
                
                var periodoDeDiasDevolutivas = await mediator.Send(new ObterParametroSistemaPorTipoQuery(Dominio.TipoParametroSistema.PeriodoDeDiasDevolutiva));
                if (periodoDeDiasDevolutivas.EhNulo())
                    return false;

                if (diarioBordoTurma.QtdeDiarioBordoRegistrados >= int.Parse(periodoDeDiasDevolutivas))
                    consolidacaoDevolutiva.QuantidadeEstimadaDevolutivas = (diarioBordoTurma.QtdeDiarioBordoRegistrados / int.Parse(periodoDeDiasDevolutivas));

                await mediator.Send(new RegistraConsolidacaoDevolutivasTurmaCommand(consolidacaoDevolutiva));
            }
            return true;
        }
    }
}
