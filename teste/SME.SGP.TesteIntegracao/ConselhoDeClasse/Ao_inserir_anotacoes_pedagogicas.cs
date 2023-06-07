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
    public class Ao_inserir_anotacoes_pedagogicas : ConselhoDeClasseTesteBase
    {
        public Ao_inserir_anotacoes_pedagogicas(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //bulk insert
        //[Fact]
        public async Task Deve_inserir_anotacoes()
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito);
            await CriarDados(ObterPerfilProfessor(), salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular, TipoNota.Conceito, ANO_4, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, SituacaoConselhoClasse.EmAndamento, false);
            await InserirConselhoDeClasse();

            var dto = ObterAnotacoesDto();

            var retorno = await ExecutarComandoSalvarConselhoClasseAluno(dto);

            retorno.ShouldNotBeNull();
            retorno.AnotacoesPedagogicas.ShouldNotBeNullOrEmpty();
        }

        private async Task<ConselhoClasseAluno> ExecutarComandoSalvarConselhoClasseAluno(ConselhoClasseAlunoAnotacoesDto dto)
        {
            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoRecomendacaoUseCase>();
            return await useCase.Executar(dto);
        }

        private async Task InserirConselhoDeClasse()
        {
            var fechamento = ObterTodos<FechamentoTurma>().FirstOrDefault();
            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                Situacao = SituacaoConselhoClasse.EmAndamento,
                FechamentoTurma = fechamento,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        private ConselhoClasseAlunoAnotacoesDto ObterAnotacoesDto()
        {
            return new ConselhoClasseAlunoAnotacoesDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                AnotacoesPedagogicas = "Teste",
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                RecomendacaoAluno = "teste",
                RecomendacaoAlunoIds = new List<long>() { 1 },
                RecomendacaoFamilia = "teste",
                RecomendacaoFamiliaIds = new List<long>() { 1 }
            };
        }

        private async Task CriarDados(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool anoAnterior, SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado, bool criarFechamentoDisciplinaAlunoNota = false)
        {
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = componente.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                SituacaoConselhoClasse = situacaoConselhoClasse
            };

            await CriarDadosBase(filtroNota);
        }
    }
}