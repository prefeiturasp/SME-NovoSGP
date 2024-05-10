using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFrequenciaAlunoQuery : IRequest<AnotacaoFrequenciaAluno>
    {
        public ObterAnotacaoFrequenciaAlunoQuery(string codigoAluno, long aulaId)
        {
            CodigoAluno = codigoAluno;
            AulaId = aulaId;
        }

        public string CodigoAluno { get; set; }
        public long AulaId { get; set; }
    }

    public class ObterAnotacaoFrequenciaAlunoQueryValidator : AbstractValidator<ObterAnotacaoFrequenciaAlunoQuery>
    {
        public ObterAnotacaoFrequenciaAlunoQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta de suas anotações");

            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("Deve ser informado a aula para consulta de anotações do aluno");
        }
    }
}
