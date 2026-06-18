using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

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
                .WithMessage("O id do período pap deve ser informado para período relatório pap.");
        }
    }
}
