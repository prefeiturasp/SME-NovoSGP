using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Ocorrencia.Base;
using SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dto;
using Xunit;
using OcorrenciaObj = SME.SGP.Dominio.Ocorrencia;
using Nest;

namespace SME.SGP.TesteIntegracao.Ocorrencia
{
    public class Ao_filtrar_ocorrencia : OcorrenciaTesteBase
    {
        public Ao_filtrar_ocorrencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorUeQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorUeQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPerfilAtualQuery, Guid>), typeof(ObterPerfilAtualQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUEsPorDREQuery, IEnumerable<AbrangenciaUeRetorno>>), typeof(ObterUEsPorDREQueryHandlerFiltroTodasUesFake), ServiceLifetime.Scoped));
        }

        [Theory(DisplayName = "Ocorrência - Ao filtrar ocorrência por titulo")]
        [InlineData("Ocorrência", true)]
        [InlineData("teste", false)]
        public async Task Ao_filtra_ocorrencia_por_titulo(string titulo, bool resultado)
        {
            await CriarDadosBasicos();
            await CriarOcorrencia();

            var dtoFiltro = new FiltroOcorrenciaListagemDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Titulo = titulo
            };
            var useCase = ListarOcorrenciasUseCase();
            var retorno = await useCase.Executar(dtoFiltro);
            retorno.ShouldNotBeNull();
            retorno.Items.Any().ShouldBe(resultado);
        }

        [Theory(DisplayName = "Ocorrência - Ao filtrar por turma")]
        [InlineData(TURMA_ID_1, true)]
        [InlineData(50, false)]
        public async Task Ao_filtra_ocorrencia_por_modalidade_turma(long turmaId, bool resultado)
        {
            await CriarDadosBasicos();
            await CriarOcorrencia();

            var dtoFiltro = new FiltroOcorrenciaListagemDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                TurmaId = turmaId
            };
            var useCase = ListarOcorrenciasUseCase();
            var retorno = await useCase.Executar(dtoFiltro);
            retorno.ShouldNotBeNull();
            retorno.Items.Any().ShouldBe(resultado);
        }

        [Theory(DisplayName = "Ocorrência - Ao filtrar por nome aluno")]
        [InlineData("Aluno 1", true)]
        [InlineData("teste", false)]
        public async Task Ao_filtra_ocorrencia_por_nome_aluno(string nome, bool resultado)
        {
            await CriarDadosBasicos();
            await CriarOcorrencia();

            var dtoFiltro = new FiltroOcorrenciaListagemDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                AlunoNome = nome
            };
            var useCase = ListarOcorrenciasUseCase();
            var retorno = await useCase.Executar(dtoFiltro);
            retorno.ShouldNotBeNull();
            retorno.Items.Any().ShouldBe(resultado);
        }

        [Theory(DisplayName = "Ocorrência - Ao filtrar por nome servidor")]
        [InlineData("3333333", true)]
        [InlineData("teste", false)]
        public async Task Ao_filtra_ocorrencia_por_nome_servidor(string nome, bool resultado)
        {
            await CriarDadosBasicos();
            await CriarOcorrencia();

            var dtoFiltro = new FiltroOcorrenciaListagemDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                ServidorNome = nome
            };
            var useCase = ListarOcorrenciasUseCase();
            var retorno = await useCase.Executar(dtoFiltro);
            retorno.ShouldNotBeNull();
            retorno.Items.Any().ShouldBe(resultado);
        }

        [Theory(DisplayName = "Ocorrência - Ao filtrar por tipo ocorrência")]
        [InlineData(1, true)]
        [InlineData(20, false)]
        public async Task Ao_filtra_ocorrencia_por_tipo_ocorrencia(int tipoOcorrencia, bool resultado)
        {
            await CriarDadosBasicos();
            await CriarOcorrencia();

            var dtoFiltro = new FiltroOcorrenciaListagemDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                TipoOcorrencia = tipoOcorrencia
            };
            var useCase = ListarOcorrenciasUseCase();
            var retorno = await useCase.Executar(dtoFiltro);
            retorno.ShouldNotBeNull();
            retorno.Items.Any().ShouldBe(resultado);
        }

        [Fact(DisplayName = "Ocorrência - Ao filtrar por data ocorrência")]
        public async Task Ao_filtra_ocorrencia_por_data_ocorrencia()
        {
            await CriarDadosBasicos();
            await CriarOcorrencia();

            var dtoFiltro = new FiltroOcorrenciaListagemDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                DataOcorrenciaInicio = DateTimeExtension.HorarioBrasilia().Date
            };
            var useCase = ListarOcorrenciasUseCase();
            var retorno = await useCase.Executar(dtoFiltro);
            retorno.ShouldNotBeNull();
            retorno.Items.Any().ShouldBeTrue();
        }

        [Fact]
        public async Task Ao_filtra_ocorrencia_por_todos_atributos()
        {
            await CriarDadosBasicos();
            await CriarOcorrencia();

            var dtoFiltro = new FiltroOcorrenciaListagemDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Titulo = "Ocorrência",
                Semestre = 1,
                TurmaId = TURMA_ID_1,
                AlunoNome = "Aluno 1",
                ServidorNome = "3333333",
                DataOcorrenciaInicio = DateTimeExtension.HorarioBrasilia().Date,
                TipoOcorrencia = 1
            };
            var useCase = ListarOcorrenciasUseCase();
            var retorno = await useCase.Executar(dtoFiltro);
            retorno.ShouldNotBeNull();
            retorno.Items.Any().ShouldBeTrue();
        }

        [Fact(DisplayName = "Ocorrência - Ao filtrar por Todas Ues")]
        public async Task Ao_filtra_ocorrencia_por_todas_ues()
        {
            await CriarDadosBasicos();
            await CriarOcorrencia();
            long idTodasUes = -99;
            var dtoFiltro = new FiltroOcorrenciaListagemDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = idTodasUes,
                DreId = 2
            };
            var useCase = ListarOcorrenciasUseCase();
            var retorno = await useCase.Executar(dtoFiltro);
            retorno.ShouldNotBeNull();
            retorno.Items.Any().ShouldBeTrue();
            retorno.Items.Count().ShouldBeEquivalentTo(4);
            retorno.Items.Select(x => !string.IsNullOrWhiteSpace(x.UeOcorrencia)).Count().ShouldBeGreaterThan(0);
            retorno.Items.FirstOrDefault().UeOcorrencia.ShouldBeEquivalentTo("EMEF UE 2");
            retorno.Items.FirstOrDefault().DataOcorrencia.ShouldBeEquivalentTo(DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"));

        }
        
        
        private async Task CriarOcorrencia()
        {
            await InserirNaBase(new OcorrenciaObj
            {
                OcorrenciaTipoId = 1,
                Titulo = "Ocorrência",
                Descricao = "Ocorrência de incidente",
                UeId = UE_ID_1,
                TurmaId = TURMA_ID_1,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().TimeOfDay,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new OcorrenciaObj
            {
                OcorrenciaTipoId = 1,
                Titulo = "Ocorrência 2",
                Descricao = "Ocorrência de incidente 2",
                UeId = UE_ID_2,
                TurmaId = TURMA_ID_1,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().TimeOfDay,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new OcorrenciaObj
            {
                OcorrenciaTipoId = 1,
                Titulo = "Ocorrência 22",
                Descricao = "Ocorrência de incidente 2",
                UeId = UE_ID_2,
                TurmaId = TURMA_ID_1,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().TimeOfDay,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new OcorrenciaObj
            {
                OcorrenciaTipoId = 1,
                Titulo = "Ocorrência 32",
                Descricao = "Ocorrência de incidente 33",
                UeId = UE_ID_3,
                TurmaId = TURMA_ID_1,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().TimeOfDay,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new OcorrenciaObj
            {
                OcorrenciaTipoId = 1,
                Titulo = "Ocorrência 3",
                Descricao = "Ocorrência de incidente 3",
                UeId = UE_ID_3,
                TurmaId = TURMA_ID_1,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().TimeOfDay,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await CriarOcorrenciaAluno();
            await CriarOcorrenciaServidor();
        }

        private async Task CriarOcorrenciaAluno()
        {
            await InserirNaBase(new OcorrenciaAluno
            {
                CodigoAluno = ALUNO_1,
                OcorrenciaId = 1
            });
            await InserirNaBase(new OcorrenciaAluno
            {
                CodigoAluno = ALUNO_2,
                OcorrenciaId = 1
            });
        }

        private async Task CriarOcorrenciaServidor()
        {
            await InserirNaBase(new OcorrenciaServidor
            {
                CodigoServidor = RF_3333333,
                OcorrenciaId = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OcorrenciaServidor
            {
                CodigoServidor = RF_4444444,
                OcorrenciaId = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }
    }
}
