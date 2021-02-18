using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterVersoesPlanoAEEQuery : IRequest<IEnumerable<PlanoAEEVersaoDto>>
    {
        public ObterVersoesPlanoAEEQuery(long planoId, long? versaoPlanoId)
        {
            PlanoId = planoId;
            VersaoPlanoId = versaoPlanoId;
        }

        public long PlanoId { get; }
        public long? VersaoPlanoId { get; }
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
