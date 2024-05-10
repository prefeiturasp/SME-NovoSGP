using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroColetivoAnexoCommand : IRequest<bool>
    {
        public InserirRegistroColetivoAnexoCommand(long registroColetivoId, IEnumerable<AnexoDto> anexos)
        {
            RegistroColetivoId = registroColetivoId;
            Anexos = anexos;
        }

        public long RegistroColetivoId { get; set; }

        public IEnumerable<AnexoDto> Anexos { get; set; }
    }

    public class InserirRegistroColetivoAnexoCommandValidator : AbstractValidator<InserirRegistroColetivoAnexoCommand>
    {
        public InserirRegistroColetivoAnexoCommandValidator()
        {
            RuleFor(a => a.RegistroColetivoId)
                .NotEmpty()
                .WithMessage("O Id do registro coletivo deve ser informado!");

            RuleFor(a => a.Anexos)
                .NotEmpty()
                .Must(a => a.Any())
                .WithMessage("Deve ser informados os anexos!");
        }
    }
}
