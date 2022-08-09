using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class DownloadDeArquivoUseCase : AbstractUseCase, IDownloadDeArquivoUseCase
    {
        public DownloadDeArquivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<(byte[], string, string)> Executar(Guid codigoArquivo)
        {
            try
            {
                var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(codigoArquivo));

                var arquivoFisico = await mediator.Send(new DownloadArquivoCommand(codigoArquivo, entidadeArquivo.Nome, entidadeArquivo.Tipo));

                return (arquivoFisico, entidadeArquivo.TipoConteudo, entidadeArquivo.Nome);
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Falha ao realizar o download do arquivo {ex.Message}",
                    LogNivel.Critico,
                    LogContexto.Arquivos));
            }

            return (null, string.Empty, string.Empty);
        }
    }
}
