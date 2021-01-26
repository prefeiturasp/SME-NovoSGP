using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsPorModalidadeCalendarioQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUEsPorModalidadeCalendarioQuery(ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
    }

    public class ObterUEsPorModalidadeCalendarioQueryValidator : AbstractValidator<ObterUEsPorModalidadeCalendarioQuery>
    {
        public ObterUEsPorModalidadeCalendarioQueryValidator()
        {
            RuleFor(c => c.ModalidadeTipoCalendario)
            .NotEmpty()
            .WithMessage("A modalidade do tipo de calendario deve ser informada para consulta das UEs.");

        }
    }
}
