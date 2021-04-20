using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SolicitaRelatorioItineranciaCommand : IRequest<Guid>
    {
        public SolicitaRelatorioItineranciaCommand(FiltroRelatorioItineranciaDto filtro)
        {
            Filtro = filtro;
        }

        public FiltroRelatorioItineranciaDto Filtro { get; set; }
    }

    public class SolicitaRelatorioItineranciaCommandValidator : AbstractValidator<SolicitaRelatorioItineranciaCommand>
    {
        public SolicitaRelatorioItineranciaCommandValidator()
        {
            RuleFor(c => c.Filtro)
               .NotEmpty()
               .WithMessage("O filtro deve ser informado para solicitação do relatório.");

        }
    }
}
