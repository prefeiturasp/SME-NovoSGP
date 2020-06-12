using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using System;

namespace SME.SGP.Aplicacao.Queries.Relatorios.Comuns.ObterCorrelacaoRelatorio
{
    public class ObterCorrelacaoRelatorioQuery : IRequest<RelatorioCorrelacao>
    {
        public ObterCorrelacaoRelatorioQuery(Guid codigoCorrelacao)
        {
            CodigoCorrelacao = codigoCorrelacao;
        }

        public Guid CodigoCorrelacao { get; set; }
    }

    public class ObterCorrelacaoRelatorioQueryValidator : AbstractValidator<ObterCorrelacaoRelatorioQuery>
    {
        public ObterCorrelacaoRelatorioQueryValidator()
        {
            RuleFor(c => c.CodigoCorrelacao)
                .NotEmpty()
                .WithMessage("O código de correlação é obrigatório.");
        }
    }
}
