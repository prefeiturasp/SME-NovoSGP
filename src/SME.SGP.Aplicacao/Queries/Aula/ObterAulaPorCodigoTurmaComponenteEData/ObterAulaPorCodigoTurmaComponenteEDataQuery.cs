using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPorCodigoTurmaComponenteEDataQuery : IRequest<long>
    {
        public ObterAulaPorCodigoTurmaComponenteEDataQuery(string turmaId, string componenteCurricularId, DateTime dataCriacao)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            DataCriacao = dataCriacao;
        }

        public string TurmaId { get; }
        public string ComponenteCurricularId { get; }
        public DateTime DataCriacao { get; }
    }

    public class ObterAulaPorCodigoTurmaComponenteEDataQueryValidator : AbstractValidator<ObterAulaPorCodigoTurmaComponenteEDataQuery>
    {
        public ObterAulaPorCodigoTurmaComponenteEDataQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta de aula");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado para consulta de aula");

            RuleFor(a => a.DataCriacao)
                .NotEmpty()
                .WithMessage("A data da aula deve set informada para consulta de aula");
        }
    }
}
