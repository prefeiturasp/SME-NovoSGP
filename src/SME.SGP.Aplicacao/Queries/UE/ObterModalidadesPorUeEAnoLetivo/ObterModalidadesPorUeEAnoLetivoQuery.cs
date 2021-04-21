using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorUeEAnoLetivoQuery : IRequest<IEnumerable<ModalidadeRetornoDto>>
    {
        public ObterModalidadesPorUeEAnoLetivoQuery(string codigoUe, int anoLetivo)
        {
            CodigoUe = codigoUe;
            AnoLetivo = anoLetivo;
        }

        public ObterModalidadesPorUeEAnoLetivoQuery(string codigoUe, int anoLetivo, IEnumerable<Modalidade> modadlidadesQueSeraoIgnoradas)
            : this(codigoUe, anoLetivo)
        {
            ModadlidadesQueSeraoIgnoradas = modadlidadesQueSeraoIgnoradas;
        }

        public string CodigoUe { get; }
        public int AnoLetivo { get; set; }
        public IEnumerable<Modalidade> ModadlidadesQueSeraoIgnoradas { get; set; }
    }

    public class ObterModalidadesPorUeEAnoLetivoQueryValidator : AbstractValidator<ObterModalidadesPorUeEAnoLetivoQuery>
    {
        public ObterModalidadesPorUeEAnoLetivoQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da ue deve ser informado.");

            RuleFor(c => c.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");
        }
    }
}