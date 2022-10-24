using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPAAIPorUeTipoResponsavelAtribuicaoQuery : IRequest<IEnumerable<SupervisorDto>>
    {
        public ObterPAAIPorUeTipoResponsavelAtribuicaoQuery(string codigoUe, TipoResponsavelAtribuicao tipoResponsavel)
        {
            TipoResponsavel = tipoResponsavel;
            CodigoUe = codigoUe;
        }
        public TipoResponsavelAtribuicao TipoResponsavel { get; set; }
        public string CodigoUe { get; set; }
    }

    public class ObterPAAIPorUeTipoResponsavelAtribuicaoQueryValidator : AbstractValidator<ObterPAAIPorUeTipoResponsavelAtribuicaoQuery>
    {
        public ObterPAAIPorUeTipoResponsavelAtribuicaoQueryValidator()
        {
            RuleFor(x => x.TipoResponsavel).NotEmpty().WithMessage("O Código do responsável deve ser informado para serem atribuídos");
            RuleFor(x => x.CodigoUe).NotEmpty().WithMessage("O Código da UE deve ser informado para obter reponsáveis para serem atribuídos");
        }
    }
}

