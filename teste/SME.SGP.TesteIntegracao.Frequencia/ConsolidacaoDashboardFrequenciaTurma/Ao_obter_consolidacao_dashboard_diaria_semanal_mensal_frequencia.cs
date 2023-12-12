using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
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
                    TipoConsolidacao = TipoConsolidadoFrequencia.Semanal,
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
                TipoConsolidacao = TipoConsolidadoFrequencia.Mensal,
                PeriodoInicio = inicioMesReferencia,
                PeriodoFim = fimMesReferencia,
            });

            var x = ObterTodos<ConsolidacaoFrequenciaTurma>();

            var dto = new FrequenciasConsolidadacaoPorTurmaEAnoDto()
            {
                AnoLetivo = dataReferencia.Year,
                DreId = ConstantesTeste.DRE_1_ID,
                UeId = ConstantesTeste.UE_1_ID,
                Modalidade = (int)Modalidade.Fundamental,
                AnoTurma = ConstantesTeste.TURMA_ANO_1
            };

            var useCase = ServiceProvider.GetService<IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase>();
            
            var retornoSemanal = await useCase.Executar(dto,
                inicioSemanaReferencia,inicioSemanaReferencia.AddDays(6),null, TipoConsolidadoFrequencia.Semanal);
            
            retornoSemanal.ShouldNotBeNull();
            retornoSemanal.DadosFrequenciaDashboard.ElementAt(0).Quantidade.ShouldBeEquivalentTo(10); //Mínimo
            retornoSemanal.DadosFrequenciaDashboard.ElementAt(1).Quantidade.ShouldBeEquivalentTo(20); //Maximo
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
                    TipoConsolidacao = TipoConsolidadoFrequencia.Semanal,
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
                TipoConsolidacao = TipoConsolidadoFrequencia.Mensal,
                PeriodoInicio = inicioMesReferencia,
                PeriodoFim = fimMesReferencia,
            });

            var dto = new FrequenciasConsolidadacaoPorTurmaEAnoDto()
            {
                AnoLetivo = dataReferencia.Year,
                DreId = ConstantesTeste.DRE_1_ID,
                UeId = ConstantesTeste.UE_1_ID,
                Modalidade = (int)Modalidade.Fundamental,
                AnoTurma = ConstantesTeste.TURMA_ANO_1
            };

            var useCase = ServiceProvider.GetService<IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase>();
            
            var retornoMensal = await useCase.Executar(dto,
                null,null,dataReferencia.Month, TipoConsolidadoFrequencia.Mensal);
            
            retornoMensal.ShouldNotBeNull();
            retornoMensal.DadosFrequenciaDashboard.ElementAt(0).Quantidade.ShouldBeEquivalentTo(25);
            retornoMensal.DadosFrequenciaDashboard.ElementAt(1).Quantidade.ShouldBeEquivalentTo(5);
        }
    }
}
