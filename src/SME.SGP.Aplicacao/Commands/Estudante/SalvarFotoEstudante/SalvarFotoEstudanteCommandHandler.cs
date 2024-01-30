﻿using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoEstudanteCommandHandler : IRequestHandler<SalvarFotoEstudanteCommand, Guid>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarFotoEstudanteCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Guid> Handle(SalvarFotoEstudanteCommand request, CancellationToken cancellationToken)
        {
            return await GerarFotoAluno(request.AlunoCodigo, request.File);
        }

        private async Task<Guid> GerarFotoAluno(string alunoCodigo, IFormFile file)
        {
            var imagem = ObterImagem(file);
            var miniatura = imagem.GetThumbnailImage(88, 88, () => false, IntPtr.Zero);
            
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var miniaturaId = await GerarFotoMiniatura(miniatura, alunoCodigo, ObterNomeMiniatura(file.FileName), file.ContentType);

                   var codigoArquivo =  await GerarFoto(imagem, alunoCodigo, file.FileName, file.ContentType, miniaturaId);

                    unitOfWork.PersistirTransacao();

                    return codigoArquivo;
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

        }


        private async Task<long> GerarFotoMiniatura(Image foto, string alunoCodigo, string nomeArquivo, string formato, long? miniaturaId = null)
        {
            var arquivo = await mediator.Send(new UploadImagemCommand(foto, Dominio.TipoArquivo.FotoAluno, nomeArquivo, formato));
            return await mediator.Send(new GerarFotoEstudanteCommand(alunoCodigo, arquivo.Id, miniaturaId));
        }

        private async Task<Guid> GerarFoto(Image foto, string alunoCodigo, string nomeArquivo, string formato, long? miniaturaId = null)
        {
            var arquivo = await mediator.Send(new UploadImagemCommand(foto, Dominio.TipoArquivo.FotoAluno, nomeArquivo, formato));
            await mediator.Send(new GerarFotoEstudanteCommand(alunoCodigo, arquivo.Id, miniaturaId));
            return arquivo.Codigo;
        }

        private string ObterNomeMiniatura(string nomeArquivo)
            => $"miniatura_{nomeArquivo}";

        private Image ObterImagem(IFormFile file)
        {
            var stream = file.OpenReadStream();
            
            var image = Image.FromStream(stream);
            
            return image;
        }
    }
}
