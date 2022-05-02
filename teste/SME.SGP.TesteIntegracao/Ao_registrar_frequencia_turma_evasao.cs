using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_registrar_frequencia_turma_evasao : TesteBase
    {
        public Ao_registrar_frequencia_turma_evasao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_registrar_apenas_alunos_com_frequencia_abaixo_50_porcento_e_maior_que_0_porcento()
        {

        }

        [Fact]
        public async Task Deve_registrar_apenas_alunos_sem_frequencia()
        {

        }

        private async Task CriarRegistrosConsolidacaoFrequenciaAlunoMensal()
        {
            await InserirNaBase(new ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "1",
                Mes = 5,
                Percentual = 50,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 5,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "2",
                Mes = 5,
                Percentual = 80,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 8,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "3",
                Mes = 5,
                Percentual = 40,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 6,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "4",
                Mes = 5,
                Percentual = 30,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 7,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "5",
                Mes = 5,
                Percentual = 0,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 10,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "6",
                Mes = 5,
                Percentual = 0,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 10,
                QuantidadeCompensacoes = 0
            });
        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1
            });

            await InserirNaBase(new Turma
            {
                Id = 1,
                UeId = 1,
                Ano = "1",
                CodigoTurma = "1",
                AnoLetivo = 2022
            });

            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                Nome = "",
                CriadoPor = "",
                CriadoRF = ""
            });
        }
    }
}
