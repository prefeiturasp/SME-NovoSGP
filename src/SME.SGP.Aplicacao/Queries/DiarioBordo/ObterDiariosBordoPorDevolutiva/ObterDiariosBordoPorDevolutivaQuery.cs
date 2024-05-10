using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosBordoPorDevolutivaQuery : IRequest<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>>
    {
        public ObterDiariosBordoPorDevolutivaQuery(long devolutivaId, int anoLetivo)
        {
            DevolutivaId = devolutivaId;
            AnoLetivo = anoLetivo;
        }

        public long DevolutivaId { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterDiariosBordoPorDevolutivaQueryValidator : AbstractValidator<ObterDiariosBordoPorDevolutivaQuery>
    {
        public ObterDiariosBordoPorDevolutivaQueryValidator()
        {
            RuleFor(a => a.DevolutivaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da devolutiva para consulta dos diários de bordo");
        }
    }
}
