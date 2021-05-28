using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAusentesPorTurmaNoPeriodoQuery : IRequest<IEnumerable<AlunoComponenteCurricularDto>>
    {
        public ObterAlunosAusentesPorTurmaNoPeriodoQuery(string turmaCodigo, DateTime dataInicio, string componenteCurricularId)
        {
            TurmaCodigo = turmaCodigo;
            DataReferencia = dataInicio;
            ComponenteCurricularId = componenteCurricularId;
        }

        public string TurmaCodigo { get; }
        public DateTime DataReferencia { get; }
        public string ComponenteCurricularId { get; set; }
    }

    public class ObterAlunosAusentesPorTurmaNoPeriodoQueryValidator : AbstractValidator<ObterAlunosAusentesPorTurmaNoPeriodoQuery>
    {
        public ObterAlunosAusentesPorTurmaNoPeriodoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta de alunos ausêntes no período");

            RuleFor(a => a.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de início deve ser informada para consulta de alunos ausêntes no período");
        }
    }
}
