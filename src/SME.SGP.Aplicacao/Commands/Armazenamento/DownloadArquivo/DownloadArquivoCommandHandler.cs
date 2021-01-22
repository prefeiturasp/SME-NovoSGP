using MediatR;
using SME.SGP.Dominio;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DownloadArquivoCommandHandler : IRequestHandler<DownloadArquivoCommand, byte[]>
    {

        public Task<byte[]> Handle(DownloadArquivoCommand request, CancellationToken cancellationToken)
        {
            var caminhoBase = ObterCaminhoArquivos(request.Tipo);
            var extencao = Path.GetExtension(request.Nome);
            var nomeArquivo = $"{request.Codigo}{extencao}";
            var caminhoArquivo = Path.Combine($"{caminhoBase}", nomeArquivo);

            var arquivo = File.ReadAllBytes(caminhoArquivo);

            if (arquivo != null)
                return Task.FromResult(arquivo);

            throw new NegocioException("Não foi possível fazer download do arquivo");
        }

        private string ObterCaminhoArquivos(TipoArquivo tipo)
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arquivos", tipo.ToString());

    }
}
