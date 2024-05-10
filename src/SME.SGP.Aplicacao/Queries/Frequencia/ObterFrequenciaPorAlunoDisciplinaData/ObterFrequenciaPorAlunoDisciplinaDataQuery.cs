using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPorAlunoDisciplinaDataQuery : IRequest<FrequenciaAluno>
    {
        public ObterFrequenciaPorAlunoDisciplinaDataQuery(string codigoAluno, string disciplinaId, DateTime dataAtual, string turmaCodigo)
        {
            CodigoAluno = codigoAluno;
            DisciplinaId = disciplinaId;
            DataAtual = dataAtual;
            TurmaCodigo = turmaCodigo;
        }

        public string CodigoAluno { get; set; }
        public string DisciplinaId { get; set; }
        public DateTime DataAtual { get; set; }
        public string TurmaCodigo { get; set; }
    }

    public class ObterFrequenciaPorAlunoDisciplinaDataQueryValidator : AbstractValidator<ObterFrequenciaPorAlunoDisciplinaDataQuery>
    {
        public ObterFrequenciaPorAlunoDisciplinaDataQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("Necessário informar o código do aluno para consulta de suas frequências");

            RuleFor(a => a.DisciplinaId)
                .NotEmpty()
                .WithMessage("Necessário informar a disciplina");

            RuleFor(a => a.DataAtual)
                .NotEmpty()
                .WithMessage("Necessário informar a data atual");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar a turma para consulta de frequências do aluno");
        }
    }
}
