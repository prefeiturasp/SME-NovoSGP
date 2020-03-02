using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasGrade : IConsultasGrade
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IConsultasAula consultasAula;
        private readonly IRepositorioGrade repositorioGrade;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IServicoEOL servicoEOL;

        public ConsultasGrade(IRepositorioGrade repositorioGrade, IConsultasAbrangencia consultasAbrangencia,
                              IConsultasAula consultasAula, IServicoEOL servicoEOL, IRepositorioUe repositorioUe, IRepositorioTurma repositorioTurma)
        {
            this.repositorioGrade = repositorioGrade ?? throw new System.ArgumentNullException(nameof(repositorioGrade));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new System.ArgumentNullException(nameof(consultasAbrangencia));
            this.consultasAula = consultasAula ?? throw new System.ArgumentNullException(nameof(consultasAula));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<GradeComponenteTurmaAulasDto> ObterGradeAulasTurmaProfessor(string turmaCodigo, long disciplina, int semana, DateTime dataAula, string codigoRf = null, bool ehRegencia = false)
        {
            var ue = repositorioUe.ObterUEPorTurma(turmaCodigo);
            if (ue == null)
                throw new NegocioException("Ue não localizada.");

            var turma = repositorioTurma.ObterPorId(turmaCodigo);
            if (turma == null)
                throw new NegocioException("Turma não localizada.");

            // Busca grade a partir dos dados da abrangencia da turma
            var grade = await ObterGradeTurma(ue.TipoEscola, turma.ModalidadeCodigo, turma.QuantidadeDuracaoAula);
            if (grade == null)
                return null;

            // verifica se é regencia de classe
            var horasGrade = await TratarHorasGrade(disciplina, turma, grade, ehRegencia);
            if (horasGrade == 0)
                return null;

            var horascadastradas = await ObtenhaHorasCadastradas(disciplina, semana, dataAula, codigoRf, turma, ehRegencia);

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

        public async Task<int> ObterHorasGradeComponente(long grade, long componenteCurricular, int ano)
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

        private async Task<int> ObtenhaHorasCadastradas(long disciplina, int semana, DateTime dataAula, string codigoRf, Turma turma, bool ehRegencia)
        {
            if (ehRegencia)
                return await consultasAula.ObterQuantidadeAulasTurmaDiaProfessor(turma.CodigoTurma, disciplina.ToString(), dataAula, codigoRf);

            // Busca horas aula cadastradas para a disciplina na turma
            return await consultasAula.ObterQuantidadeAulasTurmaSemanaProfessor(turma.CodigoTurma, disciplina.ToString(), semana, codigoRf);
        }

        private async Task<int> TratarHorasGrade(long disciplina, Turma turma, GradeDto grade, bool ehRegencia)
        {
            if (ehRegencia)
                return turma.ModalidadeCodigo == Modalidade.EJA ? 5 : 1;

            if (disciplina == 1030)
                return 4;

            return await ObterHorasGradeComponente(grade.Id, disciplina, int.Parse(turma.Ano));

        }
    }
}