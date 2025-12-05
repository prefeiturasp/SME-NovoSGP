using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentosProfissionalAtendimentosNAAPAConsolidadoCargaQuery : IRequest<IEnumerable<AtendimentosProfissionalAtendimentoNAAPAConsolidadoDto>>
    {
        public ObterAtendimentosProfissionalAtendimentosNAAPAConsolidadoCargaQuery(long ueId, int mes, int anoLetivo)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
            Mes = mes;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
    }

    public class ObterAtendimentosProfissionalAtendimentosNAAPAConsolidadoCargaQueryValidator : AbstractValidator<ObterAtendimentosProfissionalAtendimentosNAAPAConsolidadoCargaQuery>
    {
        public ObterAtendimentosProfissionalAtendimentosNAAPAConsolidadoCargaQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("Informe o Ano Letivo para realizar a consulta");
            RuleFor(x => x.Mes).GreaterThan(0).WithMessage("Informe o Mes do Ano Letivo para realizar a consulta");
            RuleFor(x => x.UeId).GreaterThan(0).WithMessage("Informe o Id da Ue para realizar a consulta");
        }
    }
}