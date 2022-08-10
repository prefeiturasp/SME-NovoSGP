using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasseLancamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasseLancamento
{
    public class Ao_inserir_conceito_pos_conselho_bimestre : ConselhoDeClasseLancamentoBase
    {
        public Ao_inserir_conceito_pos_conselho_bimestre(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //protected override void RegistrarFakes(IServiceCollection services)
        //{
        //    base.RegistrarFakes(services);

        //}

        [Fact]
        public async Task Deve_lancar_conceito_pos_conselho_bimestre()
        {
            await CrieDados(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito, ANO_4, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            var comando = ServiceProvider.GetService<IComandosConselhoClasseNota>();

            var dto = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Conceito = 1,
                Justificativa = "TESTE"
            };

            var dtoRetorno = await comando.SalvarAsync(dto, ALUNO_CODIGO_1, CONSELHO_CLASSE_ID, FECHAMENTO_TURMA_ID, TURMA_CODIGO_1, BIMESTRE_2);
            dtoRetorno.ShouldNotBeNull();

            var listaConselhoClasseNota = ObterTodos<ConselhoClasseNota>();

            listaConselhoClasseNota.ShouldHaveSingleItem();
            var conselhoNota = listaConselhoClasseNota.FirstOrDefault(x => x.ConselhoClasseAlunoId == 1);
            conselhoNota.ConceitoId.ShouldBe(1);
        }

        [Fact]
        public async Task Deve_lancar_conceito_pos_conselho_bimestre_regencia_fundamental()
        {
            await CrieDados(ObterPerfilProfessor(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, TipoNota.Conceito, ANO_4, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            var comando = ServiceProvider.GetService<IComandosConselhoClasseNota>();

            var dto = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105,
                Conceito = 1,
                Justificativa = "TESTE"
            };

            var dtoRetorno = await comando.SalvarAsync(dto, ALUNO_CODIGO_1, CONSELHO_CLASSE_ID, FECHAMENTO_TURMA_ID, TURMA_CODIGO_1, BIMESTRE_2);
            dtoRetorno.ShouldNotBeNull();
            var listaConselhoClasseNota = ObterTodos<ConselhoClasseNota>();

            listaConselhoClasseNota.ShouldHaveSingleItem();
            var conselhoNota = listaConselhoClasseNota.FirstOrDefault(x => x.ConselhoClasseAlunoId == 1);
            conselhoNota.ComponenteCurricularCodigo.ShouldBe(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105);
        }

        [Fact]
        public async Task Deve_lancar_conceito_pos_conselho_bimestre_regencia_EJA()
        {
            await CrieDados(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito, ANO_4, Modalidade.EJA, ModalidadeTipoCalendario.EJA);
            var comando = ServiceProvider.GetService<IComandosConselhoClasseNota>();

            var dto = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Conceito = 1,
                Justificativa = "TESTE"
            };

            var dtoRetorno = await comando.SalvarAsync(dto, ALUNO_CODIGO_1, CONSELHO_CLASSE_ID, FECHAMENTO_TURMA_ID, TURMA_CODIGO_1, BIMESTRE_2);
            dtoRetorno.ShouldNotBeNull();

            var listaConselhoClasseNota = ObterTodos<ConselhoClasseNota>();

            listaConselhoClasseNota.ShouldHaveSingleItem();
            var conselhoNota = listaConselhoClasseNota.FirstOrDefault(x => x.ConselhoClasseAlunoId == 1);
            conselhoNota.ConceitoId.ShouldBe(1);
        }

        public async Task Deve_lancar_conceito_para_ano_anterior()
        {
        }

        private async Task CrieDados(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = componente.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }
    }
}