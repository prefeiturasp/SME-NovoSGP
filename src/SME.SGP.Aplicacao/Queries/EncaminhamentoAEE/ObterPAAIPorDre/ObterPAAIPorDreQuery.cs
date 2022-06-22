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
    public class ObterPAAIPorDreQuery : IRequest<IEnumerable<SupervisorEscolasDreDto>>
    {
        public ObterPAAIPorDreQuery(string codigoDre, TipoResponsavelAtribuicao tipoResponsavel)
        {
            TipoResponsavel = tipoResponsavel;
            CodigoDre = codigoDre;
        }
        public TipoResponsavelAtribuicao TipoResponsavel { get; set; }
        public string CodigoDre { get; set; }
    }

    public class ObterPAAIPorDreQueryValidator : AbstractValidator<ObterPAAIPorDreQuery>
    {
        public ObterPAAIPorDreQueryValidator()
        {
            RuleFor(x => x.TipoResponsavel).NotEmpty().WithMessage("O Código do perfil deve ser informado para obter reponsáveis por perfil e dre");
            RuleFor(x => x.CodigoDre).NotEmpty().WithMessage("O Código do perfil deve ser informado para obter reponsáveis por perfil e dre");
        }
    }
}

