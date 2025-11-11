using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoEducacaoIntegral;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterEducacaoIntegralUltimoAnoConsolidado;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoEducacaoIntegral;
using SME.SGP.Infra.Dtos.SituacaoAluno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarEducacaoIntegralPainelEducacionalUseCase :
        ConsolidacaoBaseUseCase,
        IConsolidarEducacaoIntegralPainelEducacionalUseCase
    {
        public ConsolidarEducacaoIntegralPainelEducacionalUseCase(IMediator mediator)
            : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoInicioConsolidacao = await DeterminarAnoInicioConsolidacao();
            var indicadores = new List<DadosParaConsolidarEducacaoIntegralDto>();

            for (int anoUtilizado = anoInicioConsolidacao; anoUtilizado <= DateTime.Now.Year; anoUtilizado++)
            {
                var indicadoresTemp = await ObterConsolidacaoAlunosTurmas(anoUtilizado);
                indicadores.AddRange(indicadoresTemp);
            }

            if (indicadores == null || !indicadores.Any())
                return false;

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoEducacaoIntegralCommand(anoInicioConsolidacao, indicadores));

            return true;
        }

        private async Task<int> DeterminarAnoInicioConsolidacao()
        {
            var ultimoAnoConsolidado = await mediator.Send(new ObterEducacaoIntegralUltimoAnoConsolidadoQuery());
            if (ultimoAnoConsolidado == 0)
                return PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE;
            if (ultimoAnoConsolidado == DateTime.Now.Year)
                return DateTime.Now.Year;
            return ultimoAnoConsolidado + 1;
        }

        private async Task<IEnumerable<DadosParaConsolidarEducacaoIntegralDto>> ObterConsolidacaoAlunosTurmas(int anoLetivo)
        {
            var indicadores = new List<DadosParaConsolidarEducacaoIntegralDto>();

            var listagemTurmas = await mediator.Send(new ObterTurmasPainelEducacionalQuery(anoLetivo));

            if (listagemTurmas == null || !listagemTurmas.Any())
                return indicadores;

            var turmasAgrupadasDre = listagemTurmas
                .Where(t => ModalidadesTurmasEducacaoIntegral.Contains((Modalidade)t.ModalidadeCodigo))
                .Where(t => !string.IsNullOrEmpty(t.Ano) && t.Ano.All(char.IsDigit))
                .GroupBy(t => t.CodigoDre)
                .Select(g => new DreTurmaPainelEducacionalEducacaoIntegralDto
                {
                    CodigoDre = g.Key,
                    Turmas = g.ToList(),
                })
                .ToList();

            foreach (var dreTurmas in turmasAgrupadasDre)
            {

                var turmasAgrupadas = await mediator.Send(new ObterAlunosSituacaoTurmasQuery(anoLetivo, (int)SituacaoMatriculaAluno.Ativo, dreTurmas.CodigoDre));

                await Task.Delay(1000);

                if (turmasAgrupadas == null || !turmasAgrupadas.Any())
                    continue;

                var turmasDistintas = ObterTurmasDistintas(turmasAgrupadas, dreTurmas.Turmas);
                indicadores.AddRange(AgruparConsolicacaoTurmas(turmasAgrupadas, turmasDistintas));
            }

            return indicadores;
        }

        private static IEnumerable<TurmaPainelEducacionalDto> ObterTurmasDistintas(IEnumerable<AlunosSituacaoTurmas> turmasAgrupadas, IEnumerable<TurmaPainelEducacionalDto> listagemTurma)
        {
            var turmasDistintas = turmasAgrupadas?.Select(x => x.CodigoTurma)?.Distinct()?.ToList();
            var turmas = listagemTurma?.Where(x => turmasDistintas.Contains(x.TurmaId))?.ToList();
            return turmas;
        }

        private static IEnumerable<DadosParaConsolidarEducacaoIntegralDto> AgruparConsolicacaoTurmas(
            IEnumerable<AlunosSituacaoTurmas> alunosTurmas,
            IEnumerable<TurmaPainelEducacionalDto> turmas)
        {
            var indicadores = turmas
                .Join(alunosTurmas,
                      t => t.TurmaId,
                      a => a.CodigoTurma,
                      (t, a) => new { t, a })
                .GroupBy(x => new { x.t.CodigoDre, x.t.CodigoUe, x.t.Ano, x.t.ModalidadeCodigo, x.t.AnoLetivo })
                .Select(g =>
                {
                    return new DadosParaConsolidarEducacaoIntegralDto
                    {
                        AnoLetivo = g.Key.AnoLetivo,
                        CodigoDre = g.Key.CodigoDre,
                        CodigoUe = g.Key.CodigoUe,
                        ModalidadeTurma = g.Key.ModalidadeCodigo,
                        Ano = g.Key.Ano,

                        QuantidadeAlunosIntegral = g.Where(t => t.t.TipoTurno == TipoTurnoEOL.Integral)
                                                    .Sum(t => t.a.QuantidadeAlunos),

                        QuantidadeAlunosParcial = g.Where(t => t.t.TipoTurno != TipoTurnoEOL.Integral)
                                                    .Sum(t => t.a.QuantidadeAlunos)
                    };
                })
                .ToList();

            return indicadores;
        }
    }
}
