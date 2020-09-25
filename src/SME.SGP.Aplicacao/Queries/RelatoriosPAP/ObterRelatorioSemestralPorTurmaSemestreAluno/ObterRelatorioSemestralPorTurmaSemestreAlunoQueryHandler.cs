using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioSemestralPorTurmaSemestreAlunoQueryHandler : IRequestHandler<ObterRelatorioSemestralPorTurmaSemestreAlunoQuery, RelatorioSemestralAlunoDto>
    {
        private readonly IRepositorioRelatorioSemestralPAPAluno repositorioRelatorioSemestralAluno;
        private readonly IRepositorioRelatorioSemestralTurmaPAP repositorioRelatorioSemestralTurmaPAP;

        public ObterRelatorioSemestralPorTurmaSemestreAlunoQueryHandler(IRepositorioRelatorioSemestralPAPAluno repositorioRelatorioSemestralAluno,
                                                                        IRepositorioRelatorioSemestralTurmaPAP repositorioRelatorioSemestralTurmaPAP)
        {
            this.repositorioRelatorioSemestralAluno = repositorioRelatorioSemestralAluno ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestralAluno));
            this.repositorioRelatorioSemestralTurmaPAP = repositorioRelatorioSemestralTurmaPAP ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestralTurmaPAP));
        }

        public async Task<RelatorioSemestralAlunoDto> Handle(ObterRelatorioSemestralPorTurmaSemestreAlunoQuery request, CancellationToken cancellationToken)
        {
            var relatorioSemestralAluno = await repositorioRelatorioSemestralAluno.ObterRelatorioSemestralPorAlunoTurmaSemestreAsync(request.AlunoCodigo, request.TurmaCodigo, request.Semestre);
            var relatorioSemestral = relatorioSemestralAluno?.RelatorioSemestralTurmaPAP ?? await repositorioRelatorioSemestralTurmaPAP.ObterPorTurmaCodigoSemestreAsync(request.TurmaCodigo, request.Semestre);

            var relatorioSemestralAlunoDto = new RelatorioSemestralAlunoDto();
            if (relatorioSemestralAluno != null || relatorioSemestral != null) 
                relatorioSemestralAlunoDto = ConverterParaDto(relatorioSemestralAluno, relatorioSemestral);

            var dataReferencia = DateTime.Today;

            var secoes = await repositorioRelatorioSemestralAluno.ObterDadosSecaoPorRelatorioSemestralAlunoIdDataReferenciaAsync(relatorioSemestralAlunoDto.RelatorioSemestralAlunoId, dataReferencia);

            relatorioSemestralAlunoDto.Secoes = secoes;

            return relatorioSemestralAlunoDto;
        }

        private RelatorioSemestralAlunoDto ConverterParaDto(RelatorioSemestralPAPAluno relatorioAluno, RelatorioSemestralTurmaPAP relatorioTurma)
        {
            var dto = new RelatorioSemestralAlunoDto()
            {
                RelatorioSemestralAlunoId = relatorioAluno?.Id ?? 0,
                RelatorioSemestralId = relatorioTurma?.Id ?? 0
            };

            if (relatorioAluno != null)
                dto.Auditoria = (AuditoriaDto) relatorioAluno;

            return dto;
        }
    }
}
