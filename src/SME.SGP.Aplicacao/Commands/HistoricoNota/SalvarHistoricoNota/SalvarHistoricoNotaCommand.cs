using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaCommand : IRequest<long>
    {
        public SalvarHistoricoNotaCommand(string notaAnterior, string notaNova)
        {
            NotaAnterior = notaAnterior;
            NotaNova = notaNova;
        }

        public string NotaAnterior { get; set; }
        public string NotaNova { get; set; }
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
