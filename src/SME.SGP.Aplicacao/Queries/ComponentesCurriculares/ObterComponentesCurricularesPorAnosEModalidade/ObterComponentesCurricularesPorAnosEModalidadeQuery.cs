using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade
{
    public class ObterComponentesCurricularesPorAnosEModalidadeQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesPorAnosEModalidadeQuery(string codigoUe, Modalidade modalidade, string[] anosEscolares, int anoLetivo, bool turmaPrograma = false)
        {
            CodigoUe = codigoUe;
            Modalidade = modalidade;
            AnosEscolares = anosEscolares;
            AnoLetivo = anoLetivo;
            TurmaPrograma = turmaPrograma;
        }

        public string CodigoUe { get; }
        public Modalidade Modalidade { get; }
        public string[] AnosEscolares { get; }
        public int AnoLetivo { get; }
        public bool TurmaPrograma { get; }

    }


    public class ObterComponentesCurricularesPorAnosEModalidadeQueryValidator : AbstractValidator<ObterComponentesCurricularesPorAnosEModalidadeQuery>
    {
        public ObterComponentesCurricularesPorAnosEModalidadeQueryValidator()
        {

            RuleFor(c => c.Modalidade)
            .IsInEnum()
            .WithMessage("A modalidade deve ser informada.");

            RuleFor(c => c.AnosEscolares)
            .NotEmpty()
            .WithMessage("Os anos escolares devem ser informados.");

            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
