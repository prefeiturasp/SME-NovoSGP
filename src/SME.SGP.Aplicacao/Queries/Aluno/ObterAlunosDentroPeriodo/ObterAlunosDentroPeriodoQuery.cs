using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosDentroPeriodoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterAlunosDentroPeriodoQuery(string codigoTurma, (DateTime dataInicio, DateTime dataFim) periodo = default, bool consideraSomenteAtivos = false, bool consideraSomenteAtivosPeriodoFechamento = false, int tempoArmazenamentoCache = 720)
        {
            CodigoTurma = codigoTurma;
            Periodo = periodo;
            ConsideraSomenteAtivos = consideraSomenteAtivos;
            ConsideraSomenteAtivosPeriodoFechamento = consideraSomenteAtivosPeriodoFechamento;
            TempoArmazenamentoCache = tempoArmazenamentoCache;
        }

        public string CodigoTurma { get; set; }
        public (DateTime dataInicio, DateTime dataFim) Periodo { get; set; }
        public bool ConsideraSomenteAtivos { get; set; }
        public bool ConsideraSomenteAtivosPeriodoFechamento { get; set; }
        public int TempoArmazenamentoCache { get; }
    }

    public class ObterAlunosDentroPeriodoQueryValidator : AbstractValidator<ObterAlunosDentroPeriodoQuery>
    {
        public ObterAlunosDentroPeriodoQueryValidator()
        {
            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.Periodo.dataInicio)
                .GreaterThan(DateTime.MinValue)
                .When(c => !c.ConsideraSomenteAtivos)
                .WithMessage("A data de início do período deve ser informada.");

            RuleFor(c => c.Periodo.dataInicio)
                .GreaterThan(DateTime.MinValue)
                .When(c => !c.ConsideraSomenteAtivos)
                .WithMessage("A data de fim do período deve ser informada.");
        }
    }
}
