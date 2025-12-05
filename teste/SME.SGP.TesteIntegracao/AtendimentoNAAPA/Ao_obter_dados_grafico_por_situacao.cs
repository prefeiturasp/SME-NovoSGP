using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    public class Ao_obter_dados_grafico_por_situacao : AtendimentoNAAPATesteBase
    {
        private readonly int QUANTIDADERASCUNHO = 3;
        private readonly int QUANTIDADEAGUARDANDOATENDIMENTO = 4;
        private readonly int QUANTIDADEEMATENDIMENTO = 2;
        private readonly int QUANTIDADEENCERRADO = 7;
        public Ao_obter_dados_grafico_por_situacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            
        }

        [Fact(DisplayName = "Obter Grafico por Situação")]
        public async Task Obter_dados_grafico_por_situacao()
        {
            await CriarDadosBasicos();
            var consolidados = ObterTodos<ConsolidadoEncaminhamentoNAAPA>();
            consolidados.Count().ShouldBeEquivalentTo(16);

            var ueId = 1;
            var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;
            var useCase = ServiceProvider.GetService<IObterQuantidadeEncaminhamentoPorSituacaoUseCase>();
            
            var retorno = await useCase.Executar(new FiltroGraficoEncaminhamentoPorSituacaoDto(){UeId= ueId,AnoLetivo= anoLetivo,DreId = null});
            retorno.Graficos.Count().ShouldBeEquivalentTo(4);
            retorno.Graficos.FirstOrDefault(x => x.Descricao == SituacaoNAAPA.Encerrado.Name()).Quantidade.ShouldBe<int>(70);
            retorno.Graficos.FirstOrDefault(x => x.Descricao == SituacaoNAAPA.EmAtendimento.Name()).Quantidade.ShouldBe<int>(20);
            retorno.Graficos.FirstOrDefault(x => x.Descricao == SituacaoNAAPA.AguardandoAtendimento.Name()).Quantidade.ShouldBe<int>(40);
            retorno.Graficos.FirstOrDefault(x => x.Descricao == SituacaoNAAPA.Rascunho.Name()).Quantidade.ShouldBe<int>(30);
        }

        [Fact(DisplayName = "Obter Grafico por Situação modalidade")]
        public async Task Obter_dados_grafico_por_situacao_modalidade()
        {
            await CriarDadosBasicos();
            var consolidados = ObterTodos<ConsolidadoEncaminhamentoNAAPA>();
            consolidados.Count().ShouldBe(16);

            var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;
            var useCase = ServiceProvider.GetService<IObterQuantidadeEncaminhamentoPorSituacaoUseCase>();
            var retorno = await useCase.Executar(new FiltroGraficoEncaminhamentoPorSituacaoDto() { UeId = UE_ID_1, AnoLetivo = anoLetivo, DreId = null, Modalidade = Modalidade.EJA });
            
            retorno.Graficos.Count().ShouldBe(1);
            retorno.Graficos.FirstOrDefault(x => x.Descricao == SituacaoNAAPA.Rascunho.Name()).Quantidade.ShouldBe(30); 
        }

        private async Task CriarDadosBasicos()
        {  
            await InserirNaBase(new Dre()
            {
                Id = 1,
                Nome = "Dre Teste",
                DataAtualizacao = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-2).Year, 1, 1),
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                DreId = 1,
                Nome = "Ue Teste",
                DataAtualizacao = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-2).Year, 1, 1),
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                DataAtualizacao = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1),
                Historica = false,
                TipoTurma = TipoTurma.Regular,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = 1
            });
            
            for (int i = 0; i < QUANTIDADEENCERRADO; i++)
            {
                await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA
                {
                    UeId = 1,
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    Quantidade = 10,
                    Situacao = SituacaoNAAPA.Encerrado,
                    Modalidade = Modalidade.EducacaoInfantil,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), 
                    CriadoPor = SISTEMA_NOME, 
                    CriadoRF = SISTEMA_CODIGO_RF,
                });
            }
            for (int i = 0; i < QUANTIDADEEMATENDIMENTO; i++)
            {
                await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA
                {
                    UeId = 1,
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    Quantidade = 10,
                    Situacao = SituacaoNAAPA.EmAtendimento,
                    Modalidade = Modalidade.Fundamental,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), 
                    CriadoPor = SISTEMA_NOME, 
                    CriadoRF = SISTEMA_CODIGO_RF,
                });
            }
            for (int i = 0; i < QUANTIDADEAGUARDANDOATENDIMENTO; i++)
            {
                await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA
                {
                    UeId = 1,
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    Quantidade = 10,
                    Situacao = SituacaoNAAPA.AguardandoAtendimento,
                    Modalidade = Modalidade.Medio,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), 
                    CriadoPor = SISTEMA_NOME, 
                    CriadoRF = SISTEMA_CODIGO_RF,
                });
            }

            for (int i = 0; i < QUANTIDADERASCUNHO; i++)
            {
                await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA
                {
                    UeId = 1,
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    Quantidade = 10,
                    Situacao = SituacaoNAAPA.Rascunho,
                    Modalidade = Modalidade.EJA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), 
                    CriadoPor = SISTEMA_NOME, 
                    CriadoRF = SISTEMA_CODIGO_RF,
                });
            }
        }
    }
}