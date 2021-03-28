using MediatR;
﻿using FluentValidation;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoTurmaPorIdQuery : IRequest<AcompanhamentoTurma>
    {
        public ObterAcompanhamentoTurmaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterAcompanhamentoTurmaPorIdQueryValidator : AbstractValidator<ObterAcompanhamentoTurmaPorIdQuery>
    {
        public ObterAcompanhamentoTurmaPorIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id do acompanhamento da turma deve ser informado para consulta.");
        }
    }
}
