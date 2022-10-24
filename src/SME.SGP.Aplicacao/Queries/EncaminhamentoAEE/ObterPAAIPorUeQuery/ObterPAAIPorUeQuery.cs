using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPAAIPorUeQuery : IRequest<IEnumerable<SupervisorDto>>
    {
        public ObterPAAIPorUeQuery(string codigoUe, TipoResponsavelAtribuicao tipoResponsavel)
        {
            TipoResponsavel = tipoResponsavel;
            CodigoUe = codigoUe;
        }
        public TipoResponsavelAtribuicao TipoResponsavel { get; set; }
        public string CodigoUe { get; set; }
    }

    public class ObterPAAIPorUeQueryValidator : AbstractValidator<ObterPAAIPorUeQuery>
    {
        public ObterPAAIPorUeQueryValidator()
        {
            RuleFor(x => x.TipoResponsavel).NotEmpty().WithMessage("O Código do responsável deve ser informado para serem atribuídos");
            RuleFor(x => x.CodigoUe).NotEmpty().WithMessage("O Código da UE deve ser informado para obter reponsáveis para serem atribuídos");
        }
    }
}

