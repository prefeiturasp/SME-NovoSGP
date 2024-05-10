using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class VerificaExistenciaDePendenciaPerfilUsuarioQuery : IRequest<bool>
    {
        public VerificaExistenciaDePendenciaPerfilUsuarioQuery(long pendenciaPerfilId, long usuarioId)
        {
            PendenciaPerfilId = pendenciaPerfilId;
            UsuarioId = usuarioId;
        }
        public long PendenciaPerfilId { get; set; }
        public long UsuarioId { get; set; }
    }

    public class VerificaExistenciaDePendenciaPerfilUsuarioQueryValidator : AbstractValidator<VerificaExistenciaDePendenciaPerfilUsuarioQuery>
    {
        public VerificaExistenciaDePendenciaPerfilUsuarioQueryValidator()
        {
            RuleFor(a => a.PendenciaPerfilId)
                .NotEmpty()
                .WithMessage("O código da pendencia perfil deve ser informado para consulta da existência de uma pendência perfil usuario.");

            RuleFor(a => a.PendenciaPerfilId)
                .NotEmpty()
                .WithMessage("O código do usuário deve ser informado para consulta da existência de uma pendência perfil usuario.");
        }
    }
}
