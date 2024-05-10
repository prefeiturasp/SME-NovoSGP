using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class UploadArquivoItineranciaCommandHandler : IRequestHandler<UploadArquivoItineranciaCommand,ArquivoArmazenadoItineranciaDto>
    {
        private readonly IMediator mediator;

        public UploadArquivoItineranciaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ArquivoArmazenadoItineranciaDto> Handle(UploadArquivoItineranciaCommand request, CancellationToken cancellationToken)
        {
            if (request.TipoConteudo != TipoConteudoArquivo.Indefinido &&
                request.Arquivo.ContentType != request.TipoConteudo.Name())
                throw new NegocioException(MensagemNegocioComuns.FORMATO_ARQUIVO_NAO_ACEITO);
            
            var nomeArquivo = request.Arquivo.FileName;
            var arquivo = await mediator.Send(new SalvarArquivoRepositorioCommand(nomeArquivo, TipoArquivo.Itinerancia, request.Arquivo.ContentType));
            arquivo.Path = await mediator.Send(new ArmazenarArquivoFisicoCommand(request.Arquivo, arquivo.Codigo.ToString(), TipoArquivo.Itinerancia));

            return MapearDto(arquivo);
        }

        private ArquivoArmazenadoItineranciaDto MapearDto(ArquivoArmazenadoDto armazenadoDto)
        {
            return new ArquivoArmazenadoItineranciaDto(armazenadoDto.Id,armazenadoDto.Codigo,armazenadoDto.Path);
        }
    }
}