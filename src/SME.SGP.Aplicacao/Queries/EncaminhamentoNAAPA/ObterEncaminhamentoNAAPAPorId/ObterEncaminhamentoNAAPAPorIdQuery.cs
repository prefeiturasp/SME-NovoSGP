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
    public class ObterEncaminhamentoNAAPAPorIdQuery : IRequest<EncaminhamentoNAAPA>
    {
        public ObterEncaminhamentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id do encaminhamento naapa deve ser informado para a pesquisa");

        }
    }
}
