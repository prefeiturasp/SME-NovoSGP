using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery : IRequest<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>>
    {
        public ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery(string turmaCodigo, int anoLetivo, long[] componentesCurricularesIds)
        {
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
            ComponentesCurricularesIds = componentesCurricularesIds;
        }

        public string TurmaCodigo { get; }
        public int AnoLetivo { get; set; }
        public long[] ComponentesCurricularesIds { get; set; }
    }

    public class ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQueryValidator : AbstractValidator<ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery>
    {
        public ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consolidar os registros pedagógicos");

            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consolidar os registros pedagógicos");

            RuleFor(a => a.ComponentesCurricularesIds)
                .NotEmpty()
                .WithMessage("Os componentes curriculares devem ser informados para consolidar os registros pedagógicos.");
        }
    }
}