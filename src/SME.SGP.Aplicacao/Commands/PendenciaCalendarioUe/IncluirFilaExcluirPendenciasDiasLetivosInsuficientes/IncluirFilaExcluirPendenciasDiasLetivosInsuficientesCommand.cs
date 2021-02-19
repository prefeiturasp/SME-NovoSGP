using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand : IRequest<bool>
    {
        public IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand(long tipoCalendarioId, string dreCodigo, string ueCodigo, Usuario usuario)
        {
            TipoCalendarioId = tipoCalendarioId;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            Usuario = usuario;
        }

        public long TipoCalendarioId { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommandValidator : AbstractValidator<IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand>
    {
        public IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommandValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
               .Must(a => a > 0)
               .WithMessage("O id do tipo de calendário deve ser informado.");

            RuleFor(c => c.DreCodigo)
               .NotEmpty()
               .WithMessage("O id da DRE de calendário deve ser informado.");

            RuleFor(c => c.UeCodigo)
               .NotEmpty()
               .WithMessage("O id da UE de calendário deve ser informado.");

            RuleFor(c => c.Usuario)
               .NotEmpty()
               .WithMessage("O usuário deve ser informado.");
        }
    }
}
