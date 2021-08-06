using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQuery : IRequest<IEnumerable<DropdownTurmaRetornoDto>>
    {
        public ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQuery(int anoLetivo, string codigoUe, int[] modalidades, int semestre, string[] anos)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
            Modalidades = modalidades;
            Semestre = semestre;
            Anos = anos;
        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public int[] Modalidades { get; set; }
        public int Semestre { get; set; }
        public string[] Anos { get; set; }
    }
    public class ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQueryValidator : AbstractValidator<ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQuery>
    {
        public ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(x => x.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");            
        }
    }
}
