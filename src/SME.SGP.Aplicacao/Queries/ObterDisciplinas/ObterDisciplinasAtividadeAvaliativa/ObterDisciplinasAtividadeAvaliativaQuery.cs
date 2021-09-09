using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasAtividadeAvaliativaQuery : IRequest<IEnumerable<AtividadeAvaliativaDisciplina>>
    {
        public ObterDisciplinasAtividadeAvaliativaQuery(long avaliacao_id, bool ehRegencia)
        {
            Avaliacao_id = avaliacao_id;
            EhRegencia = ehRegencia;
        }

        public long Avaliacao_id { get; set; }
        public bool EhRegencia { get; set; }
    }

    public class ObterDisciplinasAtividadeAvaliativaQueryValidator : AbstractValidator<ObterDisciplinasAtividadeAvaliativaQuery>
    {
        public ObterDisciplinasAtividadeAvaliativaQueryValidator()
        {
            RuleFor(a => a.Avaliacao_id)
                .NotEmpty()
                .WithMessage("A avaliação deve ser informada");
        }
    }
}
