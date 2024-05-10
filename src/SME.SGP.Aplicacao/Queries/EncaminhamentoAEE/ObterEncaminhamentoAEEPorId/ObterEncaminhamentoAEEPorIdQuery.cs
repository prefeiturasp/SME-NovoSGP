using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEEPorIdQuery : IRequest<EncaminhamentoAEE>
    {
        public ObterEncaminhamentoAEEPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterEncaminhamentoAEEPorIdQueryValidator : AbstractValidator<ObterEncaminhamentoAEEPorIdQuery>
    {
        public ObterEncaminhamentoAEEPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id do encaminhamento deve ser informado");

        }
    }
}
