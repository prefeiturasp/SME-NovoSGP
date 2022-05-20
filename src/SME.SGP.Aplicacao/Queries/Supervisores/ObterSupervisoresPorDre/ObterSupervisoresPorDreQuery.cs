using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorDreQuery : IRequest<IEnumerable<SupervisorEscolasDreDto>>
    {
        public ObterSupervisoresPorDreQuery(string codigoDre, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            CodigoDre = codigoDre;
            TipoResponsavelAtribuicao = tipoResponsavelAtribuicao;
        }

        public string CodigoDre { get; set; }
        public TipoResponsavelAtribuicao TipoResponsavelAtribuicao { get; set; }
    }

    public class ObterSupervisoresPorDreQueryValidator : AbstractValidator<ObterSupervisoresPorDreQuery>
    {
        public ObterSupervisoresPorDreQueryValidator()
        {
            RuleFor(a => a.CodigoDre)
                .NotEmpty()
                .WithMessage("O código da Dre deve ser informado.");

            RuleFor(c => c.TipoResponsavelAtribuicao)
                .IsInEnum()
                .WithMessage("O tipo de responsável atribuição deve ser informado.");
        }
    }
}
