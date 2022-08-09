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
    public class DownloadArquivoCommandHandler : IRequestHandler<DownloadArquivoCommand, string>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        public DownloadArquivoCommandHandler(IServicoArmazenamento servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }
        public async Task<string> Handle(DownloadArquivoCommand request, CancellationToken cancellationToken)
        {
            var enderecoArquivo = await servicoArmazenamento.Obter(request.Nome,request.Tipo == TipoArquivo.temp);

            if (string.IsNullOrEmpty(enderecoArquivo))
                return enderecoArquivo;
                
            throw new NegocioException("A imagem da criança/aluno não foi encontrada.");
        }
    }
}
