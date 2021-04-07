using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaSincronizacaoEstruturaInstitucionalTurmaUseCase : AbstractUseCase, IExecutaSincronizacaoEstruturaInstitucionalTurmaUseCase
    {
        public ExecutaSincronizacaoEstruturaInstitucionalTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<bool> Executar(MensagemRabbit mensagem)
        {
            throw new NotImplementedException();
        }
    }
}
