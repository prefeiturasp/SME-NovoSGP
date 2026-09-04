using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommand : IRequest<bool>
    {
        public ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommand(long[] ids)
        {
            Ids = ids;
        }

        public long[] Ids { get; set; }
    }

    public class ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommandValidator : AbstractValidator<ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommand>
    {
        public ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommandValidator()
        {
            RuleFor(c => c.Ids)
            .NotEmpty()
            .WithMessage("Os ids das compensações de ausências alunos aulas devem ser informados para exclusão lógica.");
        }
    }
}
