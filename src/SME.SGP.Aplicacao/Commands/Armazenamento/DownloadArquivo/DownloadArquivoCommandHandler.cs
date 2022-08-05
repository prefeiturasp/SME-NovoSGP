using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;

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
            var enderecoArquivo = await servicoArmazenamento.Obter(request.Nome,request.Tipo == TipoArquivo.temp);

            if (string.IsNullOrEmpty(enderecoArquivo))
            {
                var arquivo = File.ReadAllBytes(enderecoArquivo);
            
                if (arquivo == null)
                    arquivo = Array.Empty<byte>();

                return await Task.FromResult(new byte[5]);
            }
            else
            {
                throw new NegocioException("A imagem da criança/aluno não foi encontrada.");
            }

        }
    }
}
