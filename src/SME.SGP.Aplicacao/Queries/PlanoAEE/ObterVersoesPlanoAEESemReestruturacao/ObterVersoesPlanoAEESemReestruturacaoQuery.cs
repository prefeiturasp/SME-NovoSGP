using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterVersoesPlanoAEESemReestruturacaoQuery : IRequest<IEnumerable<PlanoAEEVersaoDto>>
    {
        public ObterVersoesPlanoAEESemReestruturacaoQuery(long planoId, long reestruturacaoId)
        {
            PlanoId = planoId;
            ReestruturacaoId = reestruturacaoId;
        }

        public long PlanoId { get; }
        public long ReestruturacaoId { get; }
    }

    public class ObterVersoesPlanoAEESemReestruturacaoQueryValidator : AbstractValidator<ObterVersoesPlanoAEESemReestruturacaoQuery>
    {
        public ObterVersoesPlanoAEESemReestruturacaoQueryValidator()
        {
            RuleFor(a => a.PlanoId)
                .NotEmpty()
                .WithMessage("O id do plano deve ser informado para consulta de suas versões.");
        }
    }
}
