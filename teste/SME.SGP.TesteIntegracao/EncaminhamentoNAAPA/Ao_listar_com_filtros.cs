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

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_listar_com_filtros: EncaminhamentoNAAPATesteBase
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

            var filtroEncaminhamentosNAAPADto = new FiltroEncaminhamentoNAAPADto()
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

            var filtroEncaminhamentosNAAPADto = new FiltroEncaminhamentoNAAPADto()
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

            var filtroEncaminhamentosNAAPADto = new FiltroEncaminhamentoNAAPADto()
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
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Filtrar encaminhamentos por situação rascunho somente por Ano Letivo e Dre e Nome do Aluno")]
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

            var filtroEncaminhamentosNAAPADto = new FiltroEncaminhamentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                TurmaId = TURMA_ID_1,
                DreId = 1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                NomeAluno = "nome"
            };

            var retorno = await obterEncaminhamentosNAAPAUseCase.Executar(filtroEncaminhamentosNAAPADto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(10);
            retorno.Items.Any(a=> !a.Situacao.Equals(SituacaoNAAPA.Rascunho.ToString())).ShouldBeFalse();            
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

            var filtroEncaminhamentosNAAPADto = new FiltroEncaminhamentoNAAPADto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ExibirHistorico = true,
                TurmaId = TURMA_ID_1,
                DreId = 1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                NomeAluno = "aluno não identificado"
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

            var filtroEncaminhamentosNAAPADto = new FiltroEncaminhamentoNAAPADto()
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

            var filtroEncaminhamentosNAAPADto = new FiltroEncaminhamentoNAAPADto()
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

            var filtroEncaminhamentosNAAPADto = new FiltroEncaminhamentoNAAPADto()
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

            var filtroEncaminhamentosNAAPADto = new FiltroEncaminhamentoNAAPADto()
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
    }
}