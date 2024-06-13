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
    public class ObterRespostaMapeamentoEstudantePorIdQuery : IRequest<RespostaMapeamentoEstudante>
    {
        public ObterRespostaMapeamentoEstudantePorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterRespostaMapeamentoEstudantePorIdQueryValidator : AbstractValidator<ObterRespostaMapeamentoEstudantePorIdQuery>
    {
        public ObterRespostaMapeamentoEstudantePorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do mapeamento de estudante deve ser informado para a pesquisa");

        }
    }
}
