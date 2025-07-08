using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DownloadArquivoInformativoUseCase : DownloadDeArquivoUseCase, IDownloadArquivoInformativoUseCase
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        public DownloadArquivoInformativoUseCase(IMediator mediator, IServicoArmazenamento servicoArmazenamento) : base(mediator, servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento;
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
