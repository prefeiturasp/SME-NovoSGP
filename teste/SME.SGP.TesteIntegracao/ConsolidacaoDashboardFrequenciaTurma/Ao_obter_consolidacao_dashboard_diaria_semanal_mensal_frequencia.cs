using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Nota;
using SME.SGP.TesteIntegracao.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConsolidacaoDashboardFrequenciaTurma
{
    public class Ao_obter_consolidacao_dashboard_diaria_semanal_mensal_frequencia : ConsolidacaoDashBoardBase
    {

        public Ao_obter_consolidacao_dashboard_diaria_semanal_mensal_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        [Fact(DisplayName = "Consolidação Dashboard - Deve obter a consolidação semanal da primeira semana")]
        public async Task Deve_obter_consolidacao_semanal_da_primeira_semana()
        {
            await CriarItensBasicos();

            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date.AddDays(-30);
            var inicioMesReferencia = new DateTime(dataReferencia.Year, dataReferencia.Month, 1);
            var fimMesReferencia = inicioMesReferencia.AddMonths(1).AddDays(-1);
            var inicioSemanaReferencia = inicioMesReferencia;
            
            while(inicioSemanaReferencia.DayOfWeek != DayOfWeek.Monday)
                inicioSemanaReferencia = inicioSemanaReferencia.AddDays(-1);

            var frequenciasSemanais = new[]
            { 
                new { Maximo = 20, Minimo = 10 },
                new { Maximo = 25, Minimo = 5 },
                new { Maximo = 28, Minimo = 2 },
                new { Maximo = 10, Minimo = 20 },
                new { Maximo = 22, Minimo = 8 },
                new { Maximo = 19, Minimo = 11 },
            }.ToList();

            var inicioSemana = inicioSemanaReferencia;
            int contagem = 0;
            
            while (inicioSemana < fimMesReferencia)
            {
                var fimSemanaReferencia = inicioSemana.AddDays(6);
                
                await InserirNaBase(new ConsolidacaoFrequenciaTurma()
                {
                    TurmaId = ConstantesTeste.TURMA_ID_1,
                    QuantidadeAbaixoMinimoFrequencia = frequenciasSemanais[contagem].Minimo,
                    QuantidadeAcimaMinimoFrequencia = frequenciasSemanais[contagem].Maximo,
                    TipoConsolidado = TipoConsolidadoFrequencia.Semanal,
                    PeriodoInicio = inicioSemana,
                    PeriodoFim = fimSemanaReferencia
                });
                inicioSemana = inicioSemana.AddDays(7);
                contagem++;
            }
            
            await InserirNaBase(new ConsolidacaoFrequenciaTurma()
            {
                TurmaId = ConstantesTeste.TURMA_ID_1,
                QuantidadeAbaixoMinimoFrequencia = 25,
                QuantidadeAcimaMinimoFrequencia = 5,
                TipoConsolidado = TipoConsolidadoFrequencia.Mensal,
                PeriodoInicio = inicioMesReferencia,
                PeriodoFim = fimMesReferencia
            });

            var x = ObterTodos<ConsolidacaoFrequenciaTurma>();

            var useCase = ServiceProvider.GetService<IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase>();
            
            var retornoSemanal = await useCase.Executar(dataReferencia.Year,
                ConstantesTeste.DRE_1_ID,ConstantesTeste.UE_1_ID,(int)Modalidade.Fundamental,ConstantesTeste.TURMA_ANO_1,
                inicioSemanaReferencia,inicioSemanaReferencia.AddDays(6),null, TipoConsolidadoFrequencia.Semanal);
            
            retornoSemanal.ShouldNotBeNull();
            retornoSemanal.FirstOrDefault().QuantidadeAcimaMinimoFrequencia.ShouldBeEquivalentTo(20);
            retornoSemanal.FirstOrDefault().QuantidadeAbaixoMinimoFrequencia.ShouldBeEquivalentTo(10);
        }
        
        [Fact(DisplayName = "Consolidação Dashboard - Deve obter a consolidação mensal")]
        public async Task Deve_obter_consolidacao_mensal()
        {
            await CriarItensBasicos();

            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date.AddDays(-30);
            var inicioMesReferencia = new DateTime(dataReferencia.Year, dataReferencia.Month, 1);
            var fimMesReferencia = inicioMesReferencia.AddMonths(1).AddDays(-1);
            var inicioSemanaReferencia = inicioMesReferencia;
            
            while(inicioSemanaReferencia.DayOfWeek != DayOfWeek.Monday)
                inicioSemanaReferencia = inicioSemanaReferencia.AddDays(-1);

            var frequenciasSemanais = new[]
            { 
                new { Maximo = 20, Minimo = 10 },
                new { Maximo = 25, Minimo = 5 },
                new { Maximo = 28, Minimo = 2 },
                new { Maximo = 10, Minimo = 20 },
                new { Maximo = 22, Minimo = 8 },
                new { Maximo = 19, Minimo = 11 },
            }.ToList();

            var inicioSemana = inicioSemanaReferencia;
            int contagem = 0;
            
            while (inicioSemana < fimMesReferencia)
            {
                var fimSemanaReferencia = inicioSemana.AddDays(6);
                
                await InserirNaBase(new ConsolidacaoFrequenciaTurma()
                {
                    TurmaId = ConstantesTeste.TURMA_ID_1,
                    QuantidadeAbaixoMinimoFrequencia = frequenciasSemanais[contagem].Minimo,
                    QuantidadeAcimaMinimoFrequencia = frequenciasSemanais[contagem].Maximo,
                    TipoConsolidado = TipoConsolidadoFrequencia.Semanal,
                    PeriodoInicio = inicioSemana,
                    PeriodoFim = fimSemanaReferencia,
                });
                inicioSemana = inicioSemana.AddDays(7);
                contagem++;
            }
            
            await InserirNaBase(new ConsolidacaoFrequenciaTurma()
            {
                TurmaId = ConstantesTeste.TURMA_ID_1,
                QuantidadeAbaixoMinimoFrequencia = 25,
                QuantidadeAcimaMinimoFrequencia = 5,
                TipoConsolidado = TipoConsolidadoFrequencia.Mensal,
                PeriodoInicio = inicioMesReferencia,
                PeriodoFim = fimMesReferencia
            });

            var useCase = ServiceProvider.GetService<IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase>();
            
            var retornoMensal = await useCase.Executar(dataReferencia.Year,
                ConstantesTeste.DRE_1_ID,ConstantesTeste.UE_1_ID,(int)Modalidade.Fundamental,ConstantesTeste.TURMA_ANO_1,
                null,null,dataReferencia.Month, TipoConsolidadoFrequencia.Mensal);
            
            retornoMensal.ShouldNotBeNull();
            retornoMensal.FirstOrDefault().QuantidadeAbaixoMinimoFrequencia.ShouldBeEquivalentTo(25);
            retornoMensal.FirstOrDefault().QuantidadeAcimaMinimoFrequencia.ShouldBeEquivalentTo(5);
        }
    }
}
