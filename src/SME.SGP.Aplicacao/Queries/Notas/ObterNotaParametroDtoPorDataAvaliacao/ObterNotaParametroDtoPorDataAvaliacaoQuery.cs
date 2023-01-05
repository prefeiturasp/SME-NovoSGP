using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.Notas;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaParametroDtoPorDataAvaliacaoQuery : IRequest<NotaParametroDto>
    {
        public ObterNotaParametroDtoPorDataAvaliacaoQuery(DateTime dataAvaliacao)
        {
            DataAvaliacao = dataAvaliacao;
        }

        public DateTime DataAvaliacao { get; set; }
    }

    public class ObterNotaParametroDtoPorDataAvaliacaoQueryValidator : AbstractValidator<ObterNotaParametroDtoPorDataAvaliacaoQuery>
    {
        public ObterNotaParametroDtoPorDataAvaliacaoQueryValidator()
        {
            RuleFor(x => x.DataAvaliacao).NotEmpty().WithMessage("Informe a Data da Avaliação para Obter a Nota Parametro Por Data Avaliacao");
        }
    }
}
