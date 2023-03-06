using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaBimestrePorCodigosAlunosIdsFechamentoQuery : IRequest<IEnumerable<FechamentoNotaDto>>
    {
        public ObterNotaBimestrePorCodigosAlunosIdsFechamentoQuery(string[] codigosAlunos, long[] fechamentosIds)
        {
            CodigosAlunos = codigosAlunos;
            FechamentoId = fechamentosIds;
        }

        public string[] CodigosAlunos { get; set; }
        public long[] FechamentoId { get; set; }
    }

    public class ObterNotaBimestrePorCodigosAlunosIdsFechamentoQueryValidator : AbstractValidator<ObterNotaBimestrePorCodigosAlunosIdsFechamentoQuery>
    {

        public ObterNotaBimestrePorCodigosAlunosIdsFechamentoQueryValidator()
        {
            RuleFor(x => x.CodigosAlunos).NotNull().WithMessage("É preciso informar os códigos dos alunos para consultar as notas conceito.");
            RuleFor(x => x.FechamentoId).NotNull().WithMessage("É preciso informar os id do fechamento para consultar as notas conceito.");
        }
    }
}
