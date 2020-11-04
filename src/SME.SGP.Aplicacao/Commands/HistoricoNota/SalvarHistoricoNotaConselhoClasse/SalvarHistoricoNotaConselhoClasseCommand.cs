using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaConselhoClasseCommand : IRequest<bool>
    {
        public SalvarHistoricoNotaConselhoClasseCommand(long historicoNotaId, long conselhoClasseNotaId)
        {
            HistoricoNotaId = historicoNotaId;
            ConselhoClasseNotaId = conselhoClasseNotaId;
        }

        public long HistoricoNotaId { get; set; }
        public long ConselhoClasseNotaId { get; set; }
    }

    public class SalvarHistoricoNotaConselhoClasseCommandValidator : AbstractValidator<SalvarHistoricoNotaConselhoClasseCommand>
    {
        public SalvarHistoricoNotaConselhoClasseCommandValidator()
        {
            RuleFor(a => a.HistoricoNotaId)
                   .NotEmpty()
                   .WithMessage("O id do Historico de Nota deve ser informada!");
            RuleFor(a => a.ConselhoClasseNotaId)
                  .NotEmpty()
                  .WithMessage("O id da nota do consecho de classe deve ser informada!");
        }
    }
}
