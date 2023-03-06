using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorDreAsyncQuery : IRequest<IEnumerable<SupervisorEscolasDreDto>>
    {
        public ObterSupervisoresPorDreAsyncQuery(string codigoDre, TipoResponsavelAtribuicao tipoResponsavel)
        {
            CodigoDre = codigoDre;
            TipoResponsavel = tipoResponsavel;
        }

        public string CodigoDre { get; set; }
        public TipoResponsavelAtribuicao TipoResponsavel { get; set; }
    }

    public class ObterSupervisoresPorDreAsyncQueryValidator : AbstractValidator<ObterSupervisoresPorDreAsyncQuery>
    {
        public ObterSupervisoresPorDreAsyncQueryValidator()
        {
            RuleFor(x => x.CodigoDre).NotEmpty()
                .WithMessage("O código DRE precisa ser informado.");
            RuleFor(x => x.TipoResponsavel).NotEmpty()
                .WithMessage("O Tipo Responsável precisa ser informado.");
        }
    }
}
