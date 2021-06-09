using MediatR;
using Sentry;
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
            try
            {
                var filtro = mensagem.ObterObjetoMensagem<FiltroDevolutivaTurmaDTO>();
                var devolutivaTurma = await mediator.Send(new ObterDevolutivaPorTurmaQuery(filtro.TurmaId, filtro.AnoLetivo));

                if (devolutivaTurma != null)
                {
                    var periodoDeDiasDevolutivas = await mediator.Send(new ObterParametroSistemaPorTipoQuery(Dominio.TipoParametroSistema.PeriodoDeDiasDevolutiva));
                    if (periodoDeDiasDevolutivas == null)
                        return false;

                    var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(devolutivaTurma.TurmaId));
                    if (turma == null)
                        return false;

                    var tipoCalendario = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
                    if (tipoCalendario == 0)
                        return false;

                    var diasLetivos = await mediator.Send(new ObterQuantidadeDiasLetivosPorCalendarioQuery(tipoCalendario, devolutivaTurma.DreId, devolutivaTurma.UeId));

                    CalcularQuantidadeEstimadaDeDevolutivas(devolutivaTurma, periodoDeDiasDevolutivas, diasLetivos);

                    await RegistraConsolidacaoDevolutivasTurma(turma.Id, devolutivaTurma.QuantidadeEstimadaDevolutivas, devolutivaTurma.QuantidadeRegistradaDevolutivas);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private static void CalcularQuantidadeEstimadaDeDevolutivas(Infra.Dtos.ConsolidacaoDevolutivaTurmaDTO devolutivasTurma, string periodoDeDiasDevolutivas, Dto.DiasLetivosDto diasLetivos)
        {
            devolutivasTurma.QuantidadeEstimadaDevolutivas = (diasLetivos.Dias / int.Parse(periodoDeDiasDevolutivas));
        }

        private async Task RegistraConsolidacaoDevolutivasTurma(long turmaId, int quantidadeEstimadaDevolutivas, int quantidadeRegistradaDevolutivas)
        {
           await mediator.Send(new RegistraConsolidacaoDevolutivasTurmaCommand(turmaId, quantidadeEstimadaDevolutivas, quantidadeRegistradaDevolutivas));
        }
    }
}
