using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
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
            DateTime.TryParse(param.Mensagem?.ToString() ?? string.Empty, out DateTime dataBase);
            var identificadores = await mediator.Send(new ObterIdentificadoresDosMapeamentosDoBimestreAtualQuery(
                                                                                            dataBase.Equals(DateTime.MinValue) 
                                                                                            ? DateTimeExtension.HorarioBrasilia().Date
                                                                                            : dataBase));
           
            foreach(var identificador in identificadores)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ExecutarAtualizacaoMapeamentoEstudantesBimestre, identificador, Guid.NewGuid(), null));

            return true;
        }
    }
}
