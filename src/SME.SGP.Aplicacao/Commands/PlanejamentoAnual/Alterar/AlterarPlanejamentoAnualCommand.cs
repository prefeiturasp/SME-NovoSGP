using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class AlterarPlanejamentoAnualCommand : IRequest<AuditoriaDto>
    {
        public long Id { get; set; }
        public long PeriodoEscolarId { get; set; }
        public long PlanejamentoAnualPeriodoEscolarId { get; set; }
        public IEnumerable<PlanejamentoAnualComponenteDto> Componentes { get; set; }
    }


    public class AlterarPlanejamentoAnualCommandValidator : AbstractValidator<AlterarPlanejamentoAnualCommand>
    {
        public AlterarPlanejamentoAnualCommandValidator()
        {

            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O id do planejamento anual deve ser informado.");

            RuleFor(c => c.PeriodoEscolarId)
                .NotEmpty()
                .WithMessage("O período escolar deve ser informado.");
        }
    }
}
