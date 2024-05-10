using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery : IRequest<IEnumerable<AbrangenciaTurmaRetorno>>
    {
        public ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(string codigoUe, Modalidade modalidade, int periodo, bool consideraHistorico, int anoLetivo, int[] tipos, bool consideraNovosAnosInfantil = false)
        {
            CodigoUe = codigoUe;
            Modalidade = modalidade;
            Periodo = periodo;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
            Tipos = tipos;
            ConsideraNovosAnosInfantil = consideraNovosAnosInfantil;
        }

        public string CodigoUe { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Periodo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public int[] Tipos { get; set; }
        public bool ConsideraNovosAnosInfantil { get; set; }
    }
    public class ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryValidator : AbstractValidator<ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery>
    {
        public ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryValidator()
        {
            RuleFor(x => x.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para a pesquisa de abrangência da turma.");

            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para a pesquisa de abrangência da turma.");
        }
    }
}
