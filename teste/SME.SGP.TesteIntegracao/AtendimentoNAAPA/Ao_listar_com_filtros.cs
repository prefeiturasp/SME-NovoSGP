using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    public class Ao_listar_com_filtros: AtendimentoNAAPATesteBase
    {
        public Ao_listar_com_filtros(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ServicosFake.ObterTurmasAlunoPorFiltroQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery, IEnumerable<AbrangenciaTurmaRetorno>>), typeof(ServicosFake.ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho por Ano Letivo, Dre, Ue e Questão Prioridade")]
        public async Task Deve_retornar_registros_ao_filtrar_por_situacao_rascunho_por_ano_letivo_dre_ue_questao_prioridade()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentos(dataAtual);

            var respostas = ObterTodos<RespostaEncaminhamentoNAAPA>();

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(10);
            retorno.Items.Any(a=> !a.Situacao.Equals(SituacaoNAAPA.Rascunho.ToString())).ShouldBeFalse();
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho por Ano Letivo, Dre, Ue e Questão Data Entrada Queixa")]
        public async Task Deve_retornar_registros_ao_filtrar_por_situacao_rascunho_por_ano_letivo_dre_ue_questao_data_entrada_queixa()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentos(dataAtual);

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                DataAberturaQueixaInicio = new DateTime(dataAtual.Year, 11, 1),
                DataAberturaQueixaFim = new DateTime(dataAtual.Year, 11, 18),
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(6);
            retorno.Items.Any(a=> !a.Situacao.Equals(SituacaoNAAPA.Rascunho.ToString())).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho somente por Ano Letivo e Dre")]
        public async Task Deve_retornar_registros_ao_filtrar_por_situacao_rascunho_somente_por_ano_e_dre()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentos(dataAtual);

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                TurmaId = TURMA_ID_1,
                DreId = 1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(10);
            retorno.Items.Any(a=> !a.Situacao.Equals(SituacaoNAAPA.Rascunho.ToString())).ShouldBeFalse();            
        }
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho somente por Ano Letivo e Dre e Nome/Código do Aluno")]
        public async Task Deve_retornar_registros_ao_filtrar_por_situacao_rascunho_somente_por_ano_e_dre_nome_aluno()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentos(dataAtual);

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                TurmaId = TURMA_ID_1,
                DreId = 1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                CodigoNomeAluno = "nome"
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(10);
            retorno.Items.Any(a=> !a.Situacao.Equals(SituacaoNAAPA.Rascunho.ToString())).ShouldBeFalse();

            filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                TurmaId = TURMA_ID_1,
                DreId = 1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                CodigoNomeAluno = ALUNO_CODIGO_1
            };

            retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(10);
            retorno.Items.Any(a => !a.Situacao.Equals(SituacaoNAAPA.Rascunho.ToString())).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho somente por Ano Letivo e Dre e Nome do Aluno inválido")]
        public async Task Nao_deve_retornar_registros_ao_filtrar_por_situacao_rascunho_somente_por_ano_e_dre_nome_aluno_invalido()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentos(dataAtual);

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                TurmaId = TURMA_ID_1,
                DreId = 1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                CodigoNomeAluno = "aluno não identificado"
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(0);          
        }
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho por Ano Letivo e Dre")]
        public async Task Deve_retornar_registros_ao_filtrar_por_situacao_rascunho_por_ano_letivo_dre()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentos(dataAtual);

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                DreId = 1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(10);
            retorno.Items.Any(a=> !a.Situacao.Equals(SituacaoNAAPA.Rascunho.ToString())).ShouldBeFalse();
            
            await InserirNaBase(new Dre
            {
                CodigoDre = DRE_CODIGO_1,
                Abreviacao = DRE_NOME_1,
                Nome = DRE_NOME_1
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = UE_CODIGO_1,
                DreId = 1,
                Nome = UE_NOME_1,
            });
        }
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho por Ano Letivo, Dre e Ue")]
        public async Task Deve_retornar_registros_ao_filtrar_por_situacao_rascunho_por_ano_letivo_dre_e_ue()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentos(dataAtual);

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                DreId = 1,
                CodigoUe = "1",
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(10);
            retorno.Items.Any(a=> !a.Situacao.Equals(SituacaoNAAPA.Rascunho.ToString())).ShouldBeFalse();
            retorno.Items.Any(a=> !a.Ue.Contains(UE_NOME_1)).ShouldBeFalse();
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação Em Atendimento por Ano Letivo, Dre e Ue")]
        public async Task Deve_retornar_registros_ao_filtrar_por_situacao_em_atendimento_por_ano_letivo_dre_e_ue()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.EmAtendimento,
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentosEmAtendimento(dataAtual);

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                DreId = 1,
                CodigoUe = "1",
                Situacao = (int)SituacaoNAAPA.EmAtendimento,
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(2);
            retorno.Items.All(a => a.Situacao.Equals(SituacaoNAAPA.EmAtendimento.ObterNome())).ShouldBeTrue();
            retorno.Items.All(a => a.Ue.Contains(UE_NOME_1)).ShouldBeTrue();
            retorno.Items.All(a => a.Turma.Contains("EF-Turma Nome 1")).ShouldBeTrue();
            retorno.Items.All(a => a.DataUltimoAtendimento.Equals(dataAtual)).ShouldBeTrue();
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho por Ano Letivo, Dre sem encaminhamento")]
        public async Task Nao_deve_retornar_registros_ao_filtrar_por_situacao_rascunho_por_ano_letivo_e_Dre_sem_encaminhamento()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentos(dataAtual);

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                DreId = 10,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho por Ano Letivo, Dre e Todas as Ue")]
        public async Task Deve_retornar_registros_ao_filtrar_por_situacao_rascunho_por_ano_letivo_dre_e_todas_Ues()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            await CriarDadosBase(filtroNAAPA);

            await CriarEncaminhamentos(dataAtual);

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                DreId = 1,
                CodigoUe = "-99",
                Situacao = (int)SituacaoNAAPA.Rascunho,
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(10);
            retorno.Items.Any(a=> !a.Situacao.Equals(SituacaoNAAPA.Rascunho.ToString())).ShouldBeFalse();
            retorno.Items.Any(a=> !a.Ue.Contains(UE_NOME_1)).ShouldBeFalse();
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho por Ano Letivo, Dre e Todas as Ue com ordenação")]
        public async Task Deve_retornar_registros_ao_filtrar_por_situacao_rascunho_por_ano_letivo_dre_e_todas_ues_com_ordenacao()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                CriarTurmaPadrao = false
            };

            await CriarDadosBase(filtroNAAPA);
            await CriarTurma(Modalidade.Fundamental, "5", TURMA_CODIGO_1, TipoTurma.Regular, UE_ID_2, dataAtual.Year);
            await CriarTurma(Modalidade.Fundamental, "6", TURMA_CODIGO_2, TipoTurma.Regular, UE_ID_2, dataAtual.Year);
            await CriarTurma(Modalidade.Fundamental, "7", TURMA_CODIGO_3, TipoTurma.Regular, UE_ID_3, dataAtual.Year);
            await CriarTurma(Modalidade.Fundamental, "8", TURMA_CODIGO_4, TipoTurma.Regular, UE_ID_3, dataAtual.Year);

            await CriarEncaminhamento(dataAtual, TURMA_ID_1, CODIGO_ALUNO_1, "ALUNO A");
            await CriarEncaminhamento(dataAtual.AddDays(-2), TURMA_ID_2, CODIGO_ALUNO_2, "ALUNO B");
            await CriarEncaminhamento(dataAtual.AddDays(-1), TURMA_ID_3, CODIGO_ALUNO_3, "ALUNO C");
            await CriarEncaminhamento(dataAtual, TURMA_ID_4, CODIGO_ALUNO_4, "ALUNO D");

            var obterEncaminhamentosNAAPAUseCase = ObterServicoListagemComFiltros();

            var filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                DreId = DRE_ID_2,
                CodigoUe = "-99",
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Ordenacao = new OrdenacaoListagemPaginadaAtendimentoNAAPA[] { OrdenacaoListagemPaginadaAtendimentoNAAPA.DataEntradaQueixaDesc,
                    OrdenacaoListagemPaginadaAtendimentoNAAPA.UE,
                    OrdenacaoListagemPaginadaAtendimentoNAAPA.Estudante }
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(4);
            var primeiroRegistro = retorno.Items.FirstOrDefault();
            primeiroRegistro.DataAberturaQueixaInicio.ShouldBe(dataAtual);
            primeiroRegistro.Ue.ShouldBe("EMEF UE 2");
            primeiroRegistro.NomeAluno.ShouldBe("ALUNO A");

            var segundoRegistro = retorno.Items.ElementAt(1);
            segundoRegistro.DataAberturaQueixaInicio.ShouldBe(dataAtual);
            segundoRegistro.Ue.ShouldBe("EMEF UE 3");
            segundoRegistro.NomeAluno.ShouldBe("ALUNO D");

            var terceiroRegistro = retorno.Items.ElementAt(2);
            terceiroRegistro.DataAberturaQueixaInicio.ShouldBe(dataAtual.AddDays(-1));
            terceiroRegistro.Ue.ShouldBe("EMEF UE 3");
            terceiroRegistro.NomeAluno.ShouldBe("ALUNO C");

            var quartoRegistro = retorno.Items.ElementAt(3);
            quartoRegistro.DataAberturaQueixaInicio.ShouldBe(dataAtual.AddDays(-2));
            quartoRegistro.Ue.ShouldBe("EMEF UE 2");
            quartoRegistro.NomeAluno.ShouldBe("ALUNO B");

            filtroEncaminhamentosNAAPADto = new FiltroAtendimentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                DreId = DRE_ID_2,
                CodigoUe = "-99",
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Ordenacao = new OrdenacaoListagemPaginadaAtendimentoNAAPA[] {
                    OrdenacaoListagemPaginadaAtendimentoNAAPA.UEDesc,
                    OrdenacaoListagemPaginadaAtendimentoNAAPA.DataEntradaQueixa,
                    OrdenacaoListagemPaginadaAtendimentoNAAPA.Estudante }
            };

            retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(4);
            primeiroRegistro = retorno.Items.FirstOrDefault();
            primeiroRegistro.DataAberturaQueixaInicio.ShouldBe(dataAtual.AddDays(-1));
            primeiroRegistro.Ue.ShouldBe("EMEF UE 3");
            primeiroRegistro.NomeAluno.ShouldBe("ALUNO C");

            segundoRegistro = retorno.Items.ElementAt(1);
            segundoRegistro.DataAberturaQueixaInicio.ShouldBe(dataAtual);
            segundoRegistro.Ue.ShouldBe("EMEF UE 3");
            segundoRegistro.NomeAluno.ShouldBe("ALUNO D");


            terceiroRegistro = retorno.Items.ElementAt(2);
            terceiroRegistro.DataAberturaQueixaInicio.ShouldBe(dataAtual.AddDays(-2));
            terceiroRegistro.Ue.ShouldBe("EMEF UE 2");
            terceiroRegistro.NomeAluno.ShouldBe("ALUNO B");

            quartoRegistro = retorno.Items.ElementAt(3);
            quartoRegistro.DataAberturaQueixaInicio.ShouldBe(dataAtual);
            quartoRegistro.Ue.ShouldBe("EMEF UE 2");
            quartoRegistro.NomeAluno.ShouldBe("ALUNO A");

        }

        private async Task CriarEncaminhamento(DateTime dataQueixa, long turmaId, string codigoAluno, string nomeAluno)
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = turmaId,
                AlunoCodigo = codigoAluno,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoNome = nomeAluno,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = ObterUltimoId<Dominio.EncaminhamentoNAAPA>(),
                SecaoEncaminhamentoNAAPAId = ID_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = ObterUltimoId<Dominio.EncaminhamentoNAAPASecao>(),
                QuestaoId = ID_QUESTAO_DATA_ENTRADA_QUEIXA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = ObterUltimoId<Dominio.QuestaoEncaminhamentoNAAPA>(),
                Texto = dataQueixa.ToString("yyyy-MM-dd"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = ObterUltimoId<Dominio.EncaminhamentoNAAPASecao>(),
                QuestaoId = ID_QUESTAO_PRIORIDADE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = ObterUltimoId<Dominio.QuestaoEncaminhamentoNAAPA>(),
                RespostaId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentos(DateTime dataAtual)
        {
            var encaminhamentoNAAPAId = 1;
            var encaminhamentoNAAPASecaoID = 1;
            var questaoEncaminhamentoNAAPAId = 1;
            var dataQueixa = new DateTime(dataAtual.Year, 11, 01);

            for (int i = 1; i <= 10; i++)
            {
                await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
                {
                    TurmaId = TURMA_ID_1,
                    AlunoCodigo = ALUNO_CODIGO_1,
                    Situacao = SituacaoNAAPA.Rascunho,
                    AlunoNome = NOME_ALUNO_1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
                {
                    EncaminhamentoNAAPAId = encaminhamentoNAAPAId,
                    SecaoEncaminhamentoNAAPAId = 1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
                {
                    EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoID,
                    QuestaoId = 1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
                {
                    QuestaoEncaminhamentoId = questaoEncaminhamentoNAAPAId,
                    Texto = dataQueixa.ToString("yyyy-MM-dd"),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });
                questaoEncaminhamentoNAAPAId++;
                dataQueixa = dataQueixa.AddDays(i);

                await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
                {
                    EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoID,
                    QuestaoId = 2,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
                {
                    QuestaoEncaminhamentoId = questaoEncaminhamentoNAAPAId,
                    RespostaId = 1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });
                encaminhamentoNAAPASecaoID++;
                questaoEncaminhamentoNAAPAId++;
                encaminhamentoNAAPAId++;
            }
        }

        private async Task CriarEncaminhamentosEmAtendimento(DateTime dataAtual)
        {
            var dataQueixa = dataAtual.AddDays(-5);

            for (int i = 1; i <= 2; i++)
            {
                await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
                {
                    TurmaId = TURMA_ID_1,
                    AlunoCodigo = ALUNO_CODIGO_1,
                    Situacao = SituacaoNAAPA.EmAtendimento,
                    AlunoNome = NOME_ALUNO_1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
                {
                    EncaminhamentoNAAPAId = ObterUltimoId<Dominio.EncaminhamentoNAAPA>(),
                    SecaoEncaminhamentoNAAPAId = ID_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
                {
                    EncaminhamentoNAAPASecaoId = ObterUltimoId<Dominio.EncaminhamentoNAAPASecao>(),
                    QuestaoId = ID_QUESTAO_DATA_ENTRADA_QUEIXA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
                {
                    QuestaoEncaminhamentoId = ObterUltimoId<Dominio.QuestaoEncaminhamentoNAAPA>(),
                    Texto = dataQueixa.ToString("yyyy-MM-dd"),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
                dataQueixa = dataQueixa.AddDays(i);

                await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
                {
                    EncaminhamentoNAAPASecaoId = ObterUltimoId<Dominio.EncaminhamentoNAAPASecao>(),
                    QuestaoId = ID_QUESTAO_PRIORIDADE,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
                {
                    QuestaoEncaminhamentoId = ObterUltimoId<Dominio.QuestaoEncaminhamentoNAAPA>(),
                    RespostaId = 1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                //ITINERANCIAS - 1
                await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
                {
                    EncaminhamentoNAAPAId = ObterUltimoId<Dominio.EncaminhamentoNAAPA>(),
                    SecaoEncaminhamentoNAAPAId = ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
                {
                    EncaminhamentoNAAPASecaoId = ObterUltimoId<Dominio.EncaminhamentoNAAPASecao>(),
                    QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
                {
                    QuestaoEncaminhamentoId = ObterUltimoId<Dominio.QuestaoEncaminhamentoNAAPA>(),
                    Texto = dataAtual.AddDays(-5).ToString("yyyy-MM-dd"),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                }) ;

                //ITINERANCIAS - 2
                await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
                {
                    EncaminhamentoNAAPAId = ObterUltimoId<Dominio.EncaminhamentoNAAPA>(),
                    SecaoEncaminhamentoNAAPAId = ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
                {
                    EncaminhamentoNAAPASecaoId = ObterUltimoId<Dominio.EncaminhamentoNAAPASecao>(),
                    QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
                {
                    QuestaoEncaminhamentoId = ObterUltimoId<Dominio.QuestaoEncaminhamentoNAAPA>(),
                    Texto = dataAtual.ToString("yyyy-MM-dd"),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }
    }
}