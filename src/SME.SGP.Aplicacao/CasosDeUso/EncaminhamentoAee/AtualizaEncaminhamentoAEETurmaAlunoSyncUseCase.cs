using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizaEncaminhamentoAEETurmaAlunoSyncUseCase : AbstractUseCase, IAtualizaEncaminhamentoAEETurmaAlunoSyncUseCase
    {
        public AtualizaEncaminhamentoAEETurmaAlunoSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var encaminhamentos = await mediator.Send(new ObterEncaminhamentoAEEVigenteQuery(DateTime.Now.Year));

            if (encaminhamentos != null && encaminhamentos.Any())
                foreach (var encaminhamento in encaminhamentos)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.AtualizarTabelaEncaminhamentoAEETurmaAlunoTratar, new SalvarEncaminhamentoAEETurmaAlunoCommand(encaminhamento.EncaminhamentoId, encaminhamento.AlunoCodigo)));

            return true;
        }
    }
}
