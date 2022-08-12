using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorDreQuery : IRequest<IEnumerable<SupervisorEscolasDreDto>>
    {
        public ObterSupervisoresPorDreQuery(string codigoDre)
        {
            CodigoDre = codigoDre;
        }

        public string CodigoDre { get; set; }
    }
    public class ObterSupervisoresPorDreQueryValidator : AbstractValidator<ObterSupervisoresPorDreQuery>
    {
        public ObterSupervisoresPorDreQueryValidator()
        {
            RuleFor(a => a.CodigoDre)
                .NotEmpty()
                .WithMessage("O código da Dre deve ser informado.");
        }
    }
}
