using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_validar_situacao_conselho_classe : ConselhoDeClasseTesteBase
    {
        public Ao_validar_situacao_conselho_classe(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_apresentar_situacao_nao_iniciado()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_8, Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio, false, false);

            var consulta = ServiceProvider.GetService<IConsultasConselhoClasseRecomendacao>();
            consulta.ShouldNotBeNull(); }
        
        private async Task CriarDados(
            string componenteCurricular,
            string anoTurma, 
            Modalidade modalidade,
            ModalidadeTipoCalendario modalidadeTipoCalendario,
            bool criarConselhosTodosBimestres,
            bool criarConselhoClasseFinal
        )
        {            
            var filtroNota = new FiltroNotasDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                ComponenteCurricular = componenteCurricular,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = false,
                DataAula = DATA_03_01_INICIO_BIMESTRE_1,
                TipoNota = TipoNota.Nota,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                CriarFechamentoDisciplinaAlunoNota = true,
                CriarConselhosTodosBimestres = criarConselhosTodosBimestres,
                CriarConselhoClasseFinal = criarConselhoClasseFinal
            };
            
            await CriarDadosBase(filtroNota);
        }        
    }
}