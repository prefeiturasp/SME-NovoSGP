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

namespace SME.SGP.Aplicacao
{
    public class ArmazenarImagemFisicaCommandHandler : IRequestHandler<ArmazenarImagemFisicaCommand, bool>
    {
        public Task<bool> Handle(ArmazenarImagemFisicaCommand request, CancellationToken cancellationToken)
        {
            var nomeArquivo = request.NomeFisico + Path.GetExtension(request.NomeArquivo);
            var caminho = Path.Combine(request.Caminho, nomeArquivo);

            var bitmap = new Bitmap(request.Imagem);
            bitmap.Save(caminho, ObterFormato(request.Formato));
      
            return Task.FromResult(true);
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
