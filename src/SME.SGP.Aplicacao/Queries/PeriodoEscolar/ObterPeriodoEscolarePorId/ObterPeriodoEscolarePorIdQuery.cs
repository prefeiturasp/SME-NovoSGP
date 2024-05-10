using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarePorIdQuery : IRequest<PeriodoEscolar>
    {
        public ObterPeriodoEscolarePorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }


    public class ObterPeriodoEscolarePorIdQueryValidator : AbstractValidator<ObterPeriodoEscolarePorIdQuery>
    {
        public ObterPeriodoEscolarePorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O id do período escolar deve ser informado.");
        }
    }
}
