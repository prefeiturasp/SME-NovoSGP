using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesPerfilsCommand : IRequest<long>
    {
        public SalvarInformesPerfilsCommand(long informesId, long perfilId)
        {
            InformesId = informesId;
            PerfilId = perfilId;
        }

        public long InformesId { get; set; }
        public long PerfilId { get; set; }
    }

    public class SalvarInformesPerfilsCommandValidator : AbstractValidator<SalvarInformesPerfilsCommand>
    {
        public SalvarInformesPerfilsCommandValidator()
        {
            RuleFor(a => a.InformesId)
               .NotEmpty()
               .WithMessage("O id informes deve ser informado para o cadastro do perfil informes.");

            RuleFor(a => a.PerfilId)
               .NotEmpty()
               .WithMessage("O id do perfil deve ser informado para o cadastro do perfil informes.");
        }
    }
}
