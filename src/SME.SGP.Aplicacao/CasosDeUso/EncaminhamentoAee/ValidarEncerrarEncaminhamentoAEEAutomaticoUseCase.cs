using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
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

            var matriculasTurmaAlunoEol = await mediator.Send(new ObterMatriculasAlunoNaUEQuery(filtro.UeCodigo, filtro.AlunoCodigo));

            if (matriculasTurmaAlunoEol.EhNulo() || !matriculasTurmaAlunoEol.Any())
                return false;

            var estaAtivo = matriculasTurmaAlunoEol.Any(c => SituacoesAtivas.Contains(c.CodigoSituacaoMatricula) && c.DataSituacao <= DateTime.Today);

            if (!estaAtivo)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.RotaEncerrarEncaminhamentoAEEEncerrarAutomatico,
                    new FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto(filtro.EncaminhamentoId),  Guid.NewGuid(), null));

                return true;
            }

            return false;
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
