using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class VerificarUsuarioLogadoPertenceMesmaUEQuery : IRequest<bool>
    {
        public VerificarUsuarioLogadoPertenceMesmaUEQuery(string login, Guid perfil, string codigoUe, Modalidade modalidade, bool consideraHistorico, int anoLetivo, int periodo = 0, bool consideraNovosAnosInfantil = false)
        {
            CodigoUe = codigoUe;
            Modalidade = modalidade;
            Periodo = periodo;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
            ConsideraNovosAnosInfantil = consideraNovosAnosInfantil;
            Login = login;
            Perfil = perfil;
        }
        
        public Guid Perfil { get; set; }
        public string Login { get; set; }
        public string CodigoUe { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Periodo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public bool ConsideraNovosAnosInfantil { get; set; }
    }
    public class VerificarUsuarioLogadoPertenceMesmaUEQueryValidator : AbstractValidator<VerificarUsuarioLogadoPertenceMesmaUEQuery>
    {
        public VerificarUsuarioLogadoPertenceMesmaUEQueryValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para a verificação se está na mesma Ue.");
            
            RuleFor(x => x.Perfil)
                .NotNull()
                .WithMessage("O perfil do usuário deve ser informado para a verificação se está na mesma Ue.");
            
            RuleFor(x => x.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para a verificação se está na mesma Ue.");

            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para a verificação se está na mesma Ue.");
            
            RuleFor(x => x.Modalidade)
                .NotNull()
                .WithMessage("A modalidade deve ser informado para a verificação se está na mesma Ue.");
        }
    }
}
