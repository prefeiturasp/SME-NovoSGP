using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase : AbstractUseCase, IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase
    {
        public ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            

            return false;
        }
    }
}
