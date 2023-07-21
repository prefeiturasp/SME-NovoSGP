using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorUeAnoTiposTurmaQuery : IRequest<IEnumerable<TurmaDTO>>
    {
        public ObterTurmasPorUeAnoTiposTurmaQuery(long ueId, int anoLetivo, int[] tiposTurma)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
            TiposTurma = tiposTurma;
        }

        public long UeId { get; }
        public int AnoLetivo { get; }
        public int[] TiposTurma { get; }
    }

    public class ObterTurmasPorUeAnoTiposTurmaQueryValidator: AbstractValidator<ObterTurmasPorUeAnoTiposTurmaQuery>
    {
        public ObterTurmasPorUeAnoTiposTurmaQueryValidator()
        {
            RuleFor(t => t.UeId)
                .NotEmpty()
                .WithMessage("O Id da ue deve ser informado para consultar as turmas");

            RuleFor(t => t.AnoLetivo)
                .NotEmpty()
                .WithMessage("O Ano letivo deve ser informado para consultar as turmas");

            RuleFor(t => t.TiposTurma)
                .NotEmpty()
                .WithMessage("Os tipos de turmas deve ser informado para consultar as turmas");
        }
    }
}
