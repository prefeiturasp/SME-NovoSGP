using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirAulaEmManutencaoCommand: IRequest<IEnumerable<ProcessoExecutando>>
    {
        public InserirAulaEmManutencaoCommand(IEnumerable<long> aulasIds)
        {
            AulasIds = aulasIds;
        }

        public IEnumerable<long> AulasIds { get; set; }
    }

    public class InserirAulaEmManutencaoCommandValidator: AbstractValidator<InserirAulaEmManutencaoCommand>
    {
        public InserirAulaEmManutencaoCommandValidator()
        {
            RuleFor(c => c.AulasIds)
                .NotEmpty()
                .WithMessage("Necessário informar os ids das aulas para incluir os registros em manutenção.");
        }
    }
}
