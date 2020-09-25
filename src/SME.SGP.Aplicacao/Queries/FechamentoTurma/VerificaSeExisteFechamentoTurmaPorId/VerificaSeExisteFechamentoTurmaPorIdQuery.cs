using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExisteFechamentoTurmaPorIdQuery : IRequest<bool>
    {
        public VerificaSeExisteFechamentoTurmaPorIdQuery(long fechamentoTurmaId)
        {
            FechamentoTurmaId = fechamentoTurmaId;
        }

        public long FechamentoTurmaId { get; set; }
    }

    public class VerificaSeExisteFechamentoTurmaPorIdQueryValidator : AbstractValidator<VerificaSeExisteFechamentoTurmaPorIdQuery>
    {
        public VerificaSeExisteFechamentoTurmaPorIdQueryValidator()
        {
            RuleFor(a => a.FechamentoTurmaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id do conselho de classe");
        }
    }
}
