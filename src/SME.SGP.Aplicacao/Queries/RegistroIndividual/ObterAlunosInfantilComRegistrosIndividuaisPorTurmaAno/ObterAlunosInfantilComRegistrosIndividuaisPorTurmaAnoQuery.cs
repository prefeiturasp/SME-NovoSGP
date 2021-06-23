using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery : IRequest<IEnumerable<AlunoInfantilComRegistroIndividualDTO>>
    {
        public ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery(int anoLetivo, long turmaCodigo)
        {
            AnoLetivo = anoLetivo;
            TurmaCodigo = turmaCodigo;
        }

        public int AnoLetivo { get; set; }
        public long TurmaCodigo { get; set; }
    }

    public class ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQueryValidator : AbstractValidator<ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery>
    {
        public ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O Ano Letivo deve ser informado para consulta de turmas com registros individuais.");

            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("O Código da Turma deve ser informado para consulta de turmas com registros individuais.");


        }
    }
}
