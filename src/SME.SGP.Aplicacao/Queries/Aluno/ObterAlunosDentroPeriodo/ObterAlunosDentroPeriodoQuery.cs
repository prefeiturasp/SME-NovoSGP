using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosDentroPeriodoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterAlunosDentroPeriodoQuery(string codigoTurma, (DateTime dataInicio, DateTime dataFim) periodo = default, bool consideraSomenteAtivos = false)
        {
            CodigoTurma = codigoTurma;
            Periodo = periodo;
            ConsideraSomenteAtivos = consideraSomenteAtivos;
        }

        public string CodigoTurma { get; set; }        
        public (DateTime dataInicio, DateTime dataFim) Periodo { get; set; }
        public bool ConsideraSomenteAtivos { get; set; }
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
