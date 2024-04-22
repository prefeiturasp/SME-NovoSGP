using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarMapeamentoDosEstudantesUseCase : AbstractUseCase, IAtualizarMapeamentoDosEstudantesUseCase
    {
        public AtualizarMapeamentoDosEstudantesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var identificadores = await mediator.Send(new ObterIdentificadoresDosMapeamentosDoBimestreAtualQuery());
           
            foreach(var identificador in identificadores)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ExecutarAtualizacaoMapeamentoEstudantesBimestre, identificador, Guid.NewGuid(), null));

            return true;
        }
    }
}
