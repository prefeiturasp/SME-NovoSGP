using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase : AbstractUseCase, INotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase
    {
        public NotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<bool> Executar(MensagemRabbit param)
        {
            throw new NotImplementedException();
        }
    }
}
