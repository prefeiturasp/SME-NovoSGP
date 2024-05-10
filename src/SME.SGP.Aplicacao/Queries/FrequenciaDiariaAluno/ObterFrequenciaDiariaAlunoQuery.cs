using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaDiariaAlunoQuery : IRequest<PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>>
    {
        public ObterFrequenciaDiariaAlunoQuery(long turmaId, long componenteCurricularId, long alunoCodigo, int bimestre, int? semestre = 0)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            Semestre = semestre;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public int? Semestre { get; set; }
    }

    public class ObterFrequenciaDiariaAlunoQueryValidator : AbstractValidator<ObterFrequenciaDiariaAlunoQuery>
    {
        public ObterFrequenciaDiariaAlunoQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .NotEqual(0)
                .WithMessage("O Código do aluno deve ser informado.");
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .NotEqual(0)
                .WithMessage("O id da Turma deve ser informado.");
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .NotEqual(0)
                .WithMessage("O id da Componente deve ser informado.");
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .When(a=> a.Semestre == 0 && a.Bimestre == 0)
                .WithMessage("O Bimestre deve ser informado.");
        }
    }
}
