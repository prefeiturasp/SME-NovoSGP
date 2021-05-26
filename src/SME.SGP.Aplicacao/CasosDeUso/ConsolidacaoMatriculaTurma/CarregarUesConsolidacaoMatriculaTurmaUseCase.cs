using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarUesConsolidacaoMatriculaTurmaUseCase : AbstractUseCase, ICarregarUesConsolidacaoMatriculaTurmaUseCase
    {
        public CarregarUesConsolidacaoMatriculaTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var ue = mensagem.ObterObjetoMensagem<FiltroConsolidacaoMatriculaUeDto>();
                var anoAtual = DateTime.Now.Year;
                var anosLetivos = new List<int>() { 
                    anoAtual
                };
                if (ue.AnosAnterioresParaConsolidar != null && ue.AnosAnterioresParaConsolidar.Any())
                {
                    anosLetivos.AddRange(ue.AnosAnterioresParaConsolidar);
                }
                var turmas = await mediator.Send(new ObterTurmaIdentificadoresPorUeAnosLetivosQuery(ue.UeId, anosLetivos));
                //await ConsolidarMatriculasTurmasAnoAtual(anoAtual, ue.UeId);
                //if(ue.AnosAnterioresParaConsolidar.Any()) await ConsolidarFrequenciaTurmasHistorico(ue.AnosAnterioresParaConsolidar, ue.UeId);
                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private async Task ConsolidarUes()



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
        }
    }
}
