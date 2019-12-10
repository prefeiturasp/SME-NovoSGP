using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace SME.SGP.Aplicacao
{
    public class ConsultasGrade : IConsultasGrade
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IConsultasAula consultasAula;
        private readonly IRepositorioGrade repositorioGrade;
        private readonly IServicoEOL servicoEOL;

        public ConsultasGrade(IRepositorioGrade repositorioGrade, IConsultasAbrangencia consultasAbrangencia,
                              IConsultasAula consultasAula, IServicoEOL servicoEOL)
        {
            this.repositorioGrade = repositorioGrade ?? throw new System.ArgumentNullException(nameof(repositorioGrade));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new System.ArgumentNullException(nameof(consultasAbrangencia));
            this.consultasAula = consultasAula ?? throw new System.ArgumentNullException(nameof(consultasAula));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<GradeComponenteTurmaAulasDto> ObterGradeAulasTurmaProfessor(string turma, int disciplina, string semana, DateTime dataAula, string codigoRf = null)
        {
            // Busca abrangencia a partir da turma
            var abrangencia = await consultasAbrangencia.ObterAbrangenciaTurma(turma);
            if (abrangencia == null)
                throw new NegocioException("Abrangência da turma não localizada.");

            // Busca grade a partir dos dados da abrangencia da turma
            var grade = await ObterGradeTurma(abrangencia.TipoEscola, abrangencia.Modalidade, abrangencia.QtDuracaoAula);
            if (grade == null)
                throw new NegocioException("Grade da turma não localizada.");

            // Busca disciplina no EOL para validar se é regente
            var disciplinaEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplina });
            if (disciplinaEOL == null)
                throw new NegocioException("Disciplina não localizada.");

            bool ehRegencia = disciplinaEOL.FirstOrDefault().Regencia;

            int horasGrade;

            // verifica se é regencia de classe
            if (ehRegencia)
                horasGrade = abrangencia.Modalidade == Modalidade.EJA ? 5 : 1;
            else if (disciplina == 1030)
                horasGrade = 4;
            else
                // Busca carga horaria na grade da disciplina para o ano da turma
                horasGrade = await ObterHorasGradeComponente(grade.Id, disciplina, abrangencia.Ano);

            if (horasGrade == 0)
                return null;

            int horascadastradas;

            if (ehRegencia)
                horascadastradas = await consultasAula.ObterQuantidadeAulasTurmaDiaProfessor(turma.ToString(), disciplina.ToString(), dataAula, codigoRf);
            else
                // Busca horas aula cadastradas para a disciplina na turma
                horascadastradas = await consultasAula.ObterQuantidadeAulasTurmaSemanaProfessor(turma.ToString(), disciplina.ToString(), semana, codigoRf);

            return new GradeComponenteTurmaAulasDto
            {
                QuantidadeAulasGrade = horasGrade,
                QuantidadeAulasRestante = horasGrade - horascadastradas
            };
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