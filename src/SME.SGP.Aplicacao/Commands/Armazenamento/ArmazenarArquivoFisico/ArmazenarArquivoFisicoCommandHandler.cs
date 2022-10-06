using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarArquivoFisicoCommandHandler : IRequestHandler<ArmazenarArquivoFisicoCommand, string>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        
        public ArmazenarArquivoFisicoCommandHandler(IServicoArmazenamento servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }  
        public async Task<string> Handle(ArmazenarArquivoFisicoCommand request, CancellationToken cancellationToken)
        {
            var stream = request.Arquivo.OpenReadStream();
            
            var extensao = Path.GetExtension(request.Arquivo.FileName);
            
            var nomeArquivo = $"{request.NomeFisico}{extensao}";
                
            if (request.TipoArquivo == TipoArquivo.temp || request.TipoArquivo == TipoArquivo.Editor)
               return await servicoArmazenamento.ArmazenarTemporaria(nomeArquivo,stream,request.Arquivo.ContentType);
            else
               return await servicoArmazenamento.Armazenar(nomeArquivo,stream, request.Arquivo.ContentType);
        }
    }
}
