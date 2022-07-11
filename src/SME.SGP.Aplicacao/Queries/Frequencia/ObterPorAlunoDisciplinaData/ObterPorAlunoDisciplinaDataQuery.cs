using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterPorAlunoDisciplinaDataQuery : IRequest<FrequenciaAluno>
    {
        public ObterPorAlunoDisciplinaDataQuery(string codigoAluno, string disciplinaId, DateTime dataAtual)
        {
            CodigoAluno = codigoAluno;
            DisciplinaId = disciplinaId;
            DataAtual = dataAtual;
        }

        public ObterPorAlunoDisciplinaDataQuery(string codigoAluno, string disciplinaId, DateTime dataAtual, string turmaId)
            : this(codigoAluno, disciplinaId, dataAtual)
        {
            TurmaId = turmaId;
        }

        public string CodigoAluno { get; set; }
        public string DisciplinaId { get; set; }
        public DateTime DataAtual { get; set; } 
        public string TurmaId { get; set; }
    }

    public class ObterPorAlunoDisciplinaDataQueryValidator : AbstractValidator<ObterPorAlunoDisciplinaDataQuery>
    {
        public ObterPorAlunoDisciplinaDataQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");
            RuleFor(a => a.DisciplinaId)
                .NotEmpty()
                .WithMessage("O id da disciplina deve ser informado.");
            RuleFor(a => a.DataAtual)
                .NotEmpty()
                .WithMessage("A data atual deve ser informado.");
        }
    }
}
