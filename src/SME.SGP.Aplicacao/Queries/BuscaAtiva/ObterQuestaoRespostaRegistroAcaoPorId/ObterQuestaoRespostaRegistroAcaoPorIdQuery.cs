using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoRespostaRegistroAcaoPorIdQuery : IRequest<IEnumerable<RespostaQuestaoRegistroAcaoBuscaAtivaDto>>
    {
        public ObterQuestaoRespostaRegistroAcaoPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterQuestaoRespostaRegistroAcaoPorIdQueryValidator : AbstractValidator<ObterQuestaoRespostaRegistroAcaoPorIdQuery>
    {
        public ObterQuestaoRespostaRegistroAcaoPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do registro de ação deve ser informado para a pesquisa");

        }
    }
}
