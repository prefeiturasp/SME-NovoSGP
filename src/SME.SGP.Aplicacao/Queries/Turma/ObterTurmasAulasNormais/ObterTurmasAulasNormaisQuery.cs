using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAulasNormaisQuery : IRequest<IEnumerable<TurmaDTO>>
    {
        public ObterTurmasAulasNormaisQuery(long ueId, int anoLetivo, int[] tiposTurma, int[] modalidades, int[] ignorarTiposCiclos)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
            TiposTurma = tiposTurma;
            Modalidades = modalidades;
            IgnorarTiposCiclos = ignorarTiposCiclos;
        }

        public long UeId { get; }
        public int AnoLetivo { get; }
        public int[] TiposTurma { get; }
        public int[] Modalidades { get; set; }
        public int[] IgnorarTiposCiclos { get; set; }
    }

    public class ObterTurmasAulasNormaisQueryValidator : AbstractValidator<ObterTurmasAulasNormaisQuery>
    {
        public ObterTurmasAulasNormaisQueryValidator()
        {
            RuleFor(t => t.UeId)
                .NotEmpty()
                .WithMessage("O Id da ue deve ser informado para consultar as turmas");

            RuleFor(t => t.AnoLetivo)
                .NotEmpty()
                .WithMessage("O Ano letivo deve ser informado para consultar as turmas");

            RuleFor(t => t.TiposTurma)
                .NotEmpty()
                .WithMessage("Os tipos de turmas deve ser informado para consultar as turmas");

            RuleFor(t => t.TiposTurma)
                .NotEmpty()
                .WithMessage("As modalidades deve ser informado para consultar as turmas");

            RuleFor(t => t.IgnorarTiposCiclos)
                .NotEmpty()
                .WithMessage("Os tipos de ciclos a serem ignorados deve ser informado para consultar as turmas");
        }
    }
}
