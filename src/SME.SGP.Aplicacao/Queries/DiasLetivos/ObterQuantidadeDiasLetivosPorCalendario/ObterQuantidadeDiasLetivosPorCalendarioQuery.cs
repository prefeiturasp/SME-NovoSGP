using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeDiasLetivosPorCalendarioQuery : IRequest<DiasLetivosDto>
    {
        public ObterQuantidadeDiasLetivosPorCalendarioQuery(long tipoCalendarioId, string dreCodigo, string ueCodigo)
        {
            TipoCalendarioId = tipoCalendarioId;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
        }

        public long TipoCalendarioId { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
    }


    public class ObterQuantidadeDiasLetivosPorCalendarioQueryValidator : AbstractValidator<ObterQuantidadeDiasLetivosPorCalendarioQuery>
    {
        public ObterQuantidadeDiasLetivosPorCalendarioQueryValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
               .NotEmpty()
               .WithMessage("O id do tipo de calendário deve ser informado para consulta de seus dias letivos.");

        }
    }
}
