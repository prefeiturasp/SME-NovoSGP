using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaCommand : IRequest<AuditoriaDto>
    {
        public SalvarItineranciaCommand(DateTime dataVisita, DateTime? dataRetornoVerificacao)
        {
            DataVisita = dataVisita;
            DataRetornoVerificacao = dataRetornoVerificacao;
        }

        public DateTime DataVisita { get; set; }
        public DateTime? DataRetornoVerificacao { get; set; }
    }
    public class SalvarItineranciaCommandValidator : AbstractValidator<SalvarItineranciaCommand>
    {
        public SalvarItineranciaCommandValidator()
        {
            RuleFor(x => x.DataVisita)
                   .NotEmpty()
                   .WithMessage("A Data da Visita da itinerância deve ser informada!");
        }
    }
}
