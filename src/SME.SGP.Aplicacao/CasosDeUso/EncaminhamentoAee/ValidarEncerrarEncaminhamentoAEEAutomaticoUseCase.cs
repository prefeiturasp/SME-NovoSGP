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

            var matriculasTurmaAlunoEol = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(filtro.TurmaCodigo, filtro.AlunoCodigo));

            if (matriculasTurmaAlunoEol == null || !matriculasTurmaAlunoEol.Any())
                return false;

            bool concluiuTurmaAnoLetivo = false;

            if (filtro.AnoLetivo != DateTime.Today.Year)
            {
                var matriculasAlunoEol = await mediator.Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(filtro.AlunoCodigo, filtro.AnoLetivo));
                concluiuTurmaAnoLetivo = matriculasAlunoEol.Any(c => c.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);
            }

            if (matriculasTurmaAlunoEol.Any(c => c.EstaInativo(DateTime.Today)) || concluiuTurmaAnoLetivo)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaEncerrarEncaminhamentoAEEEncerrarAutomatico,
                    new FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto(filtro.EncaminhamentoId), new Guid(), null));

                return true;
            }

            return false;
        }
    }
}
