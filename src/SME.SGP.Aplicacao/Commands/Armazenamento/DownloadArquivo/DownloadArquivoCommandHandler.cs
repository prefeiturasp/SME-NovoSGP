using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class DownloadArquivoCommandHandler : IRequestHandler<DownloadArquivoCommand, byte[]>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        public DownloadArquivoCommandHandler(IServicoArmazenamento servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }
        public async Task<byte[]> Handle(DownloadArquivoCommand request, CancellationToken cancellationToken)
        {
            var extensao = Path.GetExtension(request.Nome);

            var nomeArquivoComExtensao = $"{request.Codigo}{extensao}";

            var enderecoArquivo = servicoArmazenamento.Obter(nomeArquivoComExtensao, request.Tipo == TipoArquivo.temp);

            if (!string.IsNullOrEmpty(enderecoArquivo))
            {
                if (!enderecoArquivo.StartsWith("http://") && !enderecoArquivo.StartsWith("https://"))
                    enderecoArquivo = "http://" + enderecoArquivo;

                var response = await new HttpClient().GetAsync(enderecoArquivo);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return await response.Content.ReadAsByteArrayAsync(cancellationToken);

                return default;
            }
            throw new NegocioException("A imagem da criança/aluno não foi encontrada.");
        }
    }
}
