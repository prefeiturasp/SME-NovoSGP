using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery(string[] codigosAlunos, string[] disciplinaId, long periodoEscolarId, string turmaCodigo = "", string professor = null)
        {
            CodigosAlunos = codigosAlunos;
            DisciplinaId = disciplinaId;
            PeriodoEscolarId = periodoEscolarId;
            TurmaCodigo = turmaCodigo;
            Professor = professor;
        }

        public string[] CodigosAlunos { get; set; }
        public string[] DisciplinaId{ get; set; }
        public long PeriodoEscolarId { get; set; }
        public string TurmaCodigo { get; set; }
        public string Professor { get; set; }
    }

    public class ObterFrequenciaPorListaDeAlunosDisciplinaDataQueryValidator : AbstractValidator<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>
    {
        public ObterFrequenciaPorListaDeAlunosDisciplinaDataQueryValidator()
        {
            RuleFor(x => x.CodigosAlunos).NotEmpty().WithMessage("Informe pelo menos um aluno para consultar a frequencia ");
            RuleFor(x => x.DisciplinaId).NotEmpty().WithMessage("Informe uma disciplina id para consultar a frequencia ");
            RuleFor(x => x.PeriodoEscolarId).NotEmpty().WithMessage("Informe um período escolar para consultar a frequencia ");
        }
    }
}