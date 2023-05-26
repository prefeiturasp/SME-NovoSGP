using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_salvar_consolidado_encaminhamento_naapa :EncaminhamentoNAAPATesteBase
    {
        public Ao_salvar_consolidado_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodasUesIdsQuery, IEnumerable<long>>), typeof(ObterTodasUesIdsQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Deve Retornar True ao Executar Rotina de Consolidação")]
        public async Task Deve_retornar_true_ao_executar_rotina()
        {
            var useCase = ServiceProvider.GetService<IExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase>();
            await CriarDadosBasicos();
            var retornoUseCase = useCase.Executar(new MensagemRabbit());
        }

        [Fact(DisplayName = "Deve Retornar True ao Executar Rotina de Consolidação para Obter as UEs")]
        public async Task Deve_retornar_true_ao_obter_ues()
        {
            var useCase = ServiceProvider.GetService<IExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase>();
            await CriarDadosBasicos();
        }

        [Fact(DisplayName = "Deve Inserir um novo registo de Consolidação")]
        public async Task Deve_inserir_um_novo_registro_consolidado()
        {
            var useCase = ServiceProvider.GetService<IExecutarInserirConsolidadoEncaminhamentoNAAPAUseCase>();
            await CriarDadosBasicos();
        }

        [Fact(DisplayName = "Deve atualizar um registo de Consolidação sem inserir umm novo")]
        public async Task Deve_atualizar_um_registro_consolidado_sem_inserir_um_novo()
        {
            var useCase = ServiceProvider.GetService<IExecutarInserirConsolidadoEncaminhamentoNAAPAUseCase>();
            await CriarDadosBasicos();
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
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}