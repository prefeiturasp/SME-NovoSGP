using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAConsolidadoCargaQuery:IRequest<IEnumerable<AtendimentosNAAPAConsolidadoDto>>
    {
        public ObterEncaminhamentosNAAPAConsolidadoCargaQuery(long ueId, int anoLetivo)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterEncaminhamentosNAAPAConsolidadoCargaQueryValidator : AbstractValidator<ObterEncaminhamentosNAAPAConsolidadoCargaQuery>
    {
        public ObterEncaminhamentosNAAPAConsolidadoCargaQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("Informe o Ano Letivo para realizar a consulta");
            RuleFor(x => x.UeId).GreaterThan(0).WithMessage("Informe o Id da Ue para realizar a consulta");
        }
    }
}