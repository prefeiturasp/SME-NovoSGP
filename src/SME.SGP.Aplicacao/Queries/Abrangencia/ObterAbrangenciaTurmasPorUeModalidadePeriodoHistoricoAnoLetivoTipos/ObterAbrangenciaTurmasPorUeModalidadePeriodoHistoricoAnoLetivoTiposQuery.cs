using FluentValidation;
using MediatR;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery : IRequest<IEnumerable<AbrangenciaTurmaRetorno>>
    {
        public ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(string codigoUe, FiltroModalidade filtroModalidade, FiltroPeriodoLetivo filtroPeriodoLetivo, int[] tipos, bool consideraNovosAnosInfantil = false)
        {
            CodigoUe = codigoUe;
            FiltroModalidade = filtroModalidade;
            FiltroPeriodoLetivo = filtroPeriodoLetivo;
            Tipos = tipos;
            ConsideraNovosAnosInfantil = consideraNovosAnosInfantil;
        }

        public string CodigoUe { get; set; }
        public FiltroModalidade FiltroModalidade { get; set; }
        public FiltroPeriodoLetivo FiltroPeriodoLetivo { get; set; }
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

            RuleFor(x => x.FiltroPeriodoLetivo.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para a pesquisa de abrangência da turma.");
        }
    }
}
