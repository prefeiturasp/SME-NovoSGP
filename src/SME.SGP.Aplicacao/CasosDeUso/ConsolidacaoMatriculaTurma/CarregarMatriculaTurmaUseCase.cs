using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarMatriculaTurmaUseCase : AbstractUseCase, ICarregarMatriculaTurmaUseCase
    {
        public CarregarMatriculaTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var ue = mensagem.ObterObjetoMensagem<FiltroConsolidacaoMatriculaUeDto>();
                var anoAtual = DateTime.Now.Year;

                await ConsolidarMatriculasTurmasAnoAtual(anoAtual, ue.UeCodigo);
                if(ue.AnosAnterioresParaConsolidar.Any()) await ConsolidarFrequenciaTurmasHistorico(ue.AnosAnterioresParaConsolidar, ue.UeCodigo);
                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }



        private async Task ConsolidarMatriculasTurmasAnoAtual(int anoAtual, string codigoUe)
        {
            var matriculasConsolidadas = await mediator.Send(new ObterMatriculasConsolidacaoPorAnoQuery(anoAtual, codigoUe));
            foreach (var matricula in matriculasConsolidadas)
            {
                var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(matricula.TurmaCodigo));
                matricula.TurmaId = turmaId;
                try
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidacaoMatriculasTurmasSync, matricula, Guid.NewGuid(), null));
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }
        }

        private async Task ConsolidarFrequenciaTurmasHistorico(IEnumerable<int> anosParaConsolidar, string ueCodigo)
        {
            foreach (var ano in anosParaConsolidar)
            {
                var matriculasConsolidadas = await mediator.Send(new ObterMatriculasConsolidacaoAnosAnterioresQuery(ano, ueCodigo));
                foreach (var matricula in matriculasConsolidadas)
                {
                    var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(matricula.TurmaCodigo));
                    if (turmaId > 0)
                    {
                        matricula.TurmaId = turmaId;
                        try
                        {
                            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidacaoMatriculasTurmasSync, matricula, new Guid(), null));
                        }
                        catch (Exception ex)
                        {
                            SentrySdk.CaptureException(ex);
                        }
                    }
                }
            }
        }
    }
}
