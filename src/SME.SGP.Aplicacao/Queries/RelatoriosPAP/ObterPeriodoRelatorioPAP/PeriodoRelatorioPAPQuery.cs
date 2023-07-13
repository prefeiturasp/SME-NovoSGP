using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PeriodoRelatorioPAPQuery : IRequest<PeriodoRelatorioPAP>
    {
        public PeriodoRelatorioPAPQuery(long periodoIdPAP)
        {
            PeriodoIdPAP = periodoIdPAP;
        }

        public long PeriodoIdPAP { get; set; }
    }

    public class PeriodoRelatorioPAPQueryValidator : AbstractValidator<PeriodoRelatorioPAPQuery>
    {
        public PeriodoRelatorioPAPQueryValidator()
        {
            RuleFor(x => x.PeriodoIdPAP)
                .NotEmpty()
                .WithMessage("O id do período pap deve ser informado.");
        }
    }
}
