using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarImagemFisicaCommandHandler : IRequestHandler<ArmazenarImagemFisicaCommand, bool>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly IMediator mediator;
        
        public ArmazenarImagemFisicaCommandHandler(IServicoArmazenamento servicoArmazenamento, IMediator mediator)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(ArmazenarImagemFisicaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var msImagem = new MemoryStream();
                 
                request.Imagem.Save(msImagem,ObterFormato(request.Formato));
                
                msImagem.Seek(0, SeekOrigin.Begin);
                
                if (request.TipoArquivo == TipoArquivo.temp || request.TipoArquivo == TipoArquivo.Editor)
                    await servicoArmazenamento.ArmazenarTemporaria(request.NomeFisico,msImagem,request.Formato);
                else
                    await servicoArmazenamento.Armazenar(request.NomeFisico,msImagem, request.Formato);
                    
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Falha ao armazenar imagem física do arquivo {ex.Message}",
                    LogNivel.Critico,
                    LogContexto.Arquivos));
            }
            return false;
        }

        private ImageFormat ObterFormato(string formato)
        {
            switch (formato)
            {
                case "image/jpeg":
                    return ImageFormat.Jpeg;
                case "image/bmp":
                    return ImageFormat.Bmp;
                case "image/emf":
                    return ImageFormat.Emf;
                case "image/exif":
                    return ImageFormat.Exif;
                case "image/gif":
                    return ImageFormat.Gif;
                case "image/icon":
                    return ImageFormat.Icon;
                case "image/png":
                    return ImageFormat.Png;
                case "image/tiff":
                    return ImageFormat.Tiff;
                case "image/wmf":
                    return ImageFormat.Wmf;
                default: 
                    throw new NegocioException("Formato da imagem não identificado");
            }
        }
    }
}
