using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaParametroEventoCommand : IRequest<long>
    {
        public SalvarPendenciaParametroEventoCommand(long pendenciaCalendarioUeId, long parametroSistemaId, int quantidadeEventos)
        {
            PendenciaCalendarioUeId = pendenciaCalendarioUeId;
            ParametroSistemaId = parametroSistemaId;
            QuantidadeEventos = quantidadeEventos;
        }

        public long PendenciaCalendarioUeId { get; set; }
        public long ParametroSistemaId { get; set; }
        public int QuantidadeEventos { get; set; }
    }

    public class SalvarPendenciaParametroEventoCommandValidator : AbstractValidator<SalvarPendenciaParametroEventoCommand>
    {
        public SalvarPendenciaParametroEventoCommandValidator()
        {
            RuleFor(c => c.PendenciaCalendarioUeId)
            .NotEmpty()
            .WithMessage("O id da pendência calendário UE deve ser informado para geração de pendência parâmetro.");

            RuleFor(c => c.ParametroSistemaId)
            .NotEmpty()
            .WithMessage("O id do parâmetro do sistema deve ser informado para geração de pendência parâmetro.");
        }
    }
}
