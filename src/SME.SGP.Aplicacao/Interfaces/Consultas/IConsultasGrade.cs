using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasGrade
    {
        Task<GradeComponenteTurmaAulasDto> ObterGradeAulasTurmaProfessor(string turmaCodigo, long disciplina, int semana, DateTime dataAula, string codigoRf = null, bool ehRegencia = false);

        Task<GradeDto> ObterGradeTurma(TipoEscola tipoEscola, Modalidade modalidade, int duracao);

        Task<int> ObterHorasGradeComponente(long grade, long componenteCurricular, int ano);
    }
}