using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterEventoPorTipoEDataQuery : IRequest<IEnumerable<Evento>>
    {
        public ObterEventoPorTipoEDataQuery(TipoEvento tipoEvento, DateTime data)
        {
            TipoEvento = tipoEvento;
            Data = data;
        }

        public TipoEvento TipoEvento { get; set; }
        public DateTime Data { get; set; }
    }

    public class ObterEventoPorTipoEDataQueryValidator : AbstractValidator<ObterEventoPorTipoEDataQuery>
    {
        public ObterEventoPorTipoEDataQueryValidator()
        {
            RuleFor(c => c.Data)
               .Must(a => a > DateTime.MinValue)
               .WithMessage("A data do evento deve ser informada para consulta dos mesmos.");

        }
    }
}
