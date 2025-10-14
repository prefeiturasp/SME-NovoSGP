using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.AnotacaoFrequenciaAluno;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFrequenciaAlunoPorPeriodoQuery : IRequest<IEnumerable<AnotacaoAlunoAulaPorPeriodoDto>>
    {
        public ObterAnotacaoFrequenciaAlunoPorPeriodoQuery(string codigoAluno, DateTime dataInicio, DateTime dataFim)
        {
            CodigoAluno = codigoAluno;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public string CodigoAluno { get; }
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
    }

    public class ObterAnotacaoFrequenciaAlunoPorPeriodoQueryValidator : AbstractValidator<ObterAnotacaoFrequenciaAlunoPorPeriodoQuery>
    {
        public ObterAnotacaoFrequenciaAlunoPorPeriodoQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta de suas anotações");

            RuleFor(x => x.DataInicio)
                .NotEmpty().WithMessage("Data de início é obrigatória");

            RuleFor(x => x.DataFim)
                .NotEmpty().WithMessage("Data de fim é obrigatória")
                .Must((model, dataFim) => dataFim > model.DataInicio)
                .WithMessage("A data de fim deve ser maior que a data de início");
        }
    }
}
