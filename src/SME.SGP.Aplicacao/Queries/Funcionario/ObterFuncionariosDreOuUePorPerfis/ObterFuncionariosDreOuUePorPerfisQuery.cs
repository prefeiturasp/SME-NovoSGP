using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosDreOuUePorPerfisQuery : IRequest<IEnumerable<string>>
    {

        public ObterFuncionariosDreOuUePorPerfisQuery(string codigoDreUe, IEnumerable<Guid> perfis)
        {
            CodigoDreUe = codigoDreUe;
            Perfis = perfis;
        }

        public string CodigoDreUe { get; set; }
        public IEnumerable<Guid> Perfis { get; set; }
    }

    public class ObterFuncionariosDreOuUePorPerfisQueryValidator : AbstractValidator<ObterFuncionariosDreOuUePorPerfisQuery>
    {
        public ObterFuncionariosDreOuUePorPerfisQueryValidator()
        {
            RuleFor(c => c.CodigoDreUe)
               .NotEmpty()
               .WithMessage("O código da Dre ou Ue deve ser informado para consulta de funcionários.");

            RuleFor(c => c.Perfis)
               .Must(a => a.Any())
               .WithMessage("Os perfis devem ser informados para consulta de funcionários.");
        }
    }
}
