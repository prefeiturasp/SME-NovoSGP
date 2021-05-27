using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAusentesPorTurmaNoPeriodoQuery : IRequest<IEnumerable<AlunoComponenteCurricularDto>>
    {
        public ObterAlunosAusentesPorTurmaNoPeriodoQuery(string turmaCodigo, DateTime dataInicio, DateTime dataFim, string componenteCurricularId)
        {
            TurmaCodigo = turmaCodigo;
            DataInicio = dataInicio;
            DataFim = dataFim;
            ComponenteCurricularId = componenteCurricularId;
        }

        public string TurmaCodigo { get; }
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
        public string ComponenteCurricularId { get; set; }
    }

    public class ObterAlunosAusentesPorTurmaNoPeriodoQueryValidator : AbstractValidator<ObterAlunosAusentesPorTurmaNoPeriodoQuery>
    {
        public ObterAlunosAusentesPorTurmaNoPeriodoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta de alunos ausêntes no período");

            RuleFor(a => a.DataInicio)
                .NotEmpty()
                .WithMessage("A data de início deve ser informada para consulta de alunos ausêntes no período");

            RuleFor(a => a.DataFim)
                .NotEmpty()
                .WithMessage("A data de fim deve ser informada para consulta de alunos ausêntes no período");
        }
    }
}
