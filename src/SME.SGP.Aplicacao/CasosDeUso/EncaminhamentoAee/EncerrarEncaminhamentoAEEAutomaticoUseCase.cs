using MediatR;
using SME.SGP.Aplicacao.Interfaces;
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
                var matriculasAlunoEol = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(encaminhamento.AlunoCodigo, encaminhamento.TurmaCodigo));

                if (matriculasAlunoEol == null || !matriculasAlunoEol.Any())
                    continue;

                if (!matriculasAlunoEol.Any(c => c.EstaAtivo(DateTime.Now)))
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaAtualizarEncaminhamentoAEEEncerrarAutomatico,
                        new FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto(encaminhamento.EncaminhamentoId), new Guid(), null));
                }
            }

            return true;
        }
    }
}
