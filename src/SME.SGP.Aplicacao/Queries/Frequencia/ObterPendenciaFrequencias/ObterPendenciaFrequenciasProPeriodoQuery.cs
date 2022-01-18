using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaFrequenciasProPeriodoQuery : IRequest<bool>
    {
        public ObterPendenciaFrequenciasProPeriodoQuery(string turmaCodigo, string componenteCurricularId, DateTime dataLimite, int anoLetivo, short bimestre)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularId = componenteCurricularId;
            DataLimite = dataLimite;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
        }

        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularId { get; set; }
        public DateTime DataLimite { get; set; }
        public int AnoLetivo { get; set; }
        public short Bimestre { get; set; }

    }

    public class ObterPendenciaFrequenciasProPeriodoQueryValidator : AbstractValidator<ObterPendenciaFrequenciasProPeriodoQuery>
    {
        public ObterPendenciaFrequenciasProPeriodoQueryValidator()
        {
            RuleFor(x => x.TurmaCodigo)
             .NotEmpty()
             .WithMessage("O codigo da turma deve ser informado.");

            RuleFor(x => x.ComponenteCurricularId)
             .NotEmpty()
             .WithMessage("O componente curricular deve ser informado.");

            RuleFor(x => x.DataLimite)
             .NotEmpty()
             .WithMessage("A data limite deve ser informada.");

            RuleFor(x => x.AnoLetivo)
             .NotEmpty()
             .WithMessage("O ano letivo deve ser informado.");

            RuleFor(x => x.Bimestre)
             .NotEmpty()
             .WithMessage("O bimestre deve ser informado.");
        }
    }
}
