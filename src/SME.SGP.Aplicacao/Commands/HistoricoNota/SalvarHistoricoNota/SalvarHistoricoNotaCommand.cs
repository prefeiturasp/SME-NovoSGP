using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaCommand : IRequest<long>
    {
        public SalvarHistoricoNotaCommand(double notaAnterior, double notaNova)
        {
            NotaAnterior = notaAnterior;
            NotaNova = notaNova;
        }

        public double NotaAnterior { get; set; }
        public double NotaNova { get; set; }
    }

    public class SalvarHistoricoNotaCommandValidator : AbstractValidator<SalvarHistoricoNotaCommand>
    {
        public SalvarHistoricoNotaCommandValidator()
        {
            RuleFor(a => a.NotaAnterior)
                   .NotEmpty()
                   .WithMessage("A nota anterior deve ser informada!");
            RuleFor(a => a.NotaNova)
                  .NotEmpty()
                  .WithMessage("A nota nova deve ser informada!");
        }
    }
}
