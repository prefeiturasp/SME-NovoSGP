using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNota;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotasParaConsolidacao;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotaUltimoAnoConsolidado;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarNotasPainelEducacionalUseCase : AbstractUseCase, IConsolidarNotasPainelEducacionalUseCase
    {
        public ConsolidarNotasPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoInicioConsolidacao = await DeterminarAnoInicioConsolidacao();

            var notasConsolidadasDre = new List<PainelEducacionalConsolidacaoNota>();

            for (int anoUtilizado = anoInicioConsolidacao; anoUtilizado <= DateTime.Now.Year; anoUtilizado++)
            {
                var dadosBrutos = await mediator.Send(new ObterNotasParaConsolidacaoQuery(anoUtilizado));

                var notasConsolidadasDreTemp = ConsolidarNotasPorDre(dadosBrutos);
                notasConsolidadasDre.AddRange(notasConsolidadasDreTemp);
            }

            if (notasConsolidadasDre == null || !notasConsolidadasDre.Any())
                return false;

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoNotaCommand(notasConsolidadasDre));
            return true;
        }

        private IList<PainelEducacionalConsolidacaoNota> ConsolidarNotasPorDre(IEnumerable<PainelEducacionalConsolidacaoNotaDadosBrutos> dadosBrutos) =>
            dadosBrutos
               .GroupBy(db => new
               {
                   db.CodigoDre,
                   db.AnoLetivo,
                   db.Bimestre,
                   db.Modalidade,
                   db.TurmaAno
               })
               .Select(notasAgrupadas =>
               {
                   var (qtdAbaixoMediaPortugues, qtdAcimaMediaPortugues) =
                        CalcularQuantidadesPorComponente(notasAgrupadas, ComponentesCurricularesConstants.CODIGO_PORTUGUES);
                   var (qtdAbaixoMediaMatematica, qtdAcimaMediaMatematica) =
                        CalcularQuantidadesPorComponente(notasAgrupadas, ComponentesCurricularesConstants.CODIGO_MATEMATICA);
                   var (qtdAbaixoMediaCiencias, qtdAcimaMediaCiencias) =
                        CalcularQuantidadesPorComponente(notasAgrupadas, ComponentesCurricularesConstants.CODIGO_CIENCIAS);

                   return new PainelEducacionalConsolidacaoNota
                   {
                       AnoLetivo = notasAgrupadas.Key.AnoLetivo,
                       Bimestre = notasAgrupadas.Key.Bimestre,
                       Modalidade = notasAgrupadas.Key.Modalidade,
                       QuantidadeAbaixoMediaPortugues = qtdAbaixoMediaPortugues,
                       QuantidadeAcimaMediaPortugues = qtdAcimaMediaPortugues,
                       QuantidadeAbaixoMediaMatematica = qtdAbaixoMediaMatematica,
                       QuantidadeAcimaMediaMatematica = qtdAcimaMediaMatematica,
                       QuantidadeAbaixoMediaCiencias = qtdAbaixoMediaCiencias,
                       QuantidadeAcimaMediaCiencias = qtdAcimaMediaCiencias,
                       AnoTurma = notasAgrupadas.Key.TurmaAno,
                       CodigoDre = notasAgrupadas.Key.CodigoDre
                   };
               }).ToList();

        private async Task<int> DeterminarAnoInicioConsolidacao()
        {
            var ultimoAnoConsolidado = await mediator.Send(new ObterNotaUltimoAnoConsolidadoQuery());
            if (ultimoAnoConsolidado == 0)
                return PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE;
            if (ultimoAnoConsolidado == DateTime.Now.Year)
                return DateTime.Now.Year;
            return ultimoAnoConsolidado + 1;
        }

        private (int quantidadeAbaixo, int quantidadeAcima) CalcularQuantidadesPorComponente(
            IGrouping<dynamic, PainelEducacionalConsolidacaoNotaDadosBrutos> notasAgrupadas,
            int componenteCurricularId)
        {
            var notasDoComponente = notasAgrupadas.Where(n => n.IdComponenteCurricular == componenteCurricularId);

            var quantidadeAbaixo = notasDoComponente.Count(EstaAbaixoDaMedia);
            var quantidadeAcima = notasDoComponente.Count(EstaAcimaOuNaMedia);

            return (quantidadeAbaixo, quantidadeAcima);
        }

        private bool EstaAbaixoDaMedia(PainelEducacionalConsolidacaoNotaDadosBrutos nota) =>
            (!string.IsNullOrWhiteSpace(nota.ValorConceito) && !nota.ConceitoDeAprovado) ||
            (nota.Nota.HasValue && nota.Nota < nota.ValorMedioNota);

        private bool EstaAcimaOuNaMedia(PainelEducacionalConsolidacaoNotaDadosBrutos nota) =>
            (!string.IsNullOrWhiteSpace(nota.ValorConceito) && nota.ConceitoDeAprovado) ||
            (nota.Nota.HasValue && nota.Nota >= nota.ValorMedioNota);
    }
}
