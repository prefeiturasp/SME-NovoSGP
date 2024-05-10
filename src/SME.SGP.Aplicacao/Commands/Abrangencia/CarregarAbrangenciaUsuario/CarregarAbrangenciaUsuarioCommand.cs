using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class CarregarAbrangenciaUsuarioCommand : IRequest
    {
        public CarregarAbrangenciaUsuarioCommand(string login, Guid perfil)
        {
            Login = login;
            Perfil = perfil;
        }

        public string Login { get; }
        public Guid Perfil { get; }
    }

    public class CarregarAbrangenciaUsuarioCommandValidator : AbstractValidator<CarregarAbrangenciaUsuarioCommand>
    {
        public CarregarAbrangenciaUsuarioCommandValidator()
        {
            RuleFor(a => a.Login)
                .NotEmpty()
                .WithMessage("Login do usuário deve ser informado para carga de sua abrangência");

            RuleFor(a => a.Perfil)
                .NotEmpty()
                .WithMessage("Perfil do usuário deve ser informado para carga de sua abrangência");
        }
    }
}
