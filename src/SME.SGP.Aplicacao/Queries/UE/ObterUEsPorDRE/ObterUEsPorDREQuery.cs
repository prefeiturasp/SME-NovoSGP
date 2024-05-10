using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsPorDREQuery : IRequest<IEnumerable<AbrangenciaUeRetorno>>
    {
        public ObterUEsPorDREQuery(UEsPorDreDto dto, string login, Guid perfil)
        {
            CodigoDre = dto.CodigoDre;
            Login = login;
            Perfil = perfil;
            Modalidade = dto.Modalidade;
            Periodo = dto.Periodo;
            ConsideraHistorico = dto.ConsideraHistorico;
            AnoLetivo = dto.AnoLetivo;
            ConsideraNovasUEs = dto.ConsideraNovasUEs;
            FiltrarTipoEscolaPorAnoLetivo = dto.FiltrarTipoEscolaPorAnoLetivo;
            Filtro = dto.Filtro;
            FiltroEhCodigo = !string.IsNullOrWhiteSpace(dto.Filtro) && dto.Filtro.All(char.IsDigit);
        }
        public ObterUEsPorDREQuery(string codigoDre, string login, Guid perfil, bool consideraHistorico, int anoLetivo)
        {
            CodigoDre = codigoDre;
            Login = login;
            Perfil = perfil;
            Modalidade = null;
            Periodo = 0;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
            ConsideraNovasUEs = false;
            FiltrarTipoEscolaPorAnoLetivo = false;
            Filtro = string.Empty;
            FiltroEhCodigo = false;
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
