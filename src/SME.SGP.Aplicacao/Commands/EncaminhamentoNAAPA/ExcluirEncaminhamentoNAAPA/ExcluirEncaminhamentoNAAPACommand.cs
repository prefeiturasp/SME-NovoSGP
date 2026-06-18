using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public ExcluirEncaminhamentoNAAPACommand(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; }
    }

    public class ExcluirEncaminhamentoNAAPACommandValidator : AbstractValidator<ExcluirEncaminhamentoNAAPACommand>
    {
        public ExcluirEncaminhamentoNAAPACommandValidator()
        {

            RuleFor(c => c.EncaminhamentoNAAPAId)
            .NotEmpty()
            .WithMessage("O id do encaminhamento NAAPA deve ser informado para exclusão.");

        }
    }
}
