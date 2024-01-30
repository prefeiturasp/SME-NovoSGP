using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirInformesPerfilsPorIdInformesCommad : IRequest<bool>
    {
        public ExcluirInformesPerfilsPorIdInformesCommad(long informesId)
        {
            InformesId = informesId;
        }
        public long InformesId { get; }
    }

    public class ExcluirInformesPerfilsCommandValidator : AbstractValidator<ExcluirInformesPerfilsPorIdInformesCommad>
    {
        public ExcluirInformesPerfilsCommandValidator()
        {
            RuleFor(c => c.InformesId)
                .GreaterThan(0)
                .WithMessage("O Id do infomes deve ser informado para exclusão do perfil.");
        }
    }
}
