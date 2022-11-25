using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoNAAPAComTurmaPorIdQuery : IRequest<EncaminhamentoNAAPA>
    {
        public ObterEncaminhamentoNAAPAComTurmaPorIdQuery(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; }
    }

    public class ObterEncaminhamentoNAAPAComTurmaPorIdQueryValidator : AbstractValidator<ObterEncaminhamentoNAAPAComTurmaPorIdQuery>
    {
        public ObterEncaminhamentoNAAPAComTurmaPorIdQueryValidator()
        {
            RuleFor(a => a.EncaminhamentoId)
                .NotEmpty()
                .WithMessage("O id do encaminhamento NAAPA é necessário para pesquisa.");
        }
    }
}
