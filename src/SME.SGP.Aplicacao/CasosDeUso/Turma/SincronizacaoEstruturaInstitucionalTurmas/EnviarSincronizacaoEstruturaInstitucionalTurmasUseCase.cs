using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarSincronizacaoEstruturaInstitucionalTurmasUseCase : AbstractUseCase, IEnviarSincronizacaoEstruturaInstitucionalTurmasUseCase
    {
        public EnviarSincronizacaoEstruturaInstitucionalTurmasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            throw new NotImplementedException();
        }
    }
}
