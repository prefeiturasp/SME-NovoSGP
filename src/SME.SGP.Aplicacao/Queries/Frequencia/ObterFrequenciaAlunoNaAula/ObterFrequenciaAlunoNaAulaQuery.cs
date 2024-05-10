using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunoNaAulaQuery : IRequest<IEnumerable<FrequenciaAlunoAulaDto>>
    {
        public string AlunoCodigo { get; set; }
        public long AulaId { get; set; }

        public ObterFrequenciaAlunoNaAulaQuery(string alunoCodigo, long aulaId)
        {
            AlunoCodigo = alunoCodigo;
            AulaId = aulaId;
        }
    }

    public class ObterFrequenciaAlunoNaAulaQueryValidator : AbstractValidator<ObterFrequenciaAlunoNaAulaQuery>
    {
        public ObterFrequenciaAlunoNaAulaQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("É preciso informar o id do aluno para consultar a frequência dele na aula");
            RuleFor(a => a.AulaId)
               .NotEmpty()
               .WithMessage("É preciso informar o id da aula para consultar a frequência dele na aula");
        }
    }
}
