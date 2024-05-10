using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosIdPorCodigoUeECargoQuery : IRequest<IEnumerable<long>>
    {
        public ObterFuncionariosIdPorCodigoUeECargoQuery(string codigoUe, Cargo cargo)
        {
            CodigoUe = codigoUe;
            Cargo = cargo;
        }

        public string CodigoUe { get; set; }
        public Cargo Cargo { get; set; }
    }

    public class ObterFuncionariosIdPorCodigoUeECargoQueryValidator : AbstractValidator<ObterFuncionariosIdPorCodigoUeECargoQuery>
    {
        public ObterFuncionariosIdPorCodigoUeECargoQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado.");

            RuleFor(c => c.Cargo)
               .NotEmpty()
               .WithMessage("O cargo deve ser informado.");
        }
    }
}
