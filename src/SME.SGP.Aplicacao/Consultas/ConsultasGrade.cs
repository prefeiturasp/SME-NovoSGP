using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ConsultasGrade : IConsultasGrade
    {
        private readonly IRepositorioGrade repositorioGrade;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IConsultasAula consultasAula;

        public ConsultasGrade(IRepositorioGrade repositorioGrade, IConsultasAbrangencia consultasAbrangencia, IConsultasAula consultasAula)
        {
            this.repositorioGrade = repositorioGrade ?? throw new System.ArgumentNullException(nameof(repositorioGrade));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new System.ArgumentNullException(nameof(consultasAbrangencia));
            this.consultasAula = consultasAula ?? throw new System.ArgumentNullException(nameof(consultasAula));
        }

        public async Task<GradeComponenteTurmaAulasDto> ObterGradeAulasTurma(string turma, int disciplina)
        {
            // Busca abrangencia a partir da turma
            var abrangencia = await consultasAbrangencia.ObterAbrangenciaTurma(turma);
            if (abrangencia == null)
                throw new NegocioException("Abrangência da turma não localizada.");

            // Busca grade a partir dos dados da abrangencia da turma
            var grade = await ObterGradeTurma(abrangencia.TipoEscola, abrangencia.Modalidade, abrangencia.QtDuracaoAula);
            if (grade == null)
                throw new NegocioException("Grade da turma não localizada.");

            // Busca carga horaria na grade da disciplina para o ano da turma
            var horasGrade = await ObterHorasGradeComponente(grade.Id, disciplina, abrangencia.Ano);
            // Busca horas aula cadastradas para a disciplina na turma
            var horascadastradas = await consultasAula.ObterQuantidadeAulasTurma(turma.ToString(), disciplina.ToString());

            if (horasGrade > 0)
                return new GradeComponenteTurmaAulasDto
                {
                    QuantidadeAulasGrade = horasGrade,
                    QuantidadeAulasRestante = horasGrade - horascadastradas
                };
            else
                return null;
        }

        public async Task<GradeDto> ObterGradeTurma(TipoEscola tipoEscola, Modalidade modalidade, int duracao)
        {
            return MapearParaDto(await repositorioGrade.ObterGradeTurma(tipoEscola, modalidade, duracao));
        }

        public async Task<int> ObterHorasGradeComponente(long grade, int componenteCurricular, int ano)
        {
            return await repositorioGrade.ObterHorasComponente(grade, componenteCurricular, ano);
        }

        private GradeDto MapearParaDto(Grade grade)
        {
            return grade == null ? null : new GradeDto
            {
                Id = grade.Id,
                Nome = grade.Nome
            };
        }
    }
}
