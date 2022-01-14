using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQuery : IRequest<FrequenciaAluno>
    {
        public string CodigoAluno { get; set; }
        public string DisciplinaId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public TipoFrequenciaAluno Tipo { get; set; }
        public string TurmaId { get; set; }

        public ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQuery(string codigoAluno, string disciplinaId, long periodoEscolarId, TipoFrequenciaAluno tipo, string turmaId)
        {
            CodigoAluno = codigoAluno;
            DisciplinaId = disciplinaId;
            PeriodoEscolarId = periodoEscolarId;
            Tipo = tipo;
            TurmaId = turmaId;
        }
    }

    public class ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQueryValidator : AbstractValidator<ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQuery>
    {
        public ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQueryValidator()
        {
            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O codigo do aluno deve ser informado.");

            RuleFor(x => x.DisciplinaId)
                .NotEmpty()
                .WithMessage("A disciplina deve ser informada.");

            RuleFor(x => x.PeriodoEscolarId)
                .NotEmpty()
                .WithMessage("O período escolar deve ser informado.");

            RuleFor(x => x.Tipo)
                .NotEmpty()
                .WithMessage("O tipo da frequencia deve ser informado.");

            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("a turma deve ser informado.");
        }
    }
} 