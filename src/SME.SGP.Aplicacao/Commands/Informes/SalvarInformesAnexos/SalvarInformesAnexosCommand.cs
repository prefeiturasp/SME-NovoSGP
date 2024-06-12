using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesAnexosCommand : IRequest<bool>
    {
        public SalvarInformesAnexosCommand(long informativoId, Guid[] codigosAnexo)
        {
            InformativoId = informativoId;
            CodigosAnexo = codigosAnexo;
        }

        public long InformativoId { get; set; }
        public Guid[] CodigosAnexo { get; set; }
    }

    public class SalvarInformesAnexosCommandValidator : AbstractValidator<SalvarInformesAnexosCommand>
    {
        public SalvarInformesAnexosCommandValidator()
        {
            RuleFor(a => a.InformativoId)
               .NotEmpty()
               .WithMessage("O id do informativo deve ser informado para o cadastro do anexo.");

            RuleFor(a => a.CodigosAnexo)
               .NotEmpty()
               .WithMessage("Os códigos dos anexos devem ser informados para o cadastro.");
        }
    }
}
