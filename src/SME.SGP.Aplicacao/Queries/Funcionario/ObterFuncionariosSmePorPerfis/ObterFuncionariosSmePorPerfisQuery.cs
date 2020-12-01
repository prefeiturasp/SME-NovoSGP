using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosSmePorPerfisQuery : IRequest<IEnumerable<string>>
    {
        public ObterFuncionariosSmePorPerfisQuery(IEnumerable<Guid> perfis)
        {
            Perfis = perfis;
        }

        public IEnumerable<Guid> Perfis { get; set; }
    }

    public class ObterFuncionariosSmePorPerfisQueryValidator : AbstractValidator<ObterFuncionariosSmePorPerfisQuery>
    {
        public ObterFuncionariosSmePorPerfisQueryValidator()
        {
            RuleFor(c => c.Perfis)
               .NotEmpty()
               .WithMessage("Os perfis devem ser informados para consulta de funcionários.");

        }
    }
}
