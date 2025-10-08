using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapSgpConsolidadoQuery : IRequest<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>>
    {
        public IEnumerable<DadosMatriculaAlunoTipoPapDto> DadosMatriculaAluno { get; set; }

        public ObterIndicadoresPapSgpConsolidadoQuery(IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosMatriculaAlunos)
        {
            DadosMatriculaAluno = dadosMatriculaAlunos;
        }
    }
}