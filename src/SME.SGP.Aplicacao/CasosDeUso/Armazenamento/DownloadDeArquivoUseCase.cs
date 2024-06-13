using MediatR;
using SME.SGP.Dominio;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DownloadDeArquivoUseCase : AbstractUseCase, IDownloadDeArquivoUseCase
    {
        public DownloadDeArquivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async virtual Task<(byte[], string, string)> Executar(Guid codigoArquivo)
        {
            var entidadeArquivo = await ObterArquivoPorCodigo(codigoArquivo);
            
            var extensao = Path.GetExtension(entidadeArquivo.Nome);

            var nomeArquivoComExtensao = $"{codigoArquivo}{extensao}";

            var arquivoFisico = await mediator.Send(new DownloadArquivoCommand(codigoArquivo, nomeArquivoComExtensao, entidadeArquivo.Tipo));

            return (arquivoFisico, entidadeArquivo.TipoConteudo, entidadeArquivo.Nome);
        }

        protected virtual Task<Arquivo> ObterArquivoPorCodigo(Guid codigoArquivo)
            => mediator.Send(new ObterArquivoPorCodigoQuery(codigoArquivo));
    }
}
