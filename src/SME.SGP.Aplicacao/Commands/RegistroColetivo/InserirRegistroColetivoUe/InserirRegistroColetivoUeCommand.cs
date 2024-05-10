using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroColetivoUeCommand : IRequest<bool>
    {
        public InserirRegistroColetivoUeCommand(long registroColetivoId, IEnumerable<long> ueIds)
        {
            RegistroColetivoId = registroColetivoId;
            UeIds = ueIds;
        }

        public long RegistroColetivoId { get; set; }

        public IEnumerable<long> UeIds { get; set; }
    }

    public class InserirRegistroColetivoUeCommandValidator : AbstractValidator<InserirRegistroColetivoUeCommand>
    {
        public InserirRegistroColetivoUeCommandValidator()
        {
            RuleFor(a => a.RegistroColetivoId)
                .NotEmpty()
                .WithMessage("O Id do registro coletivo deve ser informado!");

            RuleFor(a => a.UeIds)
                .NotEmpty()
                .Must(a => a.Any())
                .WithMessage("Deve ser informados os ids das ues!");
        }
    }
}