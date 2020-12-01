using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorCargoUeQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosPorCargoUeQuery(Cargo cargo, string codigoUe)
        {
            Cargo = cargo;
            CodigoUe = codigoUe;
        }

        public Cargo Cargo { get; set; }
        public string CodigoUe { get; set; }
    }

    public class ObterFuncionariosPorCargoUeQueryValidator : AbstractValidator<ObterFuncionariosPorCargoUeQuery>
    {
        public ObterFuncionariosPorCargoUeQueryValidator()
        {
            RuleFor(c => c.Cargo)
               .NotEmpty()
               .WithMessage("O cargo deve ser informado.");
            RuleFor(c => c.CodigoUe)
               .NotEmpty()
               .WithMessage("O código da ue deve ser informado.");
        }
    }
}
