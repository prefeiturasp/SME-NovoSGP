using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidarEncerrarEncaminhamentoAEEAutomaticoUseCase : AbstractUseCase, IValidarEncerrarEncaminhamentoAEEAutomaticoUseCase
    {
        public ValidarEncerrarEncaminhamentoAEEAutomaticoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroValidarEncerrarEncaminhamentoAEEAutomaticoDto>();
            //log-------------------------------
                var logPlanoAee = new LogPlanoAee();
            //----------------------------------

            var matriculasTurmaAlunoEol = await mediator.Send(new ObterMatriculasAlunoNaUEQuery(filtro.UeCodigo, filtro.AlunoCodigo));

            //log-------------------------------
                logPlanoAee.filtro = filtro;
                logPlanoAee.matriculasTurmaAlunoEol = matriculasTurmaAlunoEol;
            //----------------------------------

            if (matriculasTurmaAlunoEol.EhNulo() || !matriculasTurmaAlunoEol.Any())
                return false;

            var estaAtivo = matriculasTurmaAlunoEol.Any(c => SituacoesAtivas.Contains(c.CodigoSituacaoMatricula) && c.DataSituacao <= DateTime.Today);

            //log-------------------------------
                logPlanoAee.estaAtivo = estaAtivo;
            //----------------------------------

            if (!estaAtivo)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.RotaEncerrarEncaminhamentoAEEEncerrarAutomatico,
                    new FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto(filtro.EncaminhamentoId),  Guid.NewGuid(), null));
                await EnviarLog(logPlanoAee);
                return true;
            }
            await EnviarLog(logPlanoAee);
            return false;
        }
        private async Task EnviarLog(LogPlanoAee planoAee)
        {
            if (planoAee.filtro.EncaminhamentoId == 31449)
            {
                var logPlanoAeeJson = JsonConvert.SerializeObject(planoAee);
                await mediator.Send(new SalvarLogViaRabbitCommand(logPlanoAeeJson, LogNivel.Informacao, LogContexto.WorkerRabbit, rastreamento: "PlanoAEEInfoInconsistente"));
            }

        }
        private class LogPlanoAee
        {
            public FiltroValidarEncerrarEncaminhamentoAEEAutomaticoDto filtro { get; set; }
            public IEnumerable<AlunoPorUeDto> matriculasTurmaAlunoEol { get; set; } 
            public bool estaAtivo { get; set; }
        }

        private readonly SituacaoMatriculaAluno[] SituacoesAtivas = new[]
        {
            SituacaoMatriculaAluno.Ativo,
            SituacaoMatriculaAluno.PendenteRematricula,
            SituacaoMatriculaAluno.Rematriculado,
            SituacaoMatriculaAluno.SemContinuidade
        };
    }
}
