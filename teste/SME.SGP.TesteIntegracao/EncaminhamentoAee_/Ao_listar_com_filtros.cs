using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_listar_com_filtros: EncaminhamentoAEETesteBase
    {
        public Ao_listar_com_filtros(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTurmasAlunoPorFiltroQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery, IEnumerable<AbrangenciaTurmaRetorno>>), typeof(ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_filtrar_por_situacao()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            var obterEncaminhamentosAeeUseCase = ObterServicoListagemComFiltros();

            var filtroPesquisaEncaminhamentosAeeDto = new FiltroPesquisaEncaminhamentosAEEDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Situacao = SituacaoAEE.Rascunho,
                DreId = 1,
                UeId = 1
            };

            var retorno = await obterEncaminhamentosAeeUseCase.Executar(filtroPesquisaEncaminhamentosAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(1);
            retorno.Items.FirstOrDefault().Situacao.Equals("Em digitação").ShouldBeTrue();
        }
        
        [Fact]
        public async Task Ao_filtrar_por_turma()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            var obterEncaminhamentosAeeUseCase = ObterServicoListagemComFiltros();

            var filtroPesquisaEncaminhamentosAeeDto = new FiltroPesquisaEncaminhamentosAEEDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TurmaId = TURMA_ID_1,
                DreId = 1,
                UeId = 1
            };

            var retorno = await obterEncaminhamentosAeeUseCase.Executar(filtroPesquisaEncaminhamentosAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(1);
        }
        
        [Fact]
        public async Task Ao_filtrar_por_estudante()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            var obterEncaminhamentosAeeUseCase = ObterServicoListagemComFiltros();

            var filtroPesquisaEncaminhamentosAeeDto = new FiltroPesquisaEncaminhamentosAEEDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TurmaId = TURMA_ID_1,
                DreId = 1,
                UeId = 1,
                AlunoCodigo = ALUNO_CODIGO_1
            };

            var retorno = await obterEncaminhamentosAeeUseCase.Executar(filtroPesquisaEncaminhamentosAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(1);
        }
        
        [Fact]
        public async Task Ao_filtrar_por_reponsavel()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF,
                ResponsavelId = 1
            });
            
            var obterEncaminhamentosAeeUseCase = ObterServicoListagemComFiltros();

            var filtroPesquisaEncaminhamentosAeeDto = new FiltroPesquisaEncaminhamentosAEEDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DreId = 1,
                UeId = 1,
                ResponsavelRf = USUARIO_PROFESSOR_CODIGO_RF_2222222
            };

            var retorno = await obterEncaminhamentosAeeUseCase.Executar(filtroPesquisaEncaminhamentosAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(1);
        }
        
        [Fact]
        public async Task Ao_filtrar_por_turma_estudante_situacao_reponsavel()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF,
                ResponsavelId = 1
            });
            
            var obterEncaminhamentosAeeUseCase = ObterServicoListagemComFiltros();

            var filtroPesquisaEncaminhamentosAeeDto = new FiltroPesquisaEncaminhamentosAEEDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DreId = 1,
                UeId = 1,
                ResponsavelRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoAEE.Rascunho
            };

            var retorno = await obterEncaminhamentosAeeUseCase.Executar(filtroPesquisaEncaminhamentosAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(1);
        }
    }
}