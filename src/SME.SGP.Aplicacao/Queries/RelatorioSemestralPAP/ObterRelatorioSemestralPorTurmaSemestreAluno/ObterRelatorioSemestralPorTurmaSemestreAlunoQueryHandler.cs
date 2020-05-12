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
        private readonly IRepositorioRelatorioSemestralAluno repositorioRelatorioSemestralAluno;

        public ObterRelatorioSemestralPorTurmaSemestreAlunoQueryHandler(IRepositorioRelatorioSemestralAluno repositorioRelatorioSemestralAluno)
        {
            this.repositorioRelatorioSemestralAluno = repositorioRelatorioSemestralAluno ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestralAluno));
        }
        public async Task<RelatorioSemestralAlunoDto> Handle(ObterRelatorioSemestralPorTurmaSemestreAlunoQuery request, CancellationToken cancellationToken)
        {
            var relatorioSemestralAluno = await repositorioRelatorioSemestralAluno.ObterRelatorioSemestralPorAlunoTurmaSemestreAsync(request.AlunoCodigo, request.TurmaCodigo, request.Semestre);

            var relatorioSemestralAlunoDto = new RelatorioSemestralAlunoDto();
            if (relatorioSemestralAluno != null) relatorioSemestralAlunoDto = ConverterParaDto(relatorioSemestralAluno);

            var dataReferencia = DateTime.Today;

            var secoes = await repositorioRelatorioSemestralAluno.ObterDadosSecaoPorRelatorioSemestralAlunoIdDataReferenciaAsync(relatorioSemestralAlunoDto.RelatorioSemestralAlunoId, dataReferencia);

            relatorioSemestralAlunoDto.Secoes = secoes;


            return relatorioSemestralAlunoDto;
        }

        private RelatorioSemestralAlunoDto ConverterParaDto(RelatorioSemestralAluno relatorio)
        {
            var dto = new RelatorioSemestralAlunoDto()
            {
                RelatorioSemestralAlunoId = relatorio.Id,
                RelatorioSemestralId = relatorio.RelatorioSemestralId
            };

            dto.Auditoria = new AuditoriaDto()
            {
                AlteradoEm = relatorio.AlteradoEm,
                AlteradoRF = relatorio.AlteradoRF,
                AlteradoPor = relatorio.AlteradoPor,
                CriadoEm = relatorio.CriadoEm,
                CriadoPor = relatorio.CriadoPor,
                CriadoRF = relatorio.CriadoRF,
            };

            return dto;
        }
    }
}
