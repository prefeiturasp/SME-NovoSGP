using System.Collections.Generic;
using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterProfissionaisAtendimentoAtendimentosNAAPAIdConsolidadoExclusaoQuery : IRequest<IEnumerable<long>>
    {
        public ObterProfissionaisAtendimentoAtendimentosNAAPAIdConsolidadoExclusaoQuery(long ueId, int mes, int anoLetivo, string[] rfsProfissionaisIgnorados)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
            Mes = mes;
            RfsProfissionaisIgnorados = rfsProfissionaisIgnorados;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public string[] RfsProfissionaisIgnorados { get; set; }
    }

    public class ObterProfissionaisAtendimentoAtendimentosNAAPAIdConsolidadoExclusaoQueryValidator : AbstractValidator<ObterProfissionaisAtendimentoAtendimentosNAAPAIdConsolidadoExclusaoQuery>
    {
        public ObterProfissionaisAtendimentoAtendimentosNAAPAIdConsolidadoExclusaoQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("Informe o Ano Letivo para realizar a consulta");
            RuleFor(x => x.Mes).GreaterThan(0).WithMessage("Informe o Mes do Ano Letivo para realizar a consulta");
            RuleFor(x => x.UeId).GreaterThan(0).WithMessage("Informe o Id da Ue para realizar a consulta");
        }
    }
}