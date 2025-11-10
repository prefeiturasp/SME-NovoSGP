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
            var notasConsolidadasUe = new List<PainelEducacionalConsolidacaoNotaUe>();

            for (int anoUtilizado = anoInicioConsolidacao; anoUtilizado <= DateTime.Now.Year; anoUtilizado++)
            {
                var dadosBrutos = await mediator.Send(new ObterNotasParaConsolidacaoQuery(anoUtilizado));

                var notasConsolidadasDreTemp = ConsolidarNotasPorDre(dadosBrutos);
                var notasConsolidadasUeTemp = ConsolidarNotasPorUe(dadosBrutos);

                notasConsolidadasDre.AddRange(notasConsolidadasDreTemp);
                notasConsolidadasUe.AddRange(notasConsolidadasUeTemp);
            }

            if (notasConsolidadasDre == null || !notasConsolidadasDre.Any())
                return false;

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoNotaCommand(notasConsolidadasDre, notasConsolidadasUe));
            return true;
        }

        private IList<PainelEducacionalConsolidacaoNota> ConsolidarNotasPorDre(IEnumerable<PainelEducacionalConsolidacaoNotaDadosBrutos> dadosBrutos) =>
            dadosBrutos.Where(db => char.IsDigit(db.AnoTurma))
               .GroupBy(db => new
               {
                   db.CodigoDre,
                   db.AnoLetivo,
                   db.Bimestre,
                   db.Modalidade,
                   db.AnoTurma
               })
               .Select(notasAgrupadas =>
               {
                   var consolidado = new PainelEducacionalConsolidacaoNota
                   {
                       AnoLetivo = notasAgrupadas.Key.AnoLetivo,
                       Bimestre = notasAgrupadas.Key.Bimestre,
                       Modalidade = notasAgrupadas.Key.Modalidade,
                       CodigoDre = notasAgrupadas.Key.CodigoDre,
                       AnoTurma = notasAgrupadas.Key.AnoTurma
                   };

                   var quantidades = CalcularQuantidadesPorComponente(notasAgrupadas);
                   consolidado.QuantidadeAbaixoMediaPortugues = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_PORTUGUES).Abaixo;
                   consolidado.QuantidadeAcimaMediaPortugues = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_PORTUGUES).Acima;
                   consolidado.QuantidadeAbaixoMediaMatematica = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_MATEMATICA).Abaixo;
                   consolidado.QuantidadeAcimaMediaMatematica = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_MATEMATICA).Acima;
                   consolidado.QuantidadeAbaixoMediaCiencias = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_CIENCIAS).Abaixo;
                   consolidado.QuantidadeAcimaMediaCiencias = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_CIENCIAS).Acima;

                   return consolidado;
               }).ToList();

        private IList<PainelEducacionalConsolidacaoNotaUe> ConsolidarNotasPorUe(IEnumerable<PainelEducacionalConsolidacaoNotaDadosBrutos> dadosBrutos) =>
            dadosBrutos
               .GroupBy(db => new
               {
                   db.CodigoDre,
                   db.CodigoUe,
                   db.AnoLetivo,
                   db.Bimestre,
                   db.Modalidade,
                   db.TurmaNome
               })
               .Select(notasAgrupadas =>
               {
                   var consolidado = new PainelEducacionalConsolidacaoNotaUe
                   {
                       AnoLetivo = notasAgrupadas.Key.AnoLetivo,
                       Bimestre = notasAgrupadas.Key.Bimestre,
                       Modalidade = notasAgrupadas.Key.Modalidade,
                       CodigoDre = notasAgrupadas.Key.CodigoDre,
                       CodigoUe = notasAgrupadas.Key.CodigoUe,
                       SerieTurma = notasAgrupadas.Key.TurmaNome
                   };

                   var quantidades = CalcularQuantidadesPorComponente(notasAgrupadas);
                   consolidado.QuantidadeAbaixoMediaPortugues = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_PORTUGUES).Abaixo;
                   consolidado.QuantidadeAcimaMediaPortugues = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_PORTUGUES).Acima;
                   consolidado.QuantidadeAbaixoMediaMatematica = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_MATEMATICA).Abaixo;
                   consolidado.QuantidadeAcimaMediaMatematica = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_MATEMATICA).Acima;
                   consolidado.QuantidadeAbaixoMediaCiencias = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_CIENCIAS).Abaixo;
                   consolidado.QuantidadeAcimaMediaCiencias = quantidades.GetValueOrDefault(ComponentesCurricularesConstants.CODIGO_CIENCIAS).Acima;

                   return consolidado;
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

        private Dictionary<long, (int Abaixo, int Acima)> CalcularQuantidadesPorComponente(
            IGrouping<dynamic, PainelEducacionalConsolidacaoNotaDadosBrutos> notasAgrupadas)
        {
            var quantidades = new Dictionary<long, (int Abaixo, int Acima)>();

            foreach (var componenteCurricularId in PainelEducacionalConstants.ComponentesCurricularesConsolidacaoNotas)
            {
                var notasDoComponente = notasAgrupadas.Where(n => n.IdComponenteCurricular == componenteCurricularId);

                var quantidadeAbaixo = notasDoComponente.Count(EstaAbaixoDaMedia);
                var quantidadeAcima = notasDoComponente.Count(EstaAcimaOuNaMedia);

                quantidades[componenteCurricularId] = (quantidadeAbaixo, quantidadeAcima);
            }

            return quantidades;
        }

        private bool EstaAbaixoDaMedia(PainelEducacionalConsolidacaoNotaDadosBrutos nota) =>
            (!string.IsNullOrWhiteSpace(nota.ValorConceito) && !nota.ConceitoDeAprovado) ||
            (nota.Nota.HasValue && nota.Nota < nota.ValorMedioNota);

        private bool EstaAcimaOuNaMedia(PainelEducacionalConsolidacaoNotaDadosBrutos nota) =>
            (!string.IsNullOrWhiteSpace(nota.ValorConceito) && nota.ConceitoDeAprovado) ||
            (nota.Nota.HasValue && nota.Nota >= nota.ValorMedioNota);
    }
}
