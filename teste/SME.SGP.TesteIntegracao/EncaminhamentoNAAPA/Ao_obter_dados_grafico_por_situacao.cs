using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_obter_dados_grafico_por_situacao : EncaminhamentoNAAPATesteBase
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
            
            var retorno = await useCase.Executar(new FiltroGraficoEncaminhamentoPorSituacaoDto(){UeId= ueId,AnoLetivo= anoLetivo});
            retorno.Graficos.Count().ShouldBeEquivalentTo(4);
            retorno.Graficos.FirstOrDefault(x => x.Descricao == SituacaoNAAPA.Encerrado.Name()).Quantidade.ShouldBe<int>(70);
            retorno.Graficos.FirstOrDefault(x => x.Descricao == SituacaoNAAPA.EmAtendimento.Name()).Quantidade.ShouldBe<int>(20);
            retorno.Graficos.FirstOrDefault(x => x.Descricao == SituacaoNAAPA.AguardandoAtendimento.Name()).Quantidade.ShouldBe<int>(40);
            retorno.Graficos.FirstOrDefault(x => x.Descricao == SituacaoNAAPA.Rascunho.Name()).Quantidade.ShouldBe<int>(30);
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

            await InserirNaBase(new Turma()
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
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), 
                    CriadoPor = SISTEMA_NOME, 
                    CriadoRF = SISTEMA_CODIGO_RF,
                });
            }
        }
    }
}