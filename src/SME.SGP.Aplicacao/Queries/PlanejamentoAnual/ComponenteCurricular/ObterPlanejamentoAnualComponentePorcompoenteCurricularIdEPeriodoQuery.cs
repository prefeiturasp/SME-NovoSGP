using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualComponentePorcompoenteCurricularIdEPeriodoQuery : IRequest<PlanejamentoAnualComponente>
    {
        public ObterPlanejamentoAnualComponentePorcompoenteCurricularIdEPeriodoQuery(long componenteCurricularId, long periodoId)
        {
            ComponenteCurricularId = componenteCurricularId;
            PeriodoId = periodoId;
        }

        public long ComponenteCurricularId { get; set; }
        public long PeriodoId { get; set; }
    }

    public class ObterPlanejamentoAnualComponentePorcompoenteCurricularIdEPeriodoQueryValidator : AbstractValidator<ObterPlanejamentoAnualComponentePorcompoenteCurricularIdEPeriodoQuery>
    {
        public ObterPlanejamentoAnualComponentePorcompoenteCurricularIdEPeriodoQueryValidator()
        {
            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O id do componente curricular precisa ser informado.");
            RuleFor(c => c.PeriodoId)
               .NotEmpty()
               .WithMessage("O id do periodo precisa ser informado.");
        }
    }
}
