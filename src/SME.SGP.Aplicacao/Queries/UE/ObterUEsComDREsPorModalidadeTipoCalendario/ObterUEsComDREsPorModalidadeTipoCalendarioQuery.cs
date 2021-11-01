using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsComDREsPorModalidadeTipoCalendarioQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUEsComDREsPorModalidadeTipoCalendarioQuery(ModalidadeTipoCalendario modalidadeTipoCalendario, int anoLetivo)
        {
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
            AnoLetivo = anoLetivo;
        }

        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterUEsComDREsPorModalidadeTipoCalendarioQueryValidator : AbstractValidator<ObterUEsComDREsPorModalidadeTipoCalendarioQuery>
    {
        public ObterUEsComDREsPorModalidadeTipoCalendarioQueryValidator()
        {
            RuleFor(c => c.ModalidadeTipoCalendario)
            .NotEmpty()
            .WithMessage("A modalidade do tipo de calendario deve ser informada para consulta das UEs.");

            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O ano letivo deve ser informada para consulta das UEs.");

        }
    }
}
