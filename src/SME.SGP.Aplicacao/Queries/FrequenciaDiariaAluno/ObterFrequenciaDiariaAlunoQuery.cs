using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaDiariaAlunoQuery : IRequest<PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>>
    {
        public ObterFrequenciaDiariaAlunoQuery(long turmaId, long componenteCurricularId, long alunoCodigo, int bimestre)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
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
                .NotEqual(0)
                .WithMessage("O Bimestre deve ser informado.");
        }
    }
}
