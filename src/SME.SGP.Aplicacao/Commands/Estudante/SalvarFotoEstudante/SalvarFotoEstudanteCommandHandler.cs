using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Servicos.Interfaces;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoEstudanteCommandHandler : IRequestHandler<SalvarFotoEstudanteCommand, Guid>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IProcessadorImagens processadorImagens;

        public SalvarFotoEstudanteCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IProcessadorImagens processadorImagens)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.processadorImagens = processadorImagens ?? throw new ArgumentNullException(nameof(processadorImagens));
        }

        public async Task<Guid> Handle(SalvarFotoEstudanteCommand request, CancellationToken cancellationToken)
        {
            return await GerarFotoAluno(request.AlunoCodigo, request.File);
        }

        private async Task<Guid> GerarFotoAluno(string alunoCodigo, IFormFile file)
        {
            // Obtém a imagem em bytes
            var imagemBytes = await processadorImagens.ObterImagemEmBytesAsync(file);

            // Cria miniatura (88x88)
            var miniaturaBytes = await processadorImagens.CriarMiniaturaAsync(imagemBytes, 88, 88);
            var tipoConteudo = processadorImagens.ObterTipoConteudo(file.FileName);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    // Salva miniatura
                    var miniaturaId = await GerarFotoMiniatura(
                        miniaturaBytes,
                        alunoCodigo,
                        ObterNomeMiniatura(file.FileName),
                        tipoConteudo);

                    // Salva imagem original
                    var codigoArquivo = await GerarFoto(
                        imagemBytes,
                        alunoCodigo,
                        file.FileName,
                        tipoConteudo,
                        miniaturaId);

                    unitOfWork.PersistirTransacao();

                    return codigoArquivo;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task<long> GerarFotoMiniatura(byte[] fotoBytes, string alunoCodigo, string nomeArquivo, string tipoConteudo, long? miniaturaId = null)
        {
            var arquivo = await mediator.Send(new UploadImagemCommand(fotoBytes, Dominio.TipoArquivo.FotoAluno, nomeArquivo, tipoConteudo));
            return await mediator.Send(new GerarFotoEstudanteCommand(alunoCodigo, arquivo.Id, miniaturaId));
        }

        private async Task<Guid> GerarFoto(byte[] fotoBytes, string alunoCodigo, string nomeArquivo, string tipoConteudo, long? miniaturaId = null)
        {
            var arquivo = await mediator.Send(new UploadImagemCommand(fotoBytes, Dominio.TipoArquivo.FotoAluno, nomeArquivo, tipoConteudo));
            await mediator.Send(new GerarFotoEstudanteCommand(alunoCodigo, arquivo.Id, miniaturaId));
            return arquivo.Codigo;
        }

        private string ObterNomeMiniatura(string nomeArquivo)
            => $"miniatura_{nomeArquivo}";
    }
}