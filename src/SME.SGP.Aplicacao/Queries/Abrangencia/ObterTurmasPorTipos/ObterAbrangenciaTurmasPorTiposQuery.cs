using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmasPorTiposQuery : IRequest<IEnumerable<AbrangenciaTurmaRetorno>>
    {
        public ObterAbrangenciaTurmasPorTiposQuery(string codigoUe, string login, Guid perfil, Modalidade modalidade, int[] tipos, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string[] anosInfantilDesconsiderar = null)
        {
            CodigoUe = codigoUe;
            Login = login;
            Perfil = perfil;
            Modalidade = modalidade;
            Tipos = tipos;
            Periodo = periodo;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
            AnosInfantilDesconsiderar = anosInfantilDesconsiderar;
        }

        public string CodigoUe { get; set; }
        public string Login { get; set; }
        public Guid Perfil { get; set; }
        public Modalidade Modalidade { get; set; }
        public int[] Tipos { get; set; }
        public int Periodo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public string[] AnosInfantilDesconsiderar { get; set; }
    }

    public class ObterTurmasPorTiposQueryValidator : AbstractValidator<ObterAbrangenciaTurmasPorTiposQuery>
    {
        public ObterTurmasPorTiposQueryValidator()
        {
            RuleFor(x => x.CodigoUe).NotEmpty().WithMessage("Informe o Codigo UE para consultar a turma");
            RuleFor(x => x.Perfil).NotEmpty().WithMessage("Informe o Perfil para consultar a turma");
            RuleFor(x => x.Login).NotEmpty().WithMessage("Informe o Login para consultar a turma");
        }
    }
}