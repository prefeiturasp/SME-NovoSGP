using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class EnviarParaAnaliseEncaminhamentoAEECommand : IRequest<bool>
    {
        public EnviarParaAnaliseEncaminhamentoAEECommand(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; set; }
    }

    public class EnviarParaAnaliseEncaminhamentoAEECommandValidator : AbstractValidator<EnviarParaAnaliseEncaminhamentoAEECommand>
    {
        public EnviarParaAnaliseEncaminhamentoAEECommandValidator()
        {
            RuleFor(x => x.EncaminhamentoId)
                   .GreaterThan(0)
                    .WithMessage("O Id do Encaminhamento deve ser informado!");
        }
    }
}
