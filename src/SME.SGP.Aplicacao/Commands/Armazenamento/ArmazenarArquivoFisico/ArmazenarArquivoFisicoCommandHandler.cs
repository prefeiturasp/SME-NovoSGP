using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarArquivoFisicoCommandHandler : IRequestHandler<ArmazenarArquivoFisicoCommand, bool>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        
        public ArmazenarArquivoFisicoCommandHandler(IServicoArmazenamento servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }  
        public Task<bool> Handle(ArmazenarArquivoFisicoCommand request, CancellationToken cancellationToken)
        {
            var stream = request.Arquivo.OpenReadStream();
            
            if (request.TipoArquivo == TipoArquivo.temp)
               servicoArmazenamento.ArmazenarTemporaria(request.NomeFisico,stream,request.Arquivo.ContentType);
            else
               servicoArmazenamento.Armazenar(request.NomeFisico,stream, request.Arquivo.ContentType);
            
            return Task.FromResult(true);
        }
    }
}
