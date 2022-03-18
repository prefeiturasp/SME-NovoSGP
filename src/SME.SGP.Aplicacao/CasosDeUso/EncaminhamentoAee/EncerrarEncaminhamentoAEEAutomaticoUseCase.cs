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
    public class EncerrarEncaminhamentoAEEAutomaticoUseCase : AbstractUseCase, IEncerrarEncaminhamentoAEEAutomaticoUseCase
    {
        public EncerrarEncaminhamentoAEEAutomaticoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var encaminhamentos = await mediator.Send(new ObterEncaminhamentoAEEEncerrarAutomaticoQuery());

            foreach (var encaminhamento in encaminhamentos)
            {
                var matriculasAnoAlunoEol = await mediator.Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(encaminhamento.AlunoCodigo, DateTime.Today.Year));
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(encaminhamento.TurmaCodigo));

                if (matriculasAnoAlunoEol == null)
                    continue;

                var matriculasInativas = matriculasAnoAlunoEol.Where(c => c.EstaInativo(DateTime.Now));

                /* TODO:
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaAtualizarEncaminhamentoAEEEncerrarAutomatico,
                    new FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto(encaminhamento.EncaminhamentoId), new Guid(), null));
                */
            }

            return true;
        }

        private async Task<bool> DeterminaEtapaConcluida(string alunoCodigo, int anoLetivo)
        {
            var matriculasAnoTurmaEol = await mediator.Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(alunoCodigo, anoLetivo));
            var concluiuAnoTurma = matriculasAnoTurmaEol.Any(c => c.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);
            return true;
        }
    }
}
