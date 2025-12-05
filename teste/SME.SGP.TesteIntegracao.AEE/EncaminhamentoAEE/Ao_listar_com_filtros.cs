using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_listar_com_filtros : EncaminhamentoAEETesteBase
    {
        public Ao_listar_com_filtros(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(PlanoAEE.ServicosFakes.ObterTurmasAlunoPorFiltroPlanoAEEQueryHandlerFake), ServiceLifetime.Scoped));
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
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
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
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
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
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
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
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
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
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
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

        [Fact]
        public async Task Ao_filtrar_encaminhamento_sem_ue()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);
            await CriarTurma(Modalidade.Fundamental, "1", TURMA_CODIGO_2, TipoTurma.Regular, UE_ID_2, ANO_LETIVO_ANO_ATUAL, false);
            await CriarTurma(Modalidade.Fundamental, "1", TURMA_CODIGO_3, TipoTurma.Regular, UE_ID_3, ANO_LETIVO_ANO_ATUAL, false);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_2,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                ResponsavelId = 1
            });

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_3,
                AlunoCodigo = ALUNO_CODIGO_2,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 2",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                ResponsavelId = 1
            });

            var filtroPesquisaEncaminhamentosAeeDto = new FiltroPesquisaEncaminhamentosAEEDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DreId = 2,
                ResponsavelRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                Situacao = SituacaoAEE.Rascunho
            };

            var retorno = await ObterServicoListagemComFiltros().Executar(filtroPesquisaEncaminhamentosAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(2);
            retorno.Items.ToList().Exists(encaminhamento => encaminhamento.Ue == "EMEF UE 2").ShouldBeTrue();
            retorno.Items.ToList().Exists(encaminhamento => encaminhamento.Ue == "EMEF UE 3").ShouldBeTrue();
        }

        [Fact]
        public async Task Ao_filtrar_encaminhamento_sem_exibir_encerrados()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);
            await CriarTurma(Modalidade.Fundamental, "1", TURMA_CODIGO_2, TipoTurma.Regular, UE_ID_2, ANO_LETIVO_ANO_ATUAL, false);
            await CriarTurma(Modalidade.Fundamental, "1", TURMA_CODIGO_3, TipoTurma.Regular, UE_ID_3, ANO_LETIVO_ANO_ATUAL, false);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_2,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Encaminhado,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                ResponsavelId = 1
            });

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_3,
                AlunoCodigo = ALUNO_CODIGO_2,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 2",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                ResponsavelId = 1
            });

            var filtroPesquisaEncaminhamentosAeeDto = new FiltroPesquisaEncaminhamentosAEEDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DreId = 2,
                ResponsavelRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                Situacao = SituacaoAEE.Rascunho
            };

            var retorno = await ObterServicoListagemComFiltros().Executar(filtroPesquisaEncaminhamentosAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(1);
            retorno.Items.FirstOrDefault().Situacao.ShouldBe("Em digitação");
        }
    }
}