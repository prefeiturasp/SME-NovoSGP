using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Usuario.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados
{
    public class ObterDadosDeLeituraDeComunicadosQuery : IRequest<IEnumerable<DadosDeLeituraDoComunicadoDto>>
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public long ComunicadoId { get; set; }

        public ObterDadosDeLeituraDeComunicadosQuery(string codigoDre, string codigoUe, long comunicadoId)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            ComunicadoId = comunicadoId;
        }
    }

    public class ObterDadosDeLeituraDeComunicadosQueryValidator : AbstractValidator<ObterDadosDeLeituraDeComunicadosQuery>
    {
        public ObterDadosDeLeituraDeComunicadosQueryValidator()
        {
            RuleFor(x => x.ComunicadoId)
                .NotEmpty()
                .WithMessage("O comunicado é obrigatório.");
        }
    }
}