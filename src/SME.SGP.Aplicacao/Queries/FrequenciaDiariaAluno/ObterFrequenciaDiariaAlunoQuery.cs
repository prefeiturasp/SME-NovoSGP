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
                .WithMessage("O Código do aluno deve ser informado.");
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da Turma deve ser informado.");
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O id da Componente deve ser informado.");
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O Bimestre deve ser informado.");
        }
    }
}
