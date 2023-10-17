using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUeModalidadeAnoQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesPorUeModalidadeAnoQuery(string codigoUe, Modalidade modalidade, int anoLetivo, string[] anosEscolares, bool ignorarInfoPedagogicasSgp = false)
        {
            CodigoUe = codigoUe;
            Modalidade = modalidade;
            AnosEscolares = anosEscolares;
            AnoLetivo = anoLetivo;
            IgnorarInfoPedagogicasSgp = ignorarInfoPedagogicasSgp;
        }

        public string CodigoUe { get; }
        public Modalidade Modalidade { get; }
        public string[] AnosEscolares { get; }
        public int AnoLetivo { get; }
        public bool IgnorarInfoPedagogicasSgp { get; }
    }
    
    public class ObterComponentesCurricularesPorUeModalidadeAnoQueryValidator : AbstractValidator<ObterComponentesCurricularesPorUeModalidadeAnoQuery>
    {
        public ObterComponentesCurricularesPorUeModalidadeAnoQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da ue deve ser informado para a busca de componentes curriculares.");
            
            RuleFor(c => c.Modalidade)
            .IsInEnum()
            .WithMessage("A modalidade deve ser informada para a busca de componentes curriculares.");

            RuleFor(c => c.AnosEscolares)
            .NotEmpty()
            .WithMessage("Os anos escolares devem ser informados para a busca de componentes curriculares.");

            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O ano letivo devem ser informada para a busca de componentes curriculares.");
        }
    }
}
