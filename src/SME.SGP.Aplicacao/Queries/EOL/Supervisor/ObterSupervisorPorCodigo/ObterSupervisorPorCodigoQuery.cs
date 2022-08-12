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
    public class ObterSupervisorPorCodigoQuery : IRequest<IEnumerable<SupervisoresRetornoDto>>
    {
        public ObterSupervisorPorCodigoQuery(string[] supervisorIds)
        {
            SupervisorIds = supervisorIds;
        }
        public string[] SupervisorIds { get; set; }
    }

    public class ObterSupervisorPorCodigoQueryValidator : AbstractValidator<ObterSupervisorPorCodigoQuery>
    {
        public ObterSupervisorPorCodigoQueryValidator()
        {
            RuleFor(x => x.SupervisorIds)
                .NotEmpty().WithMessage("O código do supervisor precisa ser informado para obter um supervisor por código.");
        }
    }
}
