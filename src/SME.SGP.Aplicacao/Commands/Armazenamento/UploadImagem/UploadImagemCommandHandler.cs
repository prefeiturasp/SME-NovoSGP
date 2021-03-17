using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class UploadImagemCommandHandler : IRequestHandler<UploadImagemCommand, ArquivoArmazenadoDto>
    {
        private readonly IMediator mediator;

        public UploadImagemCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ArquivoArmazenadoDto> Handle(UploadImagemCommand request, CancellationToken cancellationToken)
        {
            var caminhoArquivo = ObterCaminhoArquivo(request.TipoArquivo);

            var arquivo = await mediator.Send(new SalvarArquivoRepositorioCommand(request.NomeArquivo, request.TipoArquivo, request.Formato));
            await mediator.Send(new ArmazenarImagemFisicaCommand(request.Imagem, arquivo.Codigo.ToString(), request.NomeArquivo, caminhoArquivo, request.Formato));

            return arquivo;
        }

        private string ObterCaminhoArquivo(TipoArquivo tipo)
        {
            var caminho = Path.Combine(ObterCaminhoArquivos(), tipo.ToString());
            return VerificaCaminhoExiste(caminho);
        }

        private string ObterCaminhoArquivos()
        {
            var caminho = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arquivos");
            return VerificaCaminhoExiste(caminho);
        }

        private string VerificaCaminhoExiste(string caminho)
        {
            if (!Directory.Exists(caminho))
                Directory.CreateDirectory(caminho);

            return caminho;
        }
    }
}
