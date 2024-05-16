using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnexosInformativoPorIdInformativoCommad : IRequest<bool>
    {
        public ExcluirAnexosInformativoPorIdInformativoCommad(long informativoId)
        {
            InformativoId = informativoId;
        }
        public long InformativoId { get; }
    }

    public class ExcluirAnexosInformativoPorIdInformativoCommadValidator : AbstractValidator<ExcluirAnexosInformativoPorIdInformativoCommad>
    {
        public ExcluirAnexosInformativoPorIdInformativoCommadValidator()
        {
            RuleFor(c => c.InformativoId)
                .GreaterThan(0)
                .WithMessage("O Id do informativo deve ser informado para exclusão dos anexos.");
        }
    }
}
