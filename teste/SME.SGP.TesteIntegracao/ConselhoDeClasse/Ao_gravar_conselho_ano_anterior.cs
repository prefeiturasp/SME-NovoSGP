using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_gravar_conselho_ano_anterior : ConselhoDeClasseTesteBase
    {
        public Ao_gravar_conselho_ano_anterior(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_gravar_conselho_final_sem_existencia_do_conselho_4_bimestre_fundamental()
        {
            await CriarBase(TipoNota.Nota);
        }
        
        [Fact]
        public async Task Deve_gravar_conselho_final_sem_existencia_do_conselho_2_bimestre_eja()
        {
            await CriarBase(TipoNota.Nota);
        }

        private async Task CriarBase(TipoNota tipoNota)
        {
            var filtroConselhoClasse = new FiltroNotasDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_FINAL,
                AnoTurma = ANO_7,
                TipoNota = tipoNota,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                ConsiderarAnoAnterior = false
            };

            await CriarDadosBase(filtroConselhoClasse);
        }
    }
}