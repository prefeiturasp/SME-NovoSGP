using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanejamentoAnualCommand : IRequest<AuditoriaDto>
    {
        public long PeriodoEscolarId { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long Id { get; set; }
        public List<PlanejamentoAnualComponenteDto> Componentes { get; set; }
    }


    public class SalvarPlanejamentoAnualCommandValidator : AbstractValidator<SalvarPlanejamentoAnualCommand>
    {
        public SalvarPlanejamentoAnualCommandValidator()
        {

            RuleFor(c => c.PeriodoEscolarId)
            .NotEmpty()
            .WithMessage("O Período Escolar deve ser informado.");

            RuleFor(c => c.ComponenteCurricularId)
            .NotEmpty()
            .WithMessage("O Componente Curricular deve ser informado.");

        }
    }

}
