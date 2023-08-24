using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQuery : IRequest<ConsolidacaoDashBoardFrequencia>
    {
        public ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQuery(long turmaId, DateTime dataAula, Modalidade modalidadeCodigo, int anoLetivo, long dreId, long ueId, TipoPeriodoDashboardFrequencia tipo)
        {
            AnoLetivo = anoLetivo;
            TurmaId = turmaId;
            Modalidade = modalidadeCodigo;
            DataAula = dataAula;
            DreId = dreId;
            UeId = ueId;
            Tipo = tipo;
        }

        public int AnoLetivo { get; set; }
        public long TurmaId { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime DataAula { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public TipoPeriodoDashboardFrequencia Tipo { get; set; }
    }
    
    public class ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQueryValidator : AbstractValidator<ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQuery>
    {
        public ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para obter a consolidacao dashboard frequência");

            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O identificador da turma deve ser informado para obter a consolidacao dashboard frequência");
            
            RuleFor(a => a.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada para obter a consolidacao dashboard frequência");

            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada para obter a consolidacao dashboard frequência");
            
            RuleFor(a => a.DreId)
                .NotEmpty()
                .WithMessage("O identificador da Dre deve ser informado para obter a consolidacao dashboard frequência");
            
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O identificador da Ue deve ser informado para obter a consolidacao dashboard frequência");
            
            RuleFor(a => a.Tipo)
                .NotEmpty()
                .WithMessage("O tipo da consolidação deve ser informado para obter a consolidacao dashboard frequência");
            
            
        }
    }
}
