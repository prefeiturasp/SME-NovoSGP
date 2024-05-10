using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaConselhoClasseCommand : IRequest<long>
    {
        public SalvarHistoricoNotaConselhoClasseCommand(long conselhoClasseNotaId, double notaAnterior, double notaNova)
        {
            ConselhoClasseNotaId = conselhoClasseNotaId;
            NotaAnteior = notaAnterior;
            NotaNova = notaNova;
        }

        public long ConselhoClasseNotaId { get; set; }
        public double NotaAnteior { get; set; }
        public double NotaNova { get; set; }
    }

    public class SalvarHistoricoNotaConselhoClasseCommandValidator : AbstractValidator<SalvarHistoricoNotaConselhoClasseCommand>
    {
        public SalvarHistoricoNotaConselhoClasseCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseNotaId)
                  .NotEmpty()
                  .WithMessage("O id da nota do conselho de classe deve ser informada para geração do histórico!");

            RuleFor(a => a.NotaAnteior)
                  .NotNull()
                  .WithMessage("A nota anterior do conselho de classe deve ser informada para geração do histórico!");

            RuleFor(a => a.NotaNova)
                  .NotNull()
                  .WithMessage("A nnova nota do conselho de classe deve ser informada para geração do histórico!");
        }
    }
}
