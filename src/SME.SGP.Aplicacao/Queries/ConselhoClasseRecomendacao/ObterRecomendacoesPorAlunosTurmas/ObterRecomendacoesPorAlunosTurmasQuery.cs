using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesPorAlunosTurmasQuery : IRequest<IEnumerable<RecomendacaoConselhoClasseAlunoDTO>>
    {
        public ObterRecomendacoesPorAlunosTurmasQuery(string codigoAluno, string codigoTurma, int anoLetivo, int? modalidade, int semestre)
        {
            CodigoAluno = codigoAluno;
            CodigoTurma = codigoTurma;
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            Semestre = semestre;
        }

        public string CodigoAluno { get; set; }
        public string CodigoTurma { get; set; }
        public int AnoLetivo { get; set; }
        public int? Modalidade { get; set; }
        public int Semestre { get; set; }
    }
}
