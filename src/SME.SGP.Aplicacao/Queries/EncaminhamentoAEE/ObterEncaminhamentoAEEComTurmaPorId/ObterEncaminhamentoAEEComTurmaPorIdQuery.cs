using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEEComTurmaPorIdQuery : IRequest<EncaminhamentoAEE>
    {
        public ObterEncaminhamentoAEEComTurmaPorIdQuery(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; }
    }

    public class ObterEncaminhamentoAEEComTurmaPorIdQueryValidator : AbstractValidator<ObterEncaminhamentoAEEComTurmaPorIdQuery>
    {
        public ObterEncaminhamentoAEEComTurmaPorIdQueryValidator()
        {
            RuleFor(a => a.EncaminhamentoId)
                .NotEmpty()
                .WithMessage("O id do encaminhamento é necessário para pesquisa.");
        }
    }
}
