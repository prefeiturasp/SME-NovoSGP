using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

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


            var devolutivaTurma = await mediator.Send(new ObterDevolutivaPorTurmaQuery(filtro.TurmaId, filtro.AnoLetivo));

            var diarioBordoTurma = await mediator.Send(new ObterDiariosDeBordoPorAnoLetivoTurmaQuery(filtro.TurmaId, filtro.AnoLetivo));

            if (diarioBordoTurma != null)
            {

                var consolidacaoDevolutivaTurma = MapearDTO(devolutivaTurma, diarioBordoTurma);

                var periodoDeDiasDevolutivas = await mediator.Send(new ObterParametroSistemaPorTipoQuery(Dominio.TipoParametroSistema.PeriodoDeDiasDevolutiva));
                if (periodoDeDiasDevolutivas == null)
                    return false;

                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(diarioBordoTurma.TurmaId));
                if (turma == null)
                    return false;

                var quantidadeDiarioBordoRegistrado = diarioBordoTurma == null ? 0 : diarioBordoTurma.QuantidadeDiarioBordoRegistrado;

                CalcularQuantidadeEstimadaDeDevolutivas(consolidacaoDevolutivaTurma, periodoDeDiasDevolutivas, quantidadeDiarioBordoRegistrado);

                await RegistraConsolidacaoDevolutivasTurma(turma.Id, consolidacaoDevolutivaTurma.QuantidadeEstimadaDevolutivas, consolidacaoDevolutivaTurma.QuantidadeRegistradaDevolutivas);
            }

            return true;

        }

        private static ConsolidacaoDevolutivaTurmaDTO MapearDTO(ConsolidacaoDevolutivaTurmaDTO devolutivaTurma, QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO diarioBordoTurma)
        {
            return new ConsolidacaoDevolutivaTurmaDTO
            {
                DreId = diarioBordoTurma.DreId,
                UeId = diarioBordoTurma.UeId,
                TurmaId = diarioBordoTurma.TurmaId,
                QuantidadeRegistradaDevolutivas = devolutivaTurma == null ? 0 : devolutivaTurma.QuantidadeRegistradaDevolutivas
            };
        }

        private static void CalcularQuantidadeEstimadaDeDevolutivas(ConsolidacaoDevolutivaTurmaDTO consolidacaoDevolutivaTurma, string periodoDeDiasDevolutivas, int quantidadeDiarioBordoRegistrado)
        {
            if (quantidadeDiarioBordoRegistrado >= int.Parse(periodoDeDiasDevolutivas))
            {
                consolidacaoDevolutivaTurma.QuantidadeEstimadaDevolutivas = (quantidadeDiarioBordoRegistrado / int.Parse(periodoDeDiasDevolutivas));
            }
            else
            {
                consolidacaoDevolutivaTurma.QuantidadeEstimadaDevolutivas = 0;
            }
        }

        private async Task RegistraConsolidacaoDevolutivasTurma(long turmaId, int quantidadeEstimadaDevolutivas, int quantidadeRegistradaDevolutivas)
        {
            await mediator.Send(new RegistraConsolidacaoDevolutivasTurmaCommand(turmaId, quantidadeEstimadaDevolutivas, quantidadeRegistradaDevolutivas));
        }
    }
}
