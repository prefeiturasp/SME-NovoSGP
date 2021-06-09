using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendarioPorAnoLetivoModalidadeQuery : IRequest<IEnumerable<TipoCalendario>>
    {
        public ObterTiposCalendarioPorAnoLetivoModalidadeQuery(int anoLetivo, Modalidade[] modalidades)
        {
            AnoLetivo = anoLetivo;
            Modalidades = modalidades;
        }

        public int AnoLetivo { get; }

        public Modalidade[] Modalidades { get; }
    }

    public class ObterTiposCalendarioPorAnoLetivoModalidadeoQueryValidator : AbstractValidator<ObterTiposCalendarioPorAnoLetivoModalidadeQuery>
    {
        public ObterTiposCalendarioPorAnoLetivoModalidadeoQueryValidator()
        {
            RuleFor(x => x.Modalidades).NotNull().WithMessage("As modalidades são obrigatórias");
            RuleFor(x => x.AnoLetivo).NotNull().WithMessage("O ano letivo obrigatório");
        }

    }
}
