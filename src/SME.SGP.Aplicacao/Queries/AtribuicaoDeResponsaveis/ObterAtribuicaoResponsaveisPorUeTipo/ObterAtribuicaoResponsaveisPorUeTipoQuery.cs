using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using FluentValidation;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicaoResponsaveisPorUeTipoQuery : IRequest<IEnumerable<AtribuicaoResponsavelDto>>
    {
        public string CodigoUE;
        public TipoResponsavelAtribuicao Tipo;

        public ObterAtribuicaoResponsaveisPorUeTipoQuery(string codigoUE, TipoResponsavelAtribuicao tipo)
        {
            CodigoUE = codigoUE;
            Tipo = tipo;
        }
    }
    
    public class ObterFuncionariosPorUeTipoQueryValidator : AbstractValidator<ObterAtribuicaoResponsaveisPorUeTipoQuery>
    {
        public ObterFuncionariosPorUeTipoQueryValidator()
        {
            RuleFor(a => a.CodigoUE)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para pesquisa de funcionários atribuídos na UE");
        }
    }
}