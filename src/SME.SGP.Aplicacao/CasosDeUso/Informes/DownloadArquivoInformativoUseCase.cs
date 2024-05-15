using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DownloadArquivoInformativoUseCase : DownloadDeArquivoUseCase, IDownloadArquivoInformativoUseCase
    {
        public DownloadArquivoInformativoUseCase(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task<Arquivo> ObterArquivoPorCodigo(Guid codigoArquivo)
        {
            var arquivo = await base.ObterArquivoPorCodigo(codigoArquivo);
            if (arquivo.Tipo != TipoArquivo.Informativo)
                throw new NegocioException(MensagemNegocioInformes.TIPO_INVALIDO_ARQUIVO_DOWNLOAD);
            return arquivo;
        }
    }
}
