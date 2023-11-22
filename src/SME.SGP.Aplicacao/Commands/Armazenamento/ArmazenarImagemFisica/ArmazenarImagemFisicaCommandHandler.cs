using MediatR;
using SME.SGP.Dominio;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarImagemFisicaCommandHandler : IRequestHandler<ArmazenarImagemFisicaCommand, bool>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        
        public ArmazenarImagemFisicaCommandHandler(IServicoArmazenamento servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }
        public async Task<bool> Handle(ArmazenarImagemFisicaCommand request, CancellationToken cancellationToken)
        {
            var msImagem = new MemoryStream();
            
            request.Imagem.Save(msImagem, ObterFormato(request.Formato));

            msImagem.Seek(0, SeekOrigin.Begin);
            
            if (request.TipoArquivo == TipoArquivo.temp || request.TipoArquivo == TipoArquivo.Editor)
                await servicoArmazenamento.ArmazenarTemporaria(request.NomeFisico,msImagem,request.Formato);
            else
                await servicoArmazenamento.Armazenar(request.NomeFisico,msImagem, request.Formato);
            
            return true;
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
