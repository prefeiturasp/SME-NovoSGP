using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorCargoQuery : IRequest<IEnumerable<FuncionarioDto>>
    {
        public ObterFuncionariosPorCargoQuery(Cargo cargo)
        {
            Cargo = cargo;
        }

        public Cargo Cargo { get; set; }
    }

    public class ObterFuncionariosPorCargoQueryValidator : AbstractValidator<ObterFuncionariosPorCargoQuery>
    {
        public ObterFuncionariosPorCargoQueryValidator()
        {
            RuleFor(c => c.Cargo)
               .NotEmpty()
               .WithMessage("O cargo deve ser informado.");
        }
    }
}
