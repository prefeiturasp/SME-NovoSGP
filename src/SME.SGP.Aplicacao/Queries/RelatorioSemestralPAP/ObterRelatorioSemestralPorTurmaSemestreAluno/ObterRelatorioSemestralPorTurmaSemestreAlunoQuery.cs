using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioSemestralPorTurmaSemestreAlunoQuery : IRequest<RelatorioSemestralAlunoDto>
    {
        public ObterRelatorioSemestralPorTurmaSemestreAlunoQuery(string alunoCodigo, string turmaCodigo, int semestre)
        {
            if (String.IsNullOrEmpty(alunoCodigo))
                throw new NegocioException("É necessário informar o código do aluno");
            if (String.IsNullOrEmpty(turmaCodigo))
                throw new NegocioException("É necessário informar o código da turma");
            if(semestre == 0)
                throw new NegocioException("É necessário informar o semestre");

            AlunoCodigo = alunoCodigo;
            TurmaCodigo = turmaCodigo;
            Semestre = semestre;

        }
        public string AlunoCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public int Semestre { get; set; }
    }
}
