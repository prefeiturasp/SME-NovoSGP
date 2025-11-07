using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GravarHistoricoReinicioSenhaCommand: IRequest<bool>
    {
        public GravarHistoricoReinicioSenhaCommand(string usuarioRf, string dreCodigo, string ueCodigo)
        {
            UsuarioRf = usuarioRf;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
        }

        public string UsuarioRf { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
    }

    public class GravarHistoricoReinicioSenhaCommandValidator: AbstractValidator<GravarHistoricoReinicioSenhaCommand>
    {
        public GravarHistoricoReinicioSenhaCommandValidator()
        {
            RuleFor(a => a.UsuarioRf)
                .NotEmpty()
                .WithMessage("RF do usuario alterado deve ser informado!");
        }
    }
}
