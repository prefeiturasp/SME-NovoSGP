using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterListaDeAtividadeAvaliativaDisciplinaPorIdAtividadeQuery : IRequest<IEnumerable<AtividadeAvaliativaDisciplina>>
    {
        public ObterListaDeAtividadeAvaliativaDisciplinaPorIdAtividadeQuery(long atividadeAvaliativaId)
        {
            AtividadeAvaliativaId = atividadeAvaliativaId;
        }

        public long AtividadeAvaliativaId { get; set; }
    }

    public class ObterListaDeAtividadeAvaliativaDisciplinaPorIdAtividadeQueryValidator : AbstractValidator<ObterListaDeAtividadeAvaliativaDisciplinaPorIdAtividadeQuery>
    {
        public ObterListaDeAtividadeAvaliativaDisciplinaPorIdAtividadeQueryValidator()
        {
            RuleFor(a => a.AtividadeAvaliativaId)
                .NotEmpty().WithMessage("É necessário informar o id da Ativididade Avaliativa para Obter Lista De Atividade por Id Atividade");
        }
    }
}