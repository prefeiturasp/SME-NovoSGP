using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery : IRequest<IEnumerable<DevolutivaTurmaDTO>>
    {
        public ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery(int anoLetivo, long ueId = 0)
        {
            AnoLetivo = anoLetivo;
            UeId = ueId;
        }

        public int AnoLetivo { get; set; }
        public long UeId { get; set; }
    }

    public class ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQueryValidator : AbstractValidator<ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery>
    {
        public ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O Ano Letivo deve ser informado para consulta de turmas com devolutivas.");

        }
    }
}
