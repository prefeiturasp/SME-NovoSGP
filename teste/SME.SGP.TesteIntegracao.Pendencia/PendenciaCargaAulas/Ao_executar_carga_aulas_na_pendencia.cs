using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PendenciaCargaAulas.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaCargaAulas
{
    public class Ao_executar_carga_aulas_na_pendencia : TesteBaseComuns
    {
        public Ao_executar_carga_aulas_na_pendencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodasUesIdsQuery, IEnumerable<long>>), typeof(ObterTodasUesIdsQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPendenciasParaInserirAulasEDiasQuery, IEnumerable<AulasDiasPendenciaDto>>), typeof(ObterPendenciasParaInserirAulasEDiasQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Deve Executar a Rotina de Carga de Aulas e Dias , sem informar o Ano Letivo")]
        public async Task Deve_retornar_true_sem_informar_o_ano_letivo()
        {
            var useCase = ServiceProvider.GetService<IObterQuantidadeAulaDiaPendenciaUseCase>();
            var retorno = await useCase.Executar(new MensagemRabbit(""));
            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Deve Executar a Rotina de Carga de Aulas e Dias ,informando o Ano Letivo")]
        public async Task Deve_retornar_true_informando_o_ano_letivo()
        {
            var useCase = ServiceProvider.GetService<IObterQuantidadeAulaDiaPendenciaUseCase>();
            var retorno = await useCase.Executar(new MensagemRabbit("2023"));
            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Deve Obter Pendencia Por Ue para Realizar a Carga, sem informar o ano")]
        public async Task Deve_Obter_Pendencia_Por_Ue_para_Realizar_a_Carga_sem_ano()
        {
            await CriarDadosBasicos();
            var useCase = ServiceProvider.GetService<IObterQuantidadeAulaDiaPendenciaPorUeUseCase>();
            var filtro = new ObterQuantidadeAulaDiaPendenciaDto {AnoLetivo = null, UeId = 1};
            var retornoUsecase = await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(filtro)));
            retornoUsecase.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Deve Obter Pendencia Por Ue para Realizar a Carga, informando o ano")]
        public async Task Deve_Obter_Pendencia_Por_Ue_para_Realizar_a_Carga_informando_ano()
        {
            await CriarDadosBasicos();
            var useCase = ServiceProvider.GetService<IObterQuantidadeAulaDiaPendenciaPorUeUseCase>();
            var filtro = new ObterQuantidadeAulaDiaPendenciaDto {AnoLetivo = 2023, UeId = 1};
            var retornoUsecase = await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(filtro)));
            retornoUsecase.ShouldBeTrue();
        }

        [Fact(DisplayName = "Deve Realizar a Carga Somente da Pendencia Informada")]
        public async Task Deve_Realizar_a_Carga_Somente_da_Pendencia_Informada()
        {
            await CriarDadosBasicos();
            var useCase = ServiceProvider.GetService<ICargaQuantidadeAulaDiaPendenciaUseCase>();
            
            var pendenciasExistentesNaBase = ObterTodos<Dominio.Pendencia>();
            pendenciasExistentesNaBase.Count(x => x.QuantidadeDias.EhNulo() && x.QuantidadeAulas.EhNulo()).ShouldBeEquivalentTo(2);
            
            var carga = new AulasDiasPendenciaDto {PendenciaId = 1, QuantidadeAulas = 1, QuantidadeDias = 1};
            var retornoUsecase = await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(carga)));
            retornoUsecase.ShouldBeTrue();
            
            var pendenciasNaBaseAposAtualizacao = ObterTodos<Dominio.Pendencia>();
            pendenciasNaBaseAposAtualizacao.Count(x => x.QuantidadeDias.EhNulo() && x.QuantidadeAulas.EhNulo()).ShouldBeEquivalentTo(1);
            pendenciasNaBaseAposAtualizacao.Count(x => x.QuantidadeDias == 1 && x.QuantidadeAulas == 1).ShouldBeEquivalentTo(1);
        }


        private async Task CriarDadosBasicos()
        {
            await InserirNaBase(new Dominio.Pendencia()
            {
                Titulo = "Pendencia de Teste 1",
                Descricao =  "Desc Pendencia de Teste 1",
                Situacao = SituacaoPendencia.Pendente,
                Tipo = TipoPendencia.AulasSemFrequenciaNaDataDoFechamento,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
            });
            await InserirNaBase(new Dominio.Pendencia()
            {
                Titulo = "Pendencia de Teste 2",
                Descricao =  "Desc Pendencia de Teste 2",
                Situacao = SituacaoPendencia.Pendente,
                Tipo = TipoPendencia.ResultadosFinaisAbaixoDaMedia,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
            });
        }
    }
}