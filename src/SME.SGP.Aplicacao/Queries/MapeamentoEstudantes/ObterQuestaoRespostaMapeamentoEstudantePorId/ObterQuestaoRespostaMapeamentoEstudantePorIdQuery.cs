using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoRespostaMapeamentoEstudantePorIdQuery : IRequest<IEnumerable<RespostaQuestaoMapeamentoEstudanteDto>>
    {
        public ObterQuestaoRespostaMapeamentoEstudantePorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterQuestaoRespostaMapeamentoEstudantePorIdQueryValidator : AbstractValidator<ObterQuestaoRespostaMapeamentoEstudantePorIdQuery>
    {
        public ObterQuestaoRespostaMapeamentoEstudantePorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do mapeamento de estudante deve ser informado para a pesquisa");

        }
    }
}
