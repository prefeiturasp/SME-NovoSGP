using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA;
using SME.SGP.TesteIntegracao.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    public class Ao_salvar_consolidado_encaminhamento_naapa : AtendimentoNAAPATesteBase
    {
        private int ANO_ATUAL = DateTimeExtension.HorarioBrasilia().Year;
        public Ao_salvar_consolidado_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodasUesIdsQuery, IEnumerable<long>>), typeof(ObterTodasUesIdsQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Deve Retornar True ao Executar Rotina de Consolidação sem informar o ano")]
        public async Task Deve_retornar_true_ao_executar_rotina()
        {
            await CriarDadosBasicos();
            var useCase = ServiceProvider.GetService<IExecutarCargaConsolidadoAtendimentoNAAPAUseCase>();
            var retorno = await useCase.Executar(new MensagemRabbit(""));
            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Deve Retornar True ao Executar Rotina de Consolidação informando o ano")]
        public async Task Deve_retornar_true_ao_executar_rotina_com_ano()
        {
            var useCase = ServiceProvider.GetService<IExecutarCargaConsolidadoAtendimentoNAAPAUseCase>();
            await CriarDadosBasicos();
            var retornoUseCase = await useCase.Executar(new MensagemRabbit(ANO_ATUAL.ToString()));
            retornoUseCase.ShouldBeTrue();
        }

        [Fact(DisplayName = "Deve Retornar True ao Executar Rotina de Consolidação para Obter as UEs")]
        public async Task Deve_retornar_true_ao_obter_ues()
        {
            var useCase = ServiceProvider.GetService<IExecutarBuscarUesConsolidadoAtendimentoNAAPAUseCase>();
            await CriarDadosBasicos();
            var ueId = 1;
            var anoLetivo = ANO_ATUAL;
            var retornoUseCase = await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(new FiltroBuscarUesConsolidadoAtendimentoNAAPADto(ueId, anoLetivo))));
            retornoUseCase.ShouldBeTrue();
        }

        [Fact(DisplayName = "Deve Inserir um novo registo de Consolidação")]
        public async Task Deve_inserir_um_novo_registro_consolidado()
        {
            var useCase = ServiceProvider.GetService<IExecutarInserirConsolidadoAtendimentoNAAPAUseCase>();
            await CriarDadosBasicos();

            var obterTodos = ObterTodos<ConsolidadoEncaminhamentoNAAPA>();
            obterTodos.Count.ShouldBeEquivalentTo(1);

            var ueId = 1;
            var anoLetivo = ANO_ATUAL;
            var retorno = await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(new ConsolidadoEncaminhamentoNAAPA(anoLetivo, ueId, 10, SituacaoNAAPA.Rascunho, Modalidade.Medio))));
            retorno.ShouldBeTrue();

            var obterTodosAposUseCaseExecutar = ObterTodos<ConsolidadoEncaminhamentoNAAPA>();
            obterTodosAposUseCaseExecutar.Count.ShouldBeEquivalentTo(2);
            var rascunho = obterTodosAposUseCaseExecutar.Find(x => x.Situacao == SituacaoNAAPA.Rascunho);
            rascunho.ShouldNotBeNull();
            rascunho.Quantidade.ShouldBe(10);
            rascunho.Modalidade.ShouldBe(Modalidade.Medio);
            var encerrado = obterTodosAposUseCaseExecutar.Find(x => x.Situacao == SituacaoNAAPA.Encerrado);
            encerrado.ShouldNotBeNull();
            encerrado.Quantidade.ShouldBe(10);
            encerrado.Modalidade.ShouldBe(Modalidade.Fundamental);
        }

        [Fact(DisplayName = "Deve atualizar um registo de Consolidação sem inserir umm novo")]
        public async Task Deve_atualizar_um_registro_consolidado_sem_inserir_um_novo()
        {
            await CriarDadosBasicos();
            var useCase = ServiceProvider.GetService<IExecutarInserirConsolidadoAtendimentoNAAPAUseCase>();
            var obterTodos = ObterTodos<ConsolidadoEncaminhamentoNAAPA>();
            obterTodos.Count.ShouldBeEquivalentTo(1);
            obterTodos.FirstOrDefault().Quantidade.ShouldBe<long>(10);

            var ueId = 1;
            var anoLetivo = ANO_ATUAL;
            var retorno = await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(new ConsolidadoEncaminhamentoNAAPA(anoLetivo, ueId, 12, SituacaoNAAPA.Encerrado, Modalidade.Fundamental))));
            retorno.ShouldBeTrue();

            var obterTodosAposUseCaseExecutar = ObterTodos<ConsolidadoEncaminhamentoNAAPA>();
            obterTodosAposUseCaseExecutar.Count.ShouldBeEquivalentTo(1);
            var encerrado = obterTodosAposUseCaseExecutar.Find(x => x.Situacao == SituacaoNAAPA.Encerrado);
            encerrado.ShouldNotBeNull();
            encerrado.Quantidade.ShouldBe(12);
            encerrado.Modalidade.ShouldBe(Modalidade.Fundamental);
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
                DataAtualizacao = new DateTime(ANO_ATUAL, 1, 1),
                Historica = false,
                TipoTurma = TipoTurma.Regular,
                AnoLetivo = ANO_ATUAL,
                UeId = 1
            });
            for (int i = 0; i < 5; i++)
            {
                await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
                {
                    TurmaId = TURMA_ID_1,
                    AlunoCodigo = i.ToString(),
                    Situacao = SituacaoNAAPA.Rascunho,
                    AlunoNome = $"Nome do aluno ${i}",
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });
            }

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA
            {
                UeId = 1,
                AnoLetivo = ANO_ATUAL,
                Quantidade = 10,
                Situacao = SituacaoNAAPA.Encerrado,
                Modalidade = Modalidade.Fundamental,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), 
                CriadoPor = SISTEMA_NOME, 
                CriadoRF = SISTEMA_CODIGO_RF,
            });
            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "GerarConsolidadoAtendimentoNAAPA",
                Tipo = TipoParametroSistema.GerarConsolidadoAtendimentoNAAPA,
                Descricao = "Controle de geração do consolidado Atendimento Encaminhamento NAAPA",
                Valor = string.Empty,
                Ano = DateTime.Now.Year,
                Ativo = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "GerarConsolidadoEncaminhamentoNAAPA",
                Tipo = TipoParametroSistema.GerarConsolidadoEncaminhamentoNAAPA,
                Descricao = "Controle de geração do consolidado Encaminhamento NAAPA",
                Valor = string.Empty,
                Ano = DateTime.Now.Year,
                Ativo = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });


            var parametros = ObterTodos<ParametrosSistema>();


        }
    }
}