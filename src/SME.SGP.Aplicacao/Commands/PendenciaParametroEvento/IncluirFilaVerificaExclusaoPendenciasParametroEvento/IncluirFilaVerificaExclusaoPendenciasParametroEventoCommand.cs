using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaVerificaExclusaoPendenciasParametroEventoCommand : IRequest<bool>
    {
        public IncluirFilaVerificaExclusaoPendenciasParametroEventoCommand(long tipoCalendarioId, string ueCodigo, TipoEvento tipoEvento, Usuario usuario)
        {
            TipoCalendarioId = tipoCalendarioId;
            UeCodigo = ueCodigo;
            TipoEvento = tipoEvento;
            Usuario = usuario;
        }

        public long TipoCalendarioId { get; set; }
        public string UeCodigo { get; set; }
        public TipoEvento TipoEvento { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class IncluirFilaVerificaExclusaoPendenciasParametroEventoCommandValidator : AbstractValidator<IncluirFilaVerificaExclusaoPendenciasParametroEventoCommand>
    {
        public IncluirFilaVerificaExclusaoPendenciasParametroEventoCommandValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
               .Must(a => a > 0)
               .WithMessage("O id do tipo de calendário deve ser informado.");

            RuleFor(c => c.UeCodigo)
               .NotEmpty()
               .WithMessage("O id da UE de calendário deve ser informado.");

            RuleFor(c => c.TipoEvento)
               .NotEmpty()
               .WithMessage("O tipo de evento deve ser informado.");

            RuleFor(c => c.Usuario)
               .NotEmpty()
               .WithMessage("O usuário deve ser informado.");
        }
    }
}
