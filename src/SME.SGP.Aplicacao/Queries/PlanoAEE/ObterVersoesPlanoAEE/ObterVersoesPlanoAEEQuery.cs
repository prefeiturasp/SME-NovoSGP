using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterVersoesPlanoAEEQuery : IRequest<IEnumerable<PlanoAEEVersaoDto>>
    {
        public ObterVersoesPlanoAEEQuery(long planoId)
        {
            PlanoId = planoId;
        }

        public long PlanoId { get; }
    }

    public class ObterVersoesPlanoAEEQueryValidator : AbstractValidator<ObterVersoesPlanoAEEQuery>
    {
        public ObterVersoesPlanoAEEQueryValidator()
        {
            RuleFor(a => a.PlanoId)
                .NotEmpty()
                .WithMessage("O id do plano deve ser informado para consulta de suas versões.");
        }
    }
}
