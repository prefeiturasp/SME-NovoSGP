using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQuery : IRequest<IEnumerable<TotalAulasNaoLancamNotaDto>>
    {
        public ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQuery(string disciplinaID, string codigoTurma, string codigoAluno)
        {
            DisciplinaId = disciplinaID;
            CodigoTurma = codigoTurma;
            CodigoAluno = codigoAluno;
        }
        public string DisciplinaId { get; set; }
        public string CodigoTurma { get; set; }
        public string CodigoAluno { get; set; }
    }

    public class ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQueryValidator : AbstractValidator<ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQuery>
    {
        public ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQueryValidator()
        {
            RuleFor(x => x.DisciplinaId).NotEmpty().WithMessage("O id da discplina deve ser informado para obter o total de aulas dos bimestres");
            RuleFor(x => x.CodigoTurma).NotEmpty().WithMessage("O codigo da turma deve ser informado para obter o total de aulas dos bimestres");
            RuleFor(x => x.CodigoAluno).NotEmpty().WithMessage("O id da discplina deve ser informado para obter o total de aulas dos bimestres");
        }
    }
}
