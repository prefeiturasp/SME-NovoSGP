using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterVersaoPlanoAEEPorIdQuery : IRequest<PlanoAEEVersaoDto>
    {
        public ObterVersaoPlanoAEEPorIdQuery(long versaoPlanoId)
        {
            VersaoPlanoId = versaoPlanoId;
        }

        public long VersaoPlanoId { get; set; }
    }

    public class ObterVersaoPlanoAEEPorIdQueryValidator : AbstractValidator<ObterVersaoPlanoAEEPorIdQuery>
    {
        public ObterVersaoPlanoAEEPorIdQueryValidator()
        {
            RuleFor(a => a.VersaoPlanoId)
                .NotEmpty()
                .WithMessage("O id da versão do plano deve ser informada.");
        }
    }
}
