using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaTurmasPorAnoUseCase : AbstractUseCase, IConsolidarFrequenciaTurmasPorAnoUseCase
    {
        public ConsolidarFrequenciaTurmasPorAnoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var filtro = mensagem.ObterObjetoMensagem<FiltroAnoDto>();

                var parametroPercentualMinimo = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.PercentualFrequenciaCritico, filtro.Ano));
                var parametroPercentualMinimoInfantil = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.PercentualFrequenciaMinimaInfantil, filtro.Ano));

                await ConsolidarFrequenciasTurmas(filtro.Ano, double.Parse(parametroPercentualMinimo.Valor), double.Parse(parametroPercentualMinimoInfantil.Valor));

                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private async Task ConsolidarFrequenciasTurmas(int ano, double percentualMinimo, double percentualMinimoInfantil)
        {
            var turmas = await mediator.Send(new ObterTurmasComModalidadePorAnoQuery(ano));
            foreach(var turma in turmas)
            {
                var filtro = new FiltroConsolidacaoFrequenciaTurma(turma.TurmaId, turma.TurmaCodigo, turma.ModalidadeInfantil ? percentualMinimoInfantil : percentualMinimo);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarFrequenciasPorTurma, filtro, Guid.NewGuid(), null));
            }
        }
    }
}
