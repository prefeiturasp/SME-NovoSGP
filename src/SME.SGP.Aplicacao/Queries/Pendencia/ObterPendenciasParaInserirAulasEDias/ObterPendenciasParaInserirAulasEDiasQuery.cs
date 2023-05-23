using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParaInserirAulasEDiasQuery : IRequest<IEnumerable<AulasDiasPendenciaDto>>
    {
        public ObterPendenciasParaInserirAulasEDiasQuery(int? anoLetivo,long ueid)
        {
            AnoLetivo = anoLetivo;
            UeId = ueid;
        }

        public int? AnoLetivo { get; set; }
        public long UeId { get; set; }
    }

    public class ObterPendenciasParaInserirAulasEDiasQueryValidator : AbstractValidator<ObterPendenciasParaInserirAulasEDiasQuery>
    {
        public ObterPendenciasParaInserirAulasEDiasQueryValidator()
        {
            RuleFor(x => x.UeId).GreaterThan(0).WithMessage("Informe o Id da Ue para realizar a consulta");
        }
    }
}