using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaCommand : IRequest<AuditoriaDto>
    {
        public SalvarItineranciaCommand(int anoLetivo, DateTime dataVisita, DateTime? dataRetornoVerificacao, long? eventoId, long dreId, long ueId)
        {
            AnoLetivo = anoLetivo;
            DataVisita = dataVisita;
            DataRetornoVerificacao = dataRetornoVerificacao;
            EventoId = eventoId;
            DreId = dreId;
            UeId = ueId;
        }

        public int AnoLetivo { get; set; }
        public DateTime DataVisita { get; set; }
        public DateTime? DataRetornoVerificacao { get; set; }
        public long? EventoId { get; }

        public long DreId { get; }
        public long UeId { get; }
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

            RuleFor(x => x.UeId)
                .NotEmpty()
                .WithMessage("O Id da UE deve ser informado!");

            RuleFor(x => x.DataRetornoVerificacao.Value.Date)
                .GreaterThan( a => a.DataVisita.Date)
                .WithMessage("A data de retorno/verificação não pode ser menor ou igual que a data de visita")
                .When( a => a.DataRetornoVerificacao.HasValue);
        }
    }
}
