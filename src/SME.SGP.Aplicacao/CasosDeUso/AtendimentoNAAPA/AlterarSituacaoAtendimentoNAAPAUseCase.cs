using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class AlterarSituacaoAtendimentoNAAPAUseCase : IAlterarSituacaoAtendimentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public AlterarSituacaoAtendimentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long atendimentoId)
        {         
            var atendimento = await mediator.Send(new ObterAtendimentoNAAPAPorIdQuery(atendimentoId));
            
            if (atendimento == null)
                return false;

            return await mediator.Send(new AtualizarSituacaoAtendimentoNAAPACommand(atendimentoId));
        }
    }
}
