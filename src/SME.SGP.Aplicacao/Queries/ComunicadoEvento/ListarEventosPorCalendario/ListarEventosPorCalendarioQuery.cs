using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarEventosPorCalendarioQuery : IRequest<IEnumerable<EventoCalendarioRetornoDto>>
    {
        public ListarEventosPorCalendarioQuery(long tipoCalendario,
                                               int anoLetivo,
                                               string codigoDre,
                                               string codigoUe,
                                               IEnumerable<int> modalidades)
        {
            TipoCalendario = tipoCalendario;
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            Modalidades = modalidades;
        }

        public long TipoCalendario { get; }
        public int AnoLetivo { get; }
        public string CodigoDre { get; }
        public string CodigoUe { get; }
        public IEnumerable<int> Modalidades { get; }
    }
    public class ListarEventosPorCalendarioQueryValidator : AbstractValidator<ListarEventosPorCalendarioQuery>
    {
        public ListarEventosPorCalendarioQueryValidator()
        {
            RuleFor(c => c.TipoCalendario)
                .NotEmpty()
                .WithMessage("O id do tipo de calendário deve ser informado.");

            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Modalidades)
                .NotEmpty()
                .WithMessage("Pelo menos uma modalidade deve ser informada.");
        }
    }
}
