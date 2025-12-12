using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterNovoEncaminhamentoNAAPAPorId
{
    public class ObterNovoEncaminhamentoNAAPAPorIdQuery : IRequest<EncaminhamentoEscolar>
    {
        public ObterNovoEncaminhamentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterNovoEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterNovoEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterNovoEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do encaminhamento NAAPA deve ser informado para a pesquisa.");
        }
    }
}