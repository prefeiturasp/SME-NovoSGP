using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterUesIdsPorModalidadeCalendarioQuery : IRequest<IEnumerable<long>>
    {
        public ObterUesIdsPorModalidadeCalendarioQuery(ModalidadeTipoCalendario modalidadeTipoCalendario, int anoLetivo = 0)
        {
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
            AnoLetivo = anoLetivo;
        }

        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; }
        public int AnoLetivo { get; }
    }

    public class ObterUesIdsPorModalidadeCalendario : AbstractValidator<ObterUesIdsPorModalidadeCalendarioQuery>
    {
        public ObterUesIdsPorModalidadeCalendario()
        {
            RuleFor(a => a.ModalidadeTipoCalendario)
                .NotEmpty()
                .WithMessage("A modalidade do tipo de calendario deve ser informada para consulta das identificações das UEs.");
        }
    }
}
