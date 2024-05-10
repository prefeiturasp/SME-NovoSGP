using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFrequenciaAlunoPorIdQuery : IRequest<AnotacaoFrequenciaAluno>
    {
        public ObterAnotacaoFrequenciaAlunoPorIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }

    public class ObterAnotacaoFrequenciaAlunoPorIdQueryValidator: AbstractValidator<ObterAnotacaoFrequenciaAlunoPorIdQuery>
    {
        public ObterAnotacaoFrequenciaAlunoPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O id deve ser informado para consulta da anotação de frequência do aluno.");

        }
    }
}
