using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery(string[] codigosAlunos, string disciplinaId, DateTime dataAtual, string turmaCodigo = "")
        {
            CodigosAlunos = codigosAlunos;
            DisciplinaId = disciplinaId;
            DataAtual = dataAtual;
            TurmaCodigo = turmaCodigo;
        }

        public string[] CodigosAlunos { get; set; }
        public string DisciplinaId{ get; set; }
        public DateTime DataAtual { get; set; }
        public string TurmaCodigo { get; set; }
    }

    public class ObterFrequenciaPorListaDeAlunosDisciplinaDataQueryValidator : AbstractValidator<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>
    {
        public ObterFrequenciaPorListaDeAlunosDisciplinaDataQueryValidator()
        {
            RuleFor(x => x.CodigosAlunos).NotEmpty().WithMessage("Informe pelo menos um aluno para consultar a frequencia ");
            RuleFor(x => x.DisciplinaId).NotEmpty().WithMessage("Informe uma disciplina id para consultar a frequencia ");
            RuleFor(x => x.DisciplinaId).NotEmpty().WithMessage("Informe uma data  para consultar a frequencia ");
        }
    }
}