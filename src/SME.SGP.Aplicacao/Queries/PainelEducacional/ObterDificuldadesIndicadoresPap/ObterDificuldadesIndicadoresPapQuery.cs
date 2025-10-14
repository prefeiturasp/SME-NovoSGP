using MediatR;
using SME.SGP.Infra.Dtos.Aluno;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDificuldadesIndicadoresPap
{
    public class ObterDificuldadesIndicadoresPapQuery : IRequest<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>>
    {
        public IEnumerable<DadosMatriculaAlunoTipoPapDto> DadosMatriculaAluno { get; set; }

        public ObterDificuldadesIndicadoresPapQuery(IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosMatriculaAlunos)
        {
            DadosMatriculaAluno = dadosMatriculaAlunos;
        }
    }
}