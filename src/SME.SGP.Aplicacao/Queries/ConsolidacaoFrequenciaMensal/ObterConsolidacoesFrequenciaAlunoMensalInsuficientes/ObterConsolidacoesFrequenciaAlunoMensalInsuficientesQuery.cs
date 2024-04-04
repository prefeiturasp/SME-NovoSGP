using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQuery : IRequest<IEnumerable<ConsolidacaoFreqAlunoMensalInsuficienteDto>>
    {
        public ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQuery(long ueId, int anoLetivo, int mes)
        {
            UeId = ueId;
            Mes = mes;
            AnoLetivo = anoLetivo;
        }

        public long UeId { get; set; }
        public int Mes { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQueryValidator : AbstractValidator<ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQuery>
    {
        public ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQueryValidator()
        {
            RuleFor(c => c.UeId)
                .GreaterThan(0)
                .WithMessage("O Id da Ue percisa ser informado para pesquisa de consolidações de frequência mensal aluno insuficientes");

            RuleFor(c => c.Mes)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(12)
                .WithMessage("Um mês válido precisa ser informado para pesquisa de consolidações de frequência mensal aluno insuficientes");

            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("Um ano letivo válido precisa ser informado para pesquisa de consolidações de frequência mensal aluno insuficientes");
        }
    }
}
