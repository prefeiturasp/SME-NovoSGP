using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterPorAlunoDisciplinaDataQuery : IRequest<FrequenciaAluno>
    {
        public ObterPorAlunoDisciplinaDataQuery(string codigoAluno, string[] disciplinasId, DateTime dataAtual, string professor = null)
        {
            CodigoAluno = codigoAluno;
            DisciplinasId = disciplinasId;
            DataAtual = dataAtual;
            Professor = professor;
        }

        public ObterPorAlunoDisciplinaDataQuery(string codigoAluno, string[] disciplinaId, DateTime dataAtual, string turmaId, string professor = null)
            : this(codigoAluno, disciplinaId, dataAtual, professor)
        {
            TurmaId = turmaId;
        }



        public string CodigoAluno { get; set; }
        public string[] DisciplinasId { get; set; }
        public DateTime DataAtual { get; set; } 
        public string TurmaId { get; set; }
        public string Professor { get; set; }
    }

    public class ObterPorAlunoDisciplinaDataQueryValidator : AbstractValidator<ObterPorAlunoDisciplinaDataQuery>
    {
        public ObterPorAlunoDisciplinaDataQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("É preciso informar o código do aluno para consultar a frequência.");
            RuleFor(a => a.DisciplinasId)
                .NotEmpty()
                .WithMessage("É preciso informar o id da disciplina para consultar a frequência.");
            RuleFor(a => a.DataAtual)
                .NotEmpty()
                .WithMessage("É preciso informar a data atual para consultar a frequência.");
        }
    }
}
