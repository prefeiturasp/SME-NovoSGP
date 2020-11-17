using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarArquivoFisicoCommandHandler : IRequestHandler<ArmazenarArquivoFisicoCommand, bool>
    {
        public Task<bool> Handle(ArmazenarArquivoFisicoCommand request, CancellationToken cancellationToken)
        {
            var nomeArquivo = request.NomeFisico + Path.GetExtension(request.Arquivo.FileName);
            var caminho = Path.Combine(request.Caminho, nomeArquivo);

            using (var strem = File.Create(caminho))
            {
                request.Arquivo.CopyTo(strem);
            }

            return Task.FromResult(true);
        }
    }
}
