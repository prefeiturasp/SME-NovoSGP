using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaFechamentoCommand : IRequest<bool>
    {
        public SalvarPendenciaFechamentoCommand(long fechamentoTurmaDisciplinaId, long pendenciaId)
        {
            FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
            PendenciaId = pendenciaId;
        }

        public long FechamentoTurmaDisciplinaId { get; set; }
        public long PendenciaId { get; set; }
    }

    public class SalvarPendenciaFechamentoCommandValidator : AbstractValidator<SalvarPendenciaFechamentoCommand>
    {
        public SalvarPendenciaFechamentoCommandValidator()
        {
            RuleFor(c => c.FechamentoTurmaDisciplinaId)
               .NotEmpty()
               .WithMessage("O id do fechamento da turma deve ser informado.");
            RuleFor(c => c.PendenciaId)
              .NotEmpty()
              .WithMessage("O id da pendencia deve ser informado.");
        }
    }
}
