using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterJuremaIdsPorComponentesCurricularIdQuery : IRequest<long[]>
    {
        public ObterJuremaIdsPorComponentesCurricularIdQuery()
        { 
        }
        public ObterJuremaIdsPorComponentesCurricularIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterJuremaIdsPorComponentesCurricularIdQueryValidator : AbstractValidator<ObterJuremaIdsPorComponentesCurricularIdQuery>
    {
        public ObterJuremaIdsPorComponentesCurricularIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O Id do componente curricular deve ser informado");
        }
    }

}
