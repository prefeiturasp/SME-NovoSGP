using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorUeQuery : IRequest<IEnumerable<SupervisorEscolasDreDto>>
    {
        public ObterSupervisoresPorUeQuery(string codigoUe)
        {
            CodigoUe = codigoUe;
        }

        public string CodigoUe { get; set; }
    }
    public class ObterSupervisoresPorUeQueryValidator : AbstractValidator<ObterSupervisoresPorUeQuery>
    {
        public ObterSupervisoresPorUeQueryValidator()
        {
            RuleFor(a => a.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");
        }
    }
}
