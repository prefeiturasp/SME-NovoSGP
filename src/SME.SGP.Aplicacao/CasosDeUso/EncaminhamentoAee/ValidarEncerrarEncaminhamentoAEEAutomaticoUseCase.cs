using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
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
            if (filtro.EncaminhamentoId == 31449)
            {
                logPlanoAee.filtro = filtro;
                logPlanoAee.matriculasTurmaAlunoEol = matriculasTurmaAlunoEol;
            }
            //----------------------------------

            if (matriculasTurmaAlunoEol.EhNulo() || !matriculasTurmaAlunoEol.Any())
                return false;

            var estaAtivo = matriculasTurmaAlunoEol.Any(c => SituacoesAtivas.Contains(c.CodigoSituacaoMatricula) && c.DataSituacao <= DateTime.Today);

            //log-------------------------------
            if (filtro.EncaminhamentoId == 31449)
            {
                logPlanoAee.estaAtivo = estaAtivo;
            }
            //----------------------------------

            if (!estaAtivo)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.RotaEncerrarEncaminhamentoAEEEncerrarAutomatico,
                    new FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto(filtro.EncaminhamentoId),  Guid.NewGuid(), null));

                return true;
            }

            return false;
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
