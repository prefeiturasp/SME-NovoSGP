using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.Base;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_reprocessar_conselho_classe_aluno: ConselhoClasseTesteBase
    {
        public Ao_reprocessar_conselho_classe_aluno(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_reprocessar_situacao_conselho_classe_aluno()
        {
            var filtroConselhoClasse = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                InserirConselhoClassePadrao = true,
                InserirFechamentoAlunoPadrao = true,
                CriarPeriodoEscolar = true,
                TipoNota = TipoNota.Nota,
                AnoTurma = "6"
            };

            await CriarDadosBase(filtroConselhoClasse);

            var consolidarConselhoClasseUseCase = RetornarConsolidarConselhoClasseUseCase();

            var retorno = await consolidarConselhoClasseUseCase.Executar(1);
            
            retorno.ShouldBeTrue();
        }
    }
}