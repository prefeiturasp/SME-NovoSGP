using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries.CompensacaoAusencia.ObterAusenciaParaCompensacaoPorAlunos
{
    public class ObterAusenciaParaCompensacaoPorAlunosQuery :IRequest<IEnumerable<CompensacaoDataAlunoDto>>
    {
        public ObterAusenciaParaCompensacaoPorAlunosQuery(string[] codigosAlunos, string disciplinaId, int bimestre, string turmacodigo)
        {
            CodigosAlunos = codigosAlunos;
            DisciplinaId = disciplinaId;
            Bimestre = bimestre;
            Turmacodigo = turmacodigo;
        }

        public string[] CodigosAlunos { get; set; }
        public string  DisciplinaId { get; set; }
        public int Bimestre { get; set; }
        public string Turmacodigo { get; set; }
    }
}