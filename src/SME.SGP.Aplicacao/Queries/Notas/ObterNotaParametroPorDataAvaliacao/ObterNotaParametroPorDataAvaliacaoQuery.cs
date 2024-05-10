using System;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaParametroPorDataAvaliacaoQuery : IRequest<NotaParametro>
    {
        public ObterNotaParametroPorDataAvaliacaoQuery(DateTime dataAvaliacao)
        {
            DataAvaliacao = dataAvaliacao;
        }

        public DateTime DataAvaliacao { get; set; }
    }

    public class ObterNotaParametroPorDataAvaliacaoQueryValidator : AbstractValidator<ObterNotaParametroPorDataAvaliacaoQuery>
    {
        public ObterNotaParametroPorDataAvaliacaoQueryValidator()
        {
            RuleFor(x => x.DataAvaliacao).NotEmpty().WithMessage("Informe a Data da Avaliação para Obter a Nota Parametro Por Data Avaliacao");
        }
    }
}