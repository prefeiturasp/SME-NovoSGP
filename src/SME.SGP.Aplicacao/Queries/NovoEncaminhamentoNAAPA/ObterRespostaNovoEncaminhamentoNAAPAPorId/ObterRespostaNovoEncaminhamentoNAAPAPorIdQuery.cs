using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterRespostaNovoEncaminhamentoNAAPAPorId
{
    public class ObterRespostaNovoEncaminhamentoNAAPAPorIdQuery : IRequest<RespostaEncaminhamentoEscolar>
    {
        public ObterRespostaNovoEncaminhamentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterRespostaNovoEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterRespostaNovoEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterRespostaNovoEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do encaminhamento naapa deve ser informado para a pesquisa");

        }
    }
}