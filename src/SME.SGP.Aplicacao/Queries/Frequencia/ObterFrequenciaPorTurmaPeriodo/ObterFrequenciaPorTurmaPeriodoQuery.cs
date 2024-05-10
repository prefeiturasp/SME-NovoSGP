using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPorTurmaPeriodoQuery : IRequest<IEnumerable<FrequenciaAlunoDto>>
    {
        public ObterFrequenciaPorTurmaPeriodoQuery(string turmaCodigo, DateTime dataInicio, DateTime dataFim)
        {
            TurmaCodigo = turmaCodigo;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public string TurmaCodigo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }

    public class ObterFrequenciaPorTurmaPeriodoQueryValidator : AbstractValidator<ObterFrequenciaPorTurmaPeriodoQuery>
    {
        public ObterFrequenciaPorTurmaPeriodoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta da frequência por turma e período");
            RuleFor(a => a.DataInicio)
                .NotEmpty()
                .WithMessage("A data de início deve ser informado para consulta da frequência por turma e período");
            RuleFor(a => a.DataFim)
                .NotEmpty()
                .WithMessage("A data de fim deve ser informado para consulta da frequência por turma e período");
        }
    }
}
