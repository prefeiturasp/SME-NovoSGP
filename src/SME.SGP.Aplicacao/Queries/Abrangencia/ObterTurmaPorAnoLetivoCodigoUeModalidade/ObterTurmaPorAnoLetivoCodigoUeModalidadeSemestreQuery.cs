using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Queries.Abrangencia.ObterTurmaPorAnoLetivoCodigoUeModalidade
{
    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery : IRequest<IEnumerable<OpcaoDropdownDto>>
    {
        public ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery(int anoLetivo, string codigoUe, Modalidade? modalidade, int semestre)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
            Modalidade = modalidade;
            Semestre = semestre;
        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public Modalidade? Modalidade { get; set; }
        public int Semestre { get; set; }
    }

    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQueryValidator : AbstractValidator<ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery>
    {
        public ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(1900).WithMessage("O Ano letivo deve ser informado");
            RuleFor(x => x.CodigoUe).NotEmpty().WithMessage("O Codigo da Ue deve ser informado");
        }
    }
}
