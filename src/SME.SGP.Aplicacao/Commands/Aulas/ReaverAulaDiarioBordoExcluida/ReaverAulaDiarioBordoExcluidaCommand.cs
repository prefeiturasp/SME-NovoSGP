using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao.Commands.Aulas.ReaverAulaDiarioBordoExcluida
{
    public class ReaverAulaDiarioBordoExcluidaCommand : IRequest<long>
    {
        public ReaverAulaDiarioBordoExcluidaCommand(long aulaId, long diarioBordoId)
        {
            AulaId = aulaId;
            DiarioBordoId = diarioBordoId;
        }

        public long AulaId { get; set; }
        public long DiarioBordoId { get; set; }
    }

    public class ReaverAulaDiarioBordoExcluidaCommandValidator : AbstractValidator<ReaverAulaDiarioBordoExcluidaCommand>
    {
        public ReaverAulaDiarioBordoExcluidaCommandValidator()
        {
            RuleFor(x => x.AulaId)
                .NotEmpty()
                .WithMessage("O id da aula deve ser informada.");

            RuleFor(x => x.DiarioBordoId)
                .NotEmpty()
                .WithMessage("O id dodiário de bordo deve ser informado.");
        }
    }
}
