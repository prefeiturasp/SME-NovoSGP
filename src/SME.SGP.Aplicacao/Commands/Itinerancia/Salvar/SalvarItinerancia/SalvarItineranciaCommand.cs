using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaCommand : IRequest<AuditoriaDto>
    {
        public SalvarItineranciaCommand(int anoLetivo, DateTime dataVisita, DateTime? dataRetornoVerificacao)
        {
            AnoLetivo = anoLetivo;
            DataVisita = dataVisita;
            DataRetornoVerificacao = dataRetornoVerificacao;
        }

        public int AnoLetivo { get; set; }
        public DateTime DataVisita { get; set; }
        public DateTime? DataRetornoVerificacao { get; set; }
    }
    public class SalvarItineranciaCommandValidator : AbstractValidator<SalvarItineranciaCommand>
    {
        public SalvarItineranciaCommandValidator()
        {
            RuleFor(x => x.AnoLetivo)
                   .NotEmpty()
                   .WithMessage("O ano letivo da itinerância deve ser informado!");
            RuleFor(x => x.DataVisita)
                   .NotEmpty()
                   .WithMessage("A data da visita da itinerância deve ser informada!");

            RuleFor(x => x.DataRetornoVerificacao.Value.Date)
                .GreaterThan( a => a.DataVisita.Date)
                .WithMessage("A data de retorno/verificação não pode ser menor ou igual que a data de visita")
                .When( a => a.DataRetornoVerificacao.HasValue);


        }
    }
}
