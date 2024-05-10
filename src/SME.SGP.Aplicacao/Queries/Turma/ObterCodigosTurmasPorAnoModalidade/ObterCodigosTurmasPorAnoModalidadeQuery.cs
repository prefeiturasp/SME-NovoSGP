using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosTurmasPorAnoModalidadeQuery : IRequest<IEnumerable<string>>
    {
        public ObterCodigosTurmasPorAnoModalidadeQuery(int anoLetivo, Modalidade[] modalidades, string turmaCodigo = "")
        {
            AnoLetivo = anoLetivo;
            Modalidades = modalidades;
            TurmaCodigo = turmaCodigo;
        }

        public int AnoLetivo { get; }
        public Modalidade[] Modalidades { get; }
        public string TurmaCodigo { get; }
    }

    public class ObterCodigosTurmasPorAnoModalidadeQueryValidator : AbstractValidator<ObterCodigosTurmasPorAnoModalidadeQuery>
    {
        public ObterCodigosTurmasPorAnoModalidadeQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta de turmas na modalidade.");

            RuleFor(a => a.Modalidades)
                .NotEmpty()
                .WithMessage("A modalidade da turma deve ser informada para consulta de turmas na modalidade.");
        }
    }
}
