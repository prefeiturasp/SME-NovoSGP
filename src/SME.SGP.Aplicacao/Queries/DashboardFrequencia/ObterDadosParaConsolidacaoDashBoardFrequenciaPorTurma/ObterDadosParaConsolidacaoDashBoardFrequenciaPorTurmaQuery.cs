using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery : IRequest<IEnumerable<DadosParaConsolidacaoDashBoardFrequenciaDto>>
    {
        public ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery(int anoLetivo, long turmaId, Modalidade modalidade, DateTime dataInicio, DateTime dataFim, DateTime? dataAula)
        {
            AnoLetivo = anoLetivo;
            TurmaId = turmaId;
            Modalidade = modalidade;
            DataInicio = dataInicio;
            DataFim = dataFim;
            DataAula = dataAula;
        }

        public int AnoLetivo { get; set; }
        public long TurmaId { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime? DataAula { get; set; }
    }
    
    public class ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQueryValidator : AbstractValidator<ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery>
    {
        public ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQueryValidator()
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
            
            RuleFor(a => a.DataInicio)
                .NotEmpty()
                .WithMessage("A data de início deve ser informada para obter a consolidacao dashboard frequência");
            
            RuleFor(a => a.DataFim)
                .NotEmpty()
                .WithMessage("A data fim deve ser informada para obter a consolidacao dashboard frequência");
        }
    }
}
