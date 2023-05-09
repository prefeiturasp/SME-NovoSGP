using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageProcessor;
using SME.SGP.Infra;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class OtimizarImagemCommandHandler : IRequestHandler<OtimizarImagemCommand, bool>
    {
        public Task<bool> Handle(OtimizarImagemCommand request, CancellationToken cancellationToken)
        {
            if (!request.NomeArquivo.EhArquivoImagemParaOtimizar())
                return Task.FromResult(false);

            using ( var imageFactory = new ImageFactory(preserveExifData: false))
            {
                imageFactory.Load(Path.Combine($"{Constantes.Arquivos}/", request.NomeArquivo))
                    .Quality(50)
                    .Save(Path.Combine($"{Constantes.Temp}/", request.NomeArquivo));
            };

            return Task.FromResult(true);
        }
    }
}
