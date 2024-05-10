using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAIdConsolidadoExclusaoQuery : IRequest<IEnumerable<long>>
    {
        public ObterEncaminhamentosNAAPAIdConsolidadoExclusaoQuery(long ueId, int anoLetivo, SituacaoNAAPA[] situacoesIgnoradas)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
            SituacoesIgnoradas = situacoesIgnoradas;

        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public SituacaoNAAPA[] SituacoesIgnoradas { get; set; }
    }

    public class ObterEncaminhamentosNAAPAConsolidadoCargaExclusaoQueryValidator : AbstractValidator<ObterEncaminhamentosNAAPAIdConsolidadoExclusaoQuery>
    {
        public ObterEncaminhamentosNAAPAConsolidadoCargaExclusaoQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("Informe o Ano Letivo para realizar a consulta");
            RuleFor(x => x.UeId).GreaterThan(0).WithMessage("Informe o Id da Ue para realizar a consulta");
        }
    }
}