using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterNovoEncaminhamentoNAAPAComTurmaPorId
{
    public class ObterNovoEncaminhamentoNAAPAComTurmaPorIdQuery : IRequest<EncaminhamentoEscolar>
    {
        public ObterNovoEncaminhamentoNAAPAComTurmaPorIdQuery(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; }
    }

    public class ObterNovoEncaminhamentoNAAPAComTurmaPorIdQueryValidator : AbstractValidator<ObterNovoEncaminhamentoNAAPAComTurmaPorIdQuery>
    {
        public ObterNovoEncaminhamentoNAAPAComTurmaPorIdQueryValidator()
        {
            RuleFor(a => a.EncaminhamentoId)
                .NotEmpty()
                .WithMessage("O id do encaminhamento NAAPA é necessário para pesquisa.");
        }
    }
}