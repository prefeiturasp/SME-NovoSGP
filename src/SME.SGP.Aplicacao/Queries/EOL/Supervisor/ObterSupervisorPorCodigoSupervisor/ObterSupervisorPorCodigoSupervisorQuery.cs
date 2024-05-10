using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisorPorCodigoSupervisorQuery : IRequest<IEnumerable<SupervisoresRetornoDto>>
    {
        public ObterSupervisorPorCodigoSupervisorQuery(string[] codigoSupervisores)
        {
            CodigoSupervisores = codigoSupervisores;
        }
        public string[] CodigoSupervisores { get; set; }

        public string CodigoDre { get; set; }
    }

    public class ObterSupervisorPorCodigoSupervisorQueryValidator : AbstractValidator<ObterSupervisorPorCodigoSupervisorQuery>
    {
        public ObterSupervisorPorCodigoSupervisorQueryValidator()
        {
            RuleFor(x => x.CodigoSupervisores)
                .NotNull()
                .WithMessage("Os códigos dos supervisores devem ser informados para obter um supervisor.");
        }
    }
}
