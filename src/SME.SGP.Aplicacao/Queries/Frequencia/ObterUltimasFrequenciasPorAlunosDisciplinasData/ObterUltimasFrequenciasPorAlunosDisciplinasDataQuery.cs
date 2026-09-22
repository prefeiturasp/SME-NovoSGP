using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimasFrequenciasPorAlunosDisciplinasDataQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterUltimasFrequenciasPorAlunosDisciplinasDataQuery(string[] codigosAlunos, string[] disciplinasId,
            DateTime dataAtual, string turmaCodigo = null, string professor = null)
        {
            CodigosAlunos = codigosAlunos;
            DisciplinasId = disciplinasId;
            DataAtual = dataAtual;
            TurmaCodigo = turmaCodigo;
            Professor = professor;
        }

        public string[] CodigosAlunos { get; }
        public string[] DisciplinasId { get; }
        public DateTime DataAtual { get; }
        public string TurmaCodigo { get; }
        public string Professor { get; }
    }

    public class ObterUltimasFrequenciasPorAlunosDisciplinasDataQueryValidator : AbstractValidator<ObterUltimasFrequenciasPorAlunosDisciplinasDataQuery>
    {
        public ObterUltimasFrequenciasPorAlunosDisciplinasDataQueryValidator()
        {
            RuleForEach(q => q.CodigosAlunos).NotEmpty();
            RuleFor(q => q.DisciplinasId).NotEmpty();
            RuleFor(q => q.DataAtual).NotEmpty();
        }
    }
}
