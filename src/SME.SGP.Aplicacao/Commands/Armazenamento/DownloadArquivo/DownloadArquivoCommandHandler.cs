using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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

            try
            {
                var arquivo = File.ReadAllBytes(caminhoArquivo);

                if (arquivo != null)
                    return Task.FromResult(arquivo);

                throw new NegocioException("A imagem da criança/aluno não foi encontrada");
            }
            catch (Exception)
            {
                throw new NegocioException("A imagem da criança/aluno não foi encontrada.");
            }
         

        }

        private string ObterCaminhoArquivos(TipoArquivo tipo)
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ArquivoContants.PastaAquivos, tipo.Name());

    }
}
