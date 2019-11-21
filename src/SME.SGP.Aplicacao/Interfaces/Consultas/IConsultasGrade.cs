using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasGrade
    {
        Task<GradeDto> ObterGradeTurma(TipoEscola tipoEscola, Modalidade modalidade, int duracao);
        Task<int> ObterHorasGradeComponente(long grade, int componenteCurricular, int ano);
        Task<GradeComponenteTurmaAulasDto> ObterGradeAulasTurma(string turma, int disciplina, string semana);
    }
}
