using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWFAprovacaoNotaFechamentoPorNotaCommand : IRequest
    {
        public ExcluirWFAprovacaoNotaFechamentoPorNotaCommand(long fechamentoNotaId)
        {
            FechamentoNotaId = fechamentoNotaId;
        }

        public long FechamentoNotaId { get; }
    }

    public class ExcluirWFAprovacaoNotaFechamentoPorNotaCommandValidator : AbstractValidator<ExcluirWFAprovacaoNotaFechamentoPorNotaCommand>
    {
        public ExcluirWFAprovacaoNotaFechamentoPorNotaCommandValidator()
        {
            RuleFor(a => a.FechamentoNotaId)
                .NotEmpty()
                .WithMessage("O identificador da nota de fechamento deve ser informado para exclusão do workflow de aprovação");
        }
    }
}
