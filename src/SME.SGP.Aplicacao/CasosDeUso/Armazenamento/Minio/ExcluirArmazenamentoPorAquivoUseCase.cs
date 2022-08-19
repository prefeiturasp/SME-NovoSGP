using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArmazenamentoPorAquivoUseCase : IExcluirArmazenamentoPorAquivoUseCase
    {
        private readonly IMediator mediator;

        public ExcluirArmazenamentoPorAquivoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = new FiltroExcluirArquivoArmazenamentoDto();
            try
            {
                filtro = param.ObterObjetoMensagem<FiltroExcluirArquivoArmazenamentoDto>();
                return await mediator.Send(new ExcluirArquivoMinioCommand(filtro.ArquivoNome, filtro.BucketNome));
            }
            catch (Exception ex)
            {
               await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível Excluir o Arquivo {filtro.ArquivoNome} no armazenamento do Minio", LogNivel.Critico, LogContexto.Geral, ex.Message,rastreamento:ex.StackTrace,excecaoInterna:ex.InnerException?.ToString()));
                throw;
            }
        }
    }
}