using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsPorDREQuery : IRequest<IEnumerable<AbrangenciaUeRetorno>>
    {
        public ObterUEsPorDREQuery(string codigoDre, string login, Guid perfil, Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, bool consideraNovasUEs = false, bool filtrarTipoEscolaPorAnoLetivo = false, string filtro = "", bool filtroEhCodigo = false)
        {
            CodigoDre = codigoDre;
            Login = login;
            Perfil = perfil;
            Modalidade = modalidade;
            Periodo = periodo;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
            ConsideraNovasUEs = consideraNovasUEs;
            FiltrarTipoEscolaPorAnoLetivo = filtrarTipoEscolaPorAnoLetivo;
            Filtro = filtro;
            FiltroEhCodigo = filtroEhCodigo;
        }

        public string CodigoDre { get; }
        public string Login { get; }
        public Guid Perfil { get; }
        public Modalidade? Modalidade { get; }
        public int Periodo { get; }
        public bool ConsideraHistorico { get; }
        public int AnoLetivo { get; }
        public bool ConsideraNovasUEs { get; }
        public bool FiltrarTipoEscolaPorAnoLetivo { get; }
        public string Filtro { get; }
        public bool FiltroEhCodigo { get; }
    }

    public class ObterUEsPorDREQueryValidator : AbstractValidator<ObterUEsPorDREQuery>
    {
        public ObterUEsPorDREQueryValidator()
        {
            RuleFor(a => a.CodigoDre)
                .NotEmpty()
                .WithMessage("O código da DRE deve ser informado para consulta de suas UEs");

            RuleFor(a => a.Login)
                .NotEmpty()
                .WithMessage("O login do usuário deve ser informado para consulta de suas UEs");

            RuleFor(a => a.Perfil)
                .NotEmpty()
                .WithMessage("O perfil do usuário deve ser informado para consulta de suas UEs");
        }
    }
}
