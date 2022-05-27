using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaVersaoPlanoAEEQuery : IRequest<PlanoAEEVersaoDto>
    {
        public ObterUltimaVersaoPlanoAEEQuery(long planoId)
        {
            PlanoId = planoId;
        }

        public long PlanoId { get; }
    }
    public class ObterUltimaVersaoPlanoAEEQueryValidator : AbstractValidator<ObterUltimaVersaoPlanoAEEQuery>
    {
        public ObterUltimaVersaoPlanoAEEQueryValidator()
        {
            RuleFor(a => a.PlanoId)
                .NotNull()
                .WithMessage("O id do plano deve ser informado.");
        }
    }
}
