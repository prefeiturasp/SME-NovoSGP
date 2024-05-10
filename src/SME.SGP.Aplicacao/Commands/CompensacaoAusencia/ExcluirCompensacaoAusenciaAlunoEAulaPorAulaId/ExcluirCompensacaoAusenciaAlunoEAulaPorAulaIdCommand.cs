using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand : IRequest<bool>
    {
        public ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand(long aulaId, long? numeroAula = null)
        {
            AulaId = aulaId;
            NumeroAula = numeroAula;
        }

        public long AulaId { get; set; }

        public long? NumeroAula { get; set; }
    }
    
    public class ExcluirCompensacaoAusenciaPorAulaIdCommandValidator : AbstractValidator<ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand>
    {
        public ExcluirCompensacaoAusenciaPorAulaIdCommandValidator()
        {
            RuleFor(c => c.AulaId)
                .NotNull()
                .WithMessage("O id da aula deve ser informado para efetuar a exclusão de compensações.");
        }
    }
}
