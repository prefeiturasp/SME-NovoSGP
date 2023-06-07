using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class SalvarLogViaRabbitCommandHandler : IRequestHandler<SalvarLogViaRabbitCommand, bool>
    {
        private readonly IConexoesRabbitFilasLog conexoesRabbitFilasLog;

        public SalvarLogViaRabbitCommandHandler(IConexoesRabbitFilasLog conexoesRabbitFilasLog)
        {
            this.conexoesRabbitFilasLog = conexoesRabbitFilasLog ?? throw new ArgumentNullException(nameof(conexoesRabbitFilasLog));
        }

        public Task<bool> Handle(SalvarLogViaRabbitCommand request, CancellationToken cancellationToken)
        {
            var logMensagem = new LogMensagem(request.Mensagem,
                                           request.Nivel.ToString(),
                                           request.Contexto.ToString(),
                                           request.Observacao,
                                           request.Projeto,
                                           request.Rastreamento,
                                           request.ExcecaoInterna,
                                           request.InnerException);

            var mensagem = JsonConvert.SerializeObject(logMensagem, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            var body = Encoding.UTF8.GetBytes(mensagem);
            
            var props = conexoesRabbitFilasLog.Get().CreateBasicProperties();
            
            props.Persistent = true;

            conexoesRabbitFilasLog.Get().BasicPublish(ExchangeSgpRabbit.SgpLogs, RotasRabbitLogs.RotaLogs, true, props, body);

            return Task.FromResult(true);
        }
    }
    
}
