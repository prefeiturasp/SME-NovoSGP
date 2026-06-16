using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interface;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarImagemFisicaCommandHandler : IRequestHandler<ArmazenarImagemFisicaCommand, bool>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;

        public ArmazenarImagemFisicaCommandHandler(IServicoArmazenamento servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }

        public async Task<bool> Handle(ArmazenarImagemFisicaCommand request, CancellationToken cancellationToken)
        {
            using (var msImagem = new MemoryStream(request.ImagemBytes))
            {
                if (request.TipoArquivo == TipoArquivo.temp || request.TipoArquivo == TipoArquivo.Editor)
                    await servicoArmazenamento.ArmazenarTemporaria(request.NomeFisico, msImagem, request.Formato);
                else
                    await servicoArmazenamento.Armazenar(request.NomeFisico, msImagem, request.Formato);
             }

            return true;
        }
    }
}