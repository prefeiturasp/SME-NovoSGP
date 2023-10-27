using System;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class CarregarDadosAcessoPorLoginPerfilQuery : IRequest<RetornoDadosAcessoUsuarioSgpDto>
    {
        public CarregarDadosAcessoPorLoginPerfilQuery(string login, Guid perfilGuid, AdministradorSuporteDto administradorSuporte = null)
        {
            PerfilGuid = perfilGuid;
            Login = login;
            AdministradorSuporte = administradorSuporte;
        }
        
        public Guid PerfilGuid { get; set; }
        public string Login { get; set; }
        
        public AdministradorSuporteDto AdministradorSuporte{ get; set; }
    }

    public class CarregarDadosAcessoPorLoginPerfilQueryValidator : AbstractValidator<CarregarDadosAcessoPorLoginPerfilQuery>
    {
        public CarregarDadosAcessoPorLoginPerfilQueryValidator()
        {
            RuleFor(x => x.PerfilGuid)
                .NotEmpty()
                .WithMessage("O perfil do usuário deve ser informado para buscar os dados de acesso do usuário.");
            
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("O login do usuário deve ser informado para buscar os dados de acesso do usuário.");
        }
    }
}