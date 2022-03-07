using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery : IRequest<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>>
    {
        public ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery(long ueId, int anoLetivo)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
        }

        public long UeId { get; }
        public int AnoLetivo { get; set; }
    }

    public class ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQueryValidator : AbstractValidator<ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery>
    {
        public ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQueryValidator()
        {
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O identificador da UE deve ser informado para consolidar os registros pedagógicos");

            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consolidar os registros pedagógicos");
        }
    }
}