using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiAbrangenciaAdmQuery: IRequest<bool>
    {
        public ObterUsuarioPossuiAbrangenciaAdmQuery(long usuarioId)
        {
            UsuarioId = usuarioId;
        }

        public long UsuarioId { get; set; }
    }

    public class ObterUsuarioPossuiAbrangenciaAdmQueryValidator: AbstractValidator<ObterUsuarioPossuiAbrangenciaAdmQuery>
    {
        public ObterUsuarioPossuiAbrangenciaAdmQueryValidator()
        {
            RuleFor(a => a.UsuarioId)
                .NotEmpty()
                .WithMessage("Necessário informar o usuario para validação da abrangência.");
        }
    }
}
