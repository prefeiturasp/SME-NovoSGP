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
            RuleFor(c => c.NotaAnterior)
            .NotEmpty()
            .WithMessage("A nota anteior deve ser informada para geração do histórico");

            RuleFor(c => c.NotaNova)
            .NotEmpty()
            .WithMessage("A nota nova deve ser informada para geração do histórico");
        }
    }
}
