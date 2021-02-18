using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterNomeTipoCalendarioPorIdQuery : IRequest<string>
    {
        public ObterNomeTipoCalendarioPorIdQuery(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; set; }
    }

    public class ObterNomeTipoCalendarioPorIdQueryValidator : AbstractValidator<ObterNomeTipoCalendarioPorIdQuery>
    {
        public ObterNomeTipoCalendarioPorIdQueryValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
            .NotEmpty()
            .WithMessage("O id do tipo de calendário deve ser informado para busca da descrição.");

        }
    }
}
