using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQueryHandler : ConsultasBase, IRequestHandler<ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQuery, PaginacaoResultadoDto<JustificativaAlunoDto>>
    {
        public IMediator mediator { get; }
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;

        public ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }
        public async Task<PaginacaoResultadoDto<JustificativaAlunoDto>> Handle(ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQuery request, CancellationToken cancellationToken)
        {
            return MapearParaDto(await repositorioAnotacaoFrequenciaAluno.ObterPorTurmaAlunoComponenteCurricularBimestrePaginado(
                request.TurmaId,
                request.AlunoCodigo, 
                request.ComponenteCurricularId, 
                request.Bimestre, 
                Paginacao,
                request.Semestre));
        }

        private PaginacaoResultadoDto<JustificativaAlunoDto> MapearParaDto(PaginacaoResultadoDto<JustificativaAlunoDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<JustificativaAlunoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = MapearFrequenciasParaDto(resultadoDto.Items)
            };
        }

        private IEnumerable<JustificativaAlunoDto> MapearFrequenciasParaDto(IEnumerable<JustificativaAlunoDto> frequencias)
        {
            var listaFrequencias = new List<JustificativaAlunoDto>();

            foreach (var frequencia in frequencias)
            {
                listaFrequencias.Add(new JustificativaAlunoDto()
                {
                    Id = frequencia.Id,
                    DataAusencia = frequencia.DataAusencia,
                    Motivo = UtilRegex.RemoverTagsHtml(frequencia.Motivo),
                    RegistradoPor = $"{frequencia.RegistradoPor} ({frequencia.RegistradoRF})"
                });
            }

            return listaFrequencias;
        }
    }
}
