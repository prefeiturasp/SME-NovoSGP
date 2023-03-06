using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosTurmasPorAnoModalidadeUeQuery : IRequest<IEnumerable<long>>
    {
        public ObterCodigosTurmasPorAnoModalidadeUeQuery(int anoLetivo, Modalidade modalidade, long ueId)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            UeId = ueId;
        }

        public int AnoLetivo { get; }
        public Modalidade Modalidade { get; }
        public long UeId { get; }

        public class ObterCodigosTurmasPorAnoModalidadeUeQueryValidator : AbstractValidator<ObterCodigosTurmasPorAnoModalidadeUeQuery>
        {
            public ObterCodigosTurmasPorAnoModalidadeUeQueryValidator()
            {
                RuleFor(a => a.AnoLetivo)
                    .NotEmpty()
                    .WithMessage("O ano letivo deve ser informado para consulta de turmas na modalidade.");

                RuleFor(a => a.Modalidade)
                    .NotEmpty()
                    .WithMessage("A modalidade da turma deve ser informada para consulta de turmas na modalidade.");
                
                RuleFor(a => a.UeId)
                    .NotEmpty()
                    .WithMessage("A UE da turma deve ser informada para consulta de turmas na modalidade.");
            }
        }
    }
}
