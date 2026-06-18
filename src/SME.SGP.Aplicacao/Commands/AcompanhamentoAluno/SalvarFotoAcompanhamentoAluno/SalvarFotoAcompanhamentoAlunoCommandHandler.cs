using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Servicos.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoAcompanhamentoAlunoCommandHandler : IRequestHandler<SalvarFotoAcompanhamentoAlunoCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IProcessadorImagens _processadorImagens;

        public SalvarFotoAcompanhamentoAlunoCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IProcessadorImagens processadorImagens)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _processadorImagens = processadorImagens ?? throw new ArgumentNullException(nameof(processadorImagens));
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
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    // Obtém a imagem original em bytes
                    var imagemBytes = await _processadorImagens.ObterImagemEmBytesAsync(file);

                    // Cria miniatura (88x88)
                    var miniaturaBytes = await _processadorImagens.CriarMiniaturaAsync(imagemBytes, 88, 88);
                    var tipoConteudo = _processadorImagens.ObterTipoConteudo(file.FileName);

                    // Salva miniatura
                    var miniaturaId = await GerarFotoSemestre(
                        miniaturaBytes,
                        ObterNomeMiniatura(file.FileName),
                        tipoConteudo,
                        acompanhamentoSemestre.Id);

                    // Salva imagem original
                    await GerarFotoSemestre(
                        imagemBytes,
                        file.FileName,
                        tipoConteudo,
                        acompanhamentoSemestre.Id,
                        miniaturaId);

                    if (auditarSemestre)
                        await mediator.Send(new SalvarAcompanhamentoAlunoSemestreCommand(acompanhamentoSemestre));

                    unitOfWork.PersistirTransacao();

                    return (AuditoriaDto)acompanhamentoSemestre;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task<long> GerarFotoSemestre(byte[] fotoBytes, string nomeArquivo, string tipoConteudo, long acompanhamentoSemestreId, long? miniaturaId = null)
        {
            var arquivo = await mediator.Send(new UploadImagemCommand(fotoBytes, Dominio.TipoArquivo.FotoAluno, nomeArquivo, tipoConteudo));
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
    }
}
