using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDetalhamentoPendenciaFechamentoConsolidadoQuery : IRequest<DetalhamentoPendenciaFechamentoConsolidadoDto>
    {
        public ObterDetalhamentoPendenciaFechamentoConsolidadoQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ObterDetalhamentoPendenciaFechamentoConsolidadoQueryValidator : AbstractValidator<ObterDetalhamentoPendenciaFechamentoConsolidadoQuery>
    {
        public ObterDetalhamentoPendenciaFechamentoConsolidadoQueryValidator()
        {
            RuleFor(a => a.PendenciaId)
                .NotEmpty()
                .WithMessage("O id da pendência deve ser informado.");
        }
    }
}
