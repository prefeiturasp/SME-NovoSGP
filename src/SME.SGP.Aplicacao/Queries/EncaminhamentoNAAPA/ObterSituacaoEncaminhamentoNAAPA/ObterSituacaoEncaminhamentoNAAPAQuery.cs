using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSituacaoEncaminhamentoNAAPAQuery : IRequest<SituacaoDto>
    {
        public ObterSituacaoEncaminhamentoNAAPAQuery(long id)
        {
            this.Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterSituacaoEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterSituacaoEncaminhamentoNAAPAQuery>
    {
        public ObterSituacaoEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O id do encaminhamento NAAPA deve ser informada para obter a situação do encaminhamento NAAPA.");
        }
    }
}
