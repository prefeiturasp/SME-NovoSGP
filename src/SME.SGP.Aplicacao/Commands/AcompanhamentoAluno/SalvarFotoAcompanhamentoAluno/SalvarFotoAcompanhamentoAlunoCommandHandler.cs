using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoAcompanhamentoAlunoCommandHandler : IRequestHandler<SalvarFotoAcompanhamentoAlunoCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarFotoAcompanhamentoAlunoCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaDto> Handle(SalvarFotoAcompanhamentoAlunoCommand request, CancellationToken cancellationToken)
        {
            var acompanhamentoSemestre = request.Acompanhamento.AcompanhamentoAlunoSemestreId > 0 ?
                await ObterAcompanhametnoSemestre(request.Acompanhamento.AcompanhamentoAlunoSemestreId) :
                await GerarAcompanhamentoSemestre(request.Acompanhamento);
            
            return await GerarFotosSemestre(acompanhamentoSemestre, request.Acompanhamento.File, AuditarSemestre(request.Acompanhamento.AcompanhamentoAlunoSemestreId));
        }

        private bool AuditarSemestre(long acompanhamentoAlunoSemestreId)
            => acompanhamentoAlunoSemestreId > 0;

        private async Task<AuditoriaDto> GerarFotosSemestre(AcompanhamentoAlunoSemestre acompanhamentoSemestre, IFormFile file, bool auditarSemestre)
        {
            var imagem = await ObterImagem(file);
            var miniatura = imagem.GetThumbnailImage(88, 88, () => false, IntPtr.Zero);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var miniaturaId = await GerarFotoSemestre(miniatura, ObterNomeMiniatura(file.FileName), file.ContentType, acompanhamentoSemestre.Id);
                    await GerarFotoSemestre(imagem, file.FileName, file.ContentType, acompanhamentoSemestre.Id, miniaturaId);

                    if (auditarSemestre)
                        await mediator.Send(new SalvarAcompanhamentoAlunoSemestreCommand(acompanhamentoSemestre));
                    unitOfWork.PersistirTransacao();

                    return (AuditoriaDto)acompanhamentoSemestre;
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task<long> GerarFotoSemestre(Image foto, string nomeArquivo, string formato, long acompanhamentoSemestreId, long? miniaturaId = null)
        {
            var arquivo = await mediator.Send(new UploadImagemCommand(foto, Dominio.TipoArquivo.FotoAluno, nomeArquivo, formato));
            return await mediator.Send(new GerarAcompanhamentoAlunoFotoCommand(acompanhamentoSemestreId, arquivo.Id, miniaturaId));
        }

        private async Task<AcompanhamentoAlunoSemestre> ObterAcompanhametnoSemestre(long acompanhamentoAlunoSemestreId)
            => await mediator.Send(new ObterAcompanhamentoAlunoSemestrePorIdQuery(acompanhamentoAlunoSemestreId));

        private async Task<AcompanhamentoAlunoSemestre> GerarAcompanhamentoSemestre(AcompanhamentoAlunoDto acompanhamento)
        {
            var acompanhamentoAlunoId = acompanhamento.AcompanhamentoAlunoId > 0 ?
                acompanhamento.AcompanhamentoAlunoId :
                await mediator.Send(new GerarAcompanhamentoAlunoCommand(acompanhamento.TurmaId, acompanhamento.AlunoCodigo));

            return await mediator.Send(new GerarAcompanhamentoAlunoSemestreCommand(acompanhamentoAlunoId, acompanhamento.Semestre, "", ""));
        }

        private string ObterNomeMiniatura(string nomeArquivo)
            => $"miniatura_{nomeArquivo}";

        private async Task<Image> ObterImagem(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var img = Image.FromStream(memoryStream);
                return img;
            }
        }
    }
}
