using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulasPorTipoQuery : IRequest<IEnumerable<Aula>>
    {
        public ObterPendenciasAulasPorTipoQuery(TipoPendencia tipoPendenciaAula, string tabelaReferencia, long[] modalidades)
        {
            TipoPendenciaAula = tipoPendenciaAula;
            TabelaReferencia = tabelaReferencia;
            Modalidades = modalidades;
        }

        public TipoPendencia TipoPendenciaAula { get; set; }
        public string TabelaReferencia { get; set; }
        public long[] Modalidades { get; set; }
    }

    public class ObterPendenciasAulasPorTipoQueryValidator : AbstractValidator<ObterPendenciasAulasPorTipoQuery>
    {
        public ObterPendenciasAulasPorTipoQueryValidator()
        {
            RuleFor(c => c.TipoPendenciaAula)
            .NotEmpty()
            .WithMessage("O tipo de pendência deve ser informado para consulta de pendência na aula.");

            RuleFor(c => c.TabelaReferencia)
            .NotEmpty()
            .WithMessage("A tabela de referencia deve ser informada para consulta de pendência na aula.");

            RuleFor(c => c.Modalidades)
            .NotEmpty()
            .WithMessage("As modalidades deve ser informadas para consulta de pendência na aula.");

        }
    }
}
