using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class UploadArquivoItineranciaCommand :  IRequest<ArquivoArmazenadoItineranciaDto>
    {
        public UploadArquivoItineranciaCommand(IFormFile arquivo,TipoConteudoArquivo tipoConteudo = TipoConteudoArquivo.Indefinido)
        {
            Arquivo = arquivo;
            TipoConteudo = tipoConteudo;
        }
        public IFormFile Arquivo { get; set; }
        public TipoConteudoArquivo TipoConteudo { get; set; }
    }
}