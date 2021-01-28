using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class EhGestorDaEscolaQuery : IRequest<bool>
    {
        public EhGestorDaEscolaQuery(string usuarioRf, string ueCodigo, Guid perfil)
        {
            UsuarioRf = usuarioRf;
            UeCodigo = ueCodigo;
            Perfil = perfil;
        }

        public string UsuarioRf { get; }
        public string UeCodigo { get; }
        public Guid Perfil { get; }
    }

    public class EhGestorDaEscolaQueryValidator : AbstractValidator<EhGestorDaEscolaQuery>
    {
        public EhGestorDaEscolaQueryValidator()
        {
            RuleFor(a => a.UsuarioRf)
                .NotEmpty()
                .WithMessage("O RF do usuário deve ser informado para consulta da gestão escolar");
            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para consulta da gestão escolar");
            RuleFor(a => a.Perfil)
                .NotEmpty()
                .WithMessage("O perfil do usuário deve ser informado para consulta da gestão escolar");
        }
    }
}
