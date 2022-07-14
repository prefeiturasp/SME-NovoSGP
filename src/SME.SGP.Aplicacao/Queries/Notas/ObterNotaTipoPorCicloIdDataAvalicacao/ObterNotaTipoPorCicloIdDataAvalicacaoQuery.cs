using System;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaTipoPorCicloIdDataAvalicacaoQuery : IRequest<NotaTipoValor>
    {
        public ObterNotaTipoPorCicloIdDataAvalicacaoQuery(long cicloId, DateTime dataAvalicao)
        {
            CicloId = cicloId;
            DataAvalicao = dataAvalicao;
        }

        public DateTime DataAvalicao { get; set; }

        public long CicloId { get; set; }
    }

    public class ObterNotaTipoPorCicloIdDataAvalicacaoQueryValidator : AbstractValidator<
            ObterNotaTipoPorCicloIdDataAvalicacaoQuery>
    {
        public ObterNotaTipoPorCicloIdDataAvalicacaoQueryValidator()
        {
            RuleFor(x => x.CicloId).NotEmpty().WithMessage("Informe um Ciclo Id para Obter a Nota Tipo Por Ciclo e Data Avalicação");
            RuleFor(x => x.DataAvalicao).NotEmpty().WithMessage("Informe uma Data de Avaliação para Obter a Nota Tipo Por Ciclo e Data Avalicação");
        }
    }
}