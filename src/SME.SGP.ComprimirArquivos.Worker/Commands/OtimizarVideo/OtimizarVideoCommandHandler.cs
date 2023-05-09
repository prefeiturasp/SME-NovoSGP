using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class OtimizarVideoCommandHandler : IRequestHandler<OtimizarVideoCommand, bool>
    {
        public Task<bool> Handle(OtimizarVideoCommand request, CancellationToken cancellationToken)
        {
            //if (!request.NomeArquivo.EhArquivoImagemOuVideoParaOtimizar())
            //    return Task.FromResult(false);

            //var outStream = new MemoryStream();

            //var input = @"C:\Repo\FFMpeg-Converter\FFMpeg-Converter\FFMpeg-Converter\Arquivos\VID_20230505_150506115.mp4";               

            //Console.WriteLine($"Iniciando a conversão da imagem {input}!");

            //var output = @"C:\Repo\FFMpeg-Converter\FFMpeg-Converter\FFMpeg-Converter\Arquivos\" + Guid.NewGuid() + ".mp4";                

            //FFMpeg.Convert(input, output, VideoType.Mp4, Speed.UltraFast, VideoSize.Ld, AudioQuality.Low, true);

            return Task.FromResult(true);
        }
    }
}
