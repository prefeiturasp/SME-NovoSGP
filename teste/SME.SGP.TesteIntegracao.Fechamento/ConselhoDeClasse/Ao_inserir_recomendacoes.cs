using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_Inserir_Recomendacoes : ConselhoDeClasseTesteBase
    {
        private const string TEXTO_LIVRE_ALUNO = "Recomendações ao estudante texto livre";
        private const string TEXTO_LIVRE_FAMILIA = "Recomendações a família texto livre";
        private const string TEXTO_LIVRE_ALUNO_ALTERAR = "Recomendações ao estudante";
        private const string TEXTO_LIVRE_FAMILIA_ALTERAR = "Recomendações a família";
        public Ao_Inserir_Recomendacoes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //BULCK INSERT
        //[Fact]
        public async Task Ao_inserir_apenas_recomendacoes_a_familia_e_aos_estudantes_pre_cadastrados()
        {
            await CriarDados();

            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoRecomendacaoUseCase>();

            var dto = new ConselhoClasseAlunoAnotacoesDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                RecomendacaoAlunoIds = new long[] { 1,2 },
                RecomendacaoFamiliaIds = new long[] { 3,4 }
            };
            var conselhoAluno = await useCase.Executar(dto);

            conselhoAluno.ShouldNotBeNull();
            ValidaRecomendacaoPreCadastradas(dto);
        }

        //BULCK INSERT
        //[Fact]
        public async Task Ao_inserir_apenas_o_texto_livre_nos_campos_de_recomendacao_a_familia_e_estudante()
        {
            await CriarDados();

            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoRecomendacaoUseCase>();
            var dto = new ConselhoClasseAlunoAnotacoesDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                RecomendacaoAluno = TEXTO_LIVRE_ALUNO,
                RecomendacaoFamilia = TEXTO_LIVRE_FAMILIA,
                RecomendacaoAlunoIds = new long[] { },
                RecomendacaoFamiliaIds = new long[] { }
            };

            var conselhoAluno = await useCase.Executar(dto);

            ValidaRecomendacaoCampoLivres(conselhoAluno);
        }

        //BULCK INSERT
        //[Fact]
        public async Task Ao_inserir_recomendacoes_pre_cadastradas_e_texto_livre_nos_dois_campos()
        {
            await CriarDados();

            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoRecomendacaoUseCase>();
            var dto = ObterConselhoAlunoAnotacaoDto();
            var conselhoAluno = await useCase.Executar(dto);

            ValidaRecomendacaoCampoLivres(conselhoAluno);
            ValidaRecomendacaoPreCadastradas(dto);
        }

        //BULCK INSERT
        //[Fact]
        public async Task Ao_alterar_recomendacoes_pre_cadastradas_e_texto_livre_nos_dois_campos()
        {
            await CriarDados();

            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoRecomendacaoUseCase>();
            var dto = ObterConselhoAlunoAnotacaoDto();
            await useCase.Executar(dto);

            var dtoAlteracao = new ConselhoClasseAlunoAnotacoesDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                RecomendacaoAluno = TEXTO_LIVRE_ALUNO_ALTERAR,
                RecomendacaoFamilia = TEXTO_LIVRE_FAMILIA_ALTERAR,
                RecomendacaoAlunoIds = new long[] { 2 },
                RecomendacaoFamiliaIds = new long[] { 4 }
            };

            var conselhoAluno = await useCase.Executar(dtoAlteracao);
            conselhoAluno.ShouldNotBeNull();
            conselhoAluno.RecomendacoesAluno.ShouldBe(TEXTO_LIVRE_ALUNO_ALTERAR);
            conselhoAluno.RecomendacoesFamilia.ShouldBe(TEXTO_LIVRE_FAMILIA_ALTERAR);

            var listaAlunoRecomendacao = ObterTodos<ConselhoClasseAlunoRecomendacao>().FindAll(recomenda => recomenda.ConselhoClasseAlunoId == 1);
            listaAlunoRecomendacao.ShouldNotBeNull();
            var listaRecamendacao = ObterTodos<Dominio.ConselhoClasseRecomendacao>();
            listaRecamendacao.ShouldNotBeNull();

            ValidaRecomendacao(dtoAlteracao.RecomendacaoAlunoIds.ToList(), listaAlunoRecomendacao, listaRecamendacao, ConselhoClasseRecomendacaoTipo.Aluno);
            ValidaRecomendacao(dtoAlteracao.RecomendacaoFamiliaIds.ToList(), listaAlunoRecomendacao, listaRecamendacao, ConselhoClasseRecomendacaoTipo.Familia);

            listaAlunoRecomendacao.Exists(recomenda => recomenda.ConselhoClasseRecomendacaoId == 1).ShouldBeFalse();
            listaAlunoRecomendacao.Exists(recomenda => recomenda.ConselhoClasseRecomendacaoId == 3).ShouldBeFalse();
        }
        
        [Fact]
        public async Task Nao_deve_inserir_recomendacoes_sem_periodo_abertura_apos_encerramento_bimestre()
        {
            var filtroConselhoClasse = new FiltroConselhoClasseDto()
            {
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_4,
                AnoTurma = ANO_7,
                CriarPeriodoEscolar = false
            };

            await CriarDadosBaseSemFechamentoTurmaSemAberturaReabertura(filtroConselhoClasse);

            await CriarFechamentoTurmaDisciplinaAlunoNota(filtroConselhoClasse);

            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoRecomendacaoUseCase>();
            var dto = new ConselhoClasseAlunoAnotacoesDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                RecomendacaoAluno = TEXTO_LIVRE_ALUNO,
                RecomendacaoFamilia = TEXTO_LIVRE_FAMILIA,
                RecomendacaoAlunoIds = new long[] { },
                RecomendacaoFamiliaIds = new long[] { }
            };

            await Assert.ThrowsAsync<NegocioException>(async () => await useCase.Executar(dto));
        }
        
        [Fact]
        public async Task Nao_deve_inserir_recomendacoes_sem_periodo_rabertura_apos_encerramento_bimestre_e_abertura()
        {
            var filtroConselhoClasse = new FiltroConselhoClasseDto()
            {
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_4,
                AnoTurma = ANO_7,
                ConsiderarAnoAnterior = true
            };

            await CriarDadosBaseSemFechamentoTurmaSemAberturaReabertura(filtroConselhoClasse);

            await CriarFechamentoTurmaDisciplinaAlunoNota(filtroConselhoClasse);

            await CriarPeriodoAberturaCustomizadoQuartoBimestre(false);

            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoRecomendacaoUseCase>();
            var dto = new ConselhoClasseAlunoAnotacoesDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                RecomendacaoAluno = TEXTO_LIVRE_ALUNO,
                RecomendacaoFamilia = TEXTO_LIVRE_FAMILIA,
                RecomendacaoAlunoIds = new long[] { },
                RecomendacaoFamiliaIds = new long[] { }
            };

            await Assert.ThrowsAsync<NegocioException>(async () => await useCase.Executar(dto));
        }

        [Fact]
        public async Task Deve_obter_calendario_correto_de_turma_eja_celp_considerando_semestre()
        {
            await DadosInsercaoRecomendacoesConselho();

            var turma = new Dominio.Turma()
            {
                Id = 1,
                Nome = "1A",
                CodigoTurma = "1234",
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.EJA,
                UeId = 1,
                Semestre = 1
            };

            var mediator = await ServiceProvider.GetService<IMediator>().Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, BIMESTRE_2));

            mediator.ShouldNotBeNull();
            mediator.PeriodoFim.ShouldBe(new DateTime(DateTimeExtension.HorarioBrasilia().Year, 6, 30));
            mediator.PeriodoInicio.ShouldBe(new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1));
        }

        private async Task DadosInsercaoRecomendacoesConselho()
        {
            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Calendário Teste Ano Atual",
                Modalidade = ModalidadeTipoCalendario.EJA,
                Periodo = Periodo.Anual,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = "",
                Semestre = 1
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                Bimestre = 2,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 6, 30),
                TipoCalendarioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dre()
            {
                Id = 1,
                Nome = "Dre Teste",
                CodigoDre = "11",
                Abreviacao = "DT"
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                Nome = "Ue Teste",
                DreId = 1,
                TipoEscola = TipoEscola.EMEF,
                CodigoUe = "22"
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                Nome = "1A",
                CodigoTurma = "1234",
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.EJA,
                UeId = 1,
                Semestre = 1
            });

        }

        private ConselhoClasseAlunoAnotacoesDto ObterConselhoAlunoAnotacaoDto()
        {
            return new ConselhoClasseAlunoAnotacoesDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                RecomendacaoAluno = TEXTO_LIVRE_ALUNO,
                RecomendacaoFamilia = TEXTO_LIVRE_FAMILIA,
                RecomendacaoAlunoIds = new long[] { 1, 2 },
                RecomendacaoFamiliaIds = new long[] { 3, 4 }
            };
        }

        private void ValidaRecomendacaoCampoLivres(ConselhoClasseAluno conselhoAluno)
        {
            conselhoAluno.ShouldNotBeNull();
            conselhoAluno.RecomendacoesAluno.ShouldBe(TEXTO_LIVRE_ALUNO);
            conselhoAluno.RecomendacoesFamilia.ShouldBe(TEXTO_LIVRE_FAMILIA);
        }

        private void ValidaRecomendacaoPreCadastradas(ConselhoClasseAlunoAnotacoesDto dto)
        {
            var listaAlunoRecomendacao = ObterTodos<ConselhoClasseAlunoRecomendacao>().FindAll(recomenda => recomenda.ConselhoClasseAlunoId == 1);
            listaAlunoRecomendacao.ShouldNotBeNull();
            var listaRecamendacao = ObterTodos<Dominio.ConselhoClasseRecomendacao>();
            listaRecamendacao.ShouldNotBeNull();

            ValidaRecomendacao(dto.RecomendacaoAlunoIds.ToList(), listaAlunoRecomendacao, listaRecamendacao, ConselhoClasseRecomendacaoTipo.Aluno);
            ValidaRecomendacao(dto.RecomendacaoFamiliaIds.ToList(), listaAlunoRecomendacao, listaRecamendacao, ConselhoClasseRecomendacaoTipo.Familia);
        }

        private void ValidaRecomendacao(
            List<long> listaIds,
            List<ConselhoClasseAlunoRecomendacao> listaAlunoRecomendacao,
            List<Dominio.ConselhoClasseRecomendacao> listaRecamendacao,
            ConselhoClasseRecomendacaoTipo tipo)
        {
            foreach (var id in listaIds)
            {
                listaAlunoRecomendacao.Exists(recomenda => recomenda.Id == id).ShouldBeTrue();
                var recomendacao = listaRecamendacao.Find(recomenda => recomenda.Id == id);
                recomendacao.ShouldNotBeNull();
                recomendacao.Tipo.ShouldBe(tipo);
            }
        }

        private async Task CriarDados()
        {
            var filtroNota = new FiltroConselhoClasseDto()
            {
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                AnoTurma = ANO_7
            };

            await CriarDadosBase(filtroNota);
            await InserirConselhoClassePadrao(filtroNota);
            await CriarFechamentoTurmaDisciplinaAlunoNota(filtroNota);
        }
    }
}
