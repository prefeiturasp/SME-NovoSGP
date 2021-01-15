using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciasAulasCommand : IRequest<bool>
    {
        public SalvarPendenciasAulasCommand(long pendenciaId, IEnumerable<long> aulasIds)
        {
            PendenciaId = pendenciaId;
            AulasIds = aulasIds;
        }

        public long PendenciaId { get; set; }
        public IEnumerable<long> AulasIds { get; set; }
    }

    public class SalvarPendenciasAulasCommandValidator : AbstractValidator<SalvarPendenciasAulasCommand>
    {
        public SalvarPendenciasAulasCommandValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendência deve ser informado para geração de pendência aula.");

            RuleFor(c => c.AulasIds)
            .Must(a => a.Any())
            .WithMessage("Os ids das aulas devem ser informados para geração de pendência aula.");
        }
    }
}
