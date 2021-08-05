using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaDresQuery : IRequest<IEnumerable<AbrangenciaDreRetornoDto>>
    {
        public Modalidade? Modalidade { get; set; }
        public int Periodo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public string Filtro { get; set; }
        public bool FiltroEhCodigo { get; set; }

        public string Login { get; set; }
        public Guid Perfil { get; set; }


        public ObterAbrangenciaDresQuery(string login, Guid perfil, Modalidade? modalidade, int periodo, bool consideraHistorico, int anoLetivo, string filtro, bool filtroEhCodigo)
        {
            Login = login;
            Perfil = perfil;
            Modalidade = modalidade;
            Periodo = periodo;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
            Filtro = filtro;
            FiltroEhCodigo = filtroEhCodigo;
        }

    }
    public class ObterAbrangenciaDresQueryValidator : AbstractValidator<ObterAbrangenciaDresQuery>
    {
        public ObterAbrangenciaDresQueryValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("O login do usuário logado deve ser informado.");

            RuleFor(x => x.Perfil)
                .NotEmpty()
                .WithMessage("O perfil do usuário logado deve ser informado.");
        }
    }
}
