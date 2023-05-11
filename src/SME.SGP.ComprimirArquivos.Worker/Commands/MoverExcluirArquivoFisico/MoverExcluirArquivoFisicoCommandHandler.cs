using System;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageProcessor;
using SME.SGP.Infra;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class MoverExcluirArquivoFisicoCommandHandler : IRequestHandler<MoverExcluirArquivoFisicoCommand, bool>
    {
        public Task<bool> Handle(MoverExcluirArquivoFisicoCommand request, CancellationToken cancellationToken)
        {
            var arquivoComprimido = new FileInfo(request.NomeArquivoDestino);
            
            if (arquivoComprimido.Length > new FileInfo(request.NomeArquivoOrigem).Length)
                arquivoComprimido.Delete();
            else
                File.Move(request.NomeArquivoDestino, request.NomeArquivoOrigem, true);

            return Task.FromResult(true);
        }
    }
}
