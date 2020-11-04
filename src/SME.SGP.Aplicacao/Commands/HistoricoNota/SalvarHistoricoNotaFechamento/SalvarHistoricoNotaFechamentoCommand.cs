using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaFechamentoCommand : IRequest<bool>
    {
        public SalvarHistoricoNotaFechamentoCommand(long historicoNotaId, long fechamentoNotaId)
        {
            HistoricoNotaId = historicoNotaId;
            FechamentoNotaId = fechamentoNotaId;
        }

        public long HistoricoNotaId { get; set; }
        public long FechamentoNotaId { get; set; }
    }

    public class SalvarHistoricoNotaFechamentoCommandValidator : AbstractValidator<SalvarHistoricoNotaFechamentoCommand>
    {
        public SalvarHistoricoNotaFechamentoCommandValidator()
        {
            RuleFor(a => a.HistoricoNotaId)
                   .NotEmpty()
                   .WithMessage("O id do Historico de Nota deve ser informada!");
            RuleFor(a => a.FechamentoNotaId)
                  .NotEmpty()
                  .WithMessage("O id da nota do fechamento deve ser informada!");
        }
    }
}
