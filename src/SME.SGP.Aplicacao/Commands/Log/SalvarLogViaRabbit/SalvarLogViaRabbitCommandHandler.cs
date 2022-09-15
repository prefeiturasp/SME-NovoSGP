using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarLogViaRabbitCommandHandler : IRequestHandler<SalvarLogViaRabbitCommand, bool>
    {
        private readonly IServicoMensageriaLogs servicoMensageria;

        public SalvarLogViaRabbitCommandHandler(IServicoMensageriaLogs servicoMensageria)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }

        public Task<bool> Handle(SalvarLogViaRabbitCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new LogMensagem(request.Mensagem,
                                           request.Nivel.ToString(),
                                           request.Contexto.ToString(),
                                           request.Observacao,
                                           request.Projeto,
                                           request.Rastreamento,
                                           request.ExcecaoInterna,
                                           request.InnerException);

            return servicoMensageria.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog");
        }
    }
    
}
