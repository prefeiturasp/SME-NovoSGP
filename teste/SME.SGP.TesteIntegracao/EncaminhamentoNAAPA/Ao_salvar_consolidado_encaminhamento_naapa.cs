using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Minio;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_salvar_consolidado_encaminhamento_naapa : EncaminhamentoNAAPATesteBase
    {
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
            var useCase = ServiceProvider.GetService<IExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase>();
            var retorno = await useCase.Executar(new MensagemRabbit(""));
            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Deve Retornar True ao Executar Rotina de Consolidação informando o ano")]
        public async Task Deve_retornar_true_ao_executar_rotina_com_ano()
        {
            var useCase = ServiceProvider.GetService<IExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase>();
            await CriarDadosBasicos();
            var retornoUseCase = await useCase.Executar(new MensagemRabbit("2023"));
            retornoUseCase.ShouldBeTrue();
        }

        [Fact(DisplayName = "Deve Retornar True ao Executar Rotina de Consolidação para Obter as UEs")]
        public async Task Deve_retornar_true_ao_obter_ues()
        {
            var useCase = ServiceProvider.GetService<IExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase>();
            await CriarDadosBasicos();
            var ueId = 1;
            var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;
            var retornoUseCase = await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(new FiltroBuscarUesConsolidadoEncaminhamentoNAAPADto(ueId, anoLetivo))));
            retornoUseCase.ShouldBeTrue();
        }

        [Fact(DisplayName = "Deve Inserir um novo registo de Consolidação")]
        public async Task Deve_inserir_um_novo_registro_consolidado()
        {
            var useCase = ServiceProvider.GetService<IExecutarInserirConsolidadoEncaminhamentoNAAPAUseCase>();
            await CriarDadosBasicos();

            var obterTodos = ObterTodos<ConsolidadoEncaminhamentoNAAPA>();
            obterTodos.Count.ShouldBeEquivalentTo(1);

            var ueId = 1;
            var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;
            var retorno = await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(new ConsolidadoEncaminhamentoNAAPA(anoLetivo, ueId, 10, SituacaoNAAPA.Rascunho))));
            retorno.ShouldBeTrue();

            var obterTodosAposUseCaseExecutar = ObterTodos<ConsolidadoEncaminhamentoNAAPA>();
            obterTodosAposUseCaseExecutar.Count.ShouldBeEquivalentTo(2);
            obterTodosAposUseCaseExecutar.Count(x => x.Situacao == SituacaoNAAPA.Rascunho).ShouldBeEquivalentTo(1);
            obterTodosAposUseCaseExecutar.Count(x => x.Situacao == SituacaoNAAPA.Encerrado).ShouldBeEquivalentTo(1);
        }

        [Fact(DisplayName = "Deve atualizar um registo de Consolidação sem inserir umm novo")]
        public async Task Deve_atualizar_um_registro_consolidado_sem_inserir_um_novo()
        {
            await CriarDadosBasicos();
            var useCase = ServiceProvider.GetService<IExecutarInserirConsolidadoEncaminhamentoNAAPAUseCase>();
            var obterTodos = ObterTodos<ConsolidadoEncaminhamentoNAAPA>();
            obterTodos.Count.ShouldBeEquivalentTo(1);
            obterTodos.FirstOrDefault().Quantidade.ShouldBe<long>(10);

            var ueId = 1;
            var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;
            var retorno = await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(new ConsolidadoEncaminhamentoNAAPA(anoLetivo, ueId, 12, SituacaoNAAPA.Encerrado))));
            retorno.ShouldBeTrue();

            var obterTodosAposUseCaseExecutar = ObterTodos<ConsolidadoEncaminhamentoNAAPA>();
            obterTodosAposUseCaseExecutar.Count.ShouldBeEquivalentTo(1);
            obterTodosAposUseCaseExecutar.Count(x => x.Situacao == SituacaoNAAPA.Encerrado).ShouldBeEquivalentTo(1);
            obterTodosAposUseCaseExecutar.FirstOrDefault().Quantidade.ShouldBe<long>(12);

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
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Quantidade = 10,
                Situacao = SituacaoNAAPA.Encerrado,
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