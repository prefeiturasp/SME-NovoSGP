using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.Base;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_reprocessar_conselho_classe_aluno: ConselhoDeClasseTesteBase
    {
        public Ao_reprocessar_conselho_classe_aluno(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_reprocessar_situacao_conselho_classe_aluno()
        {
            var filtroConselhoClasse = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                AnoTurma = ANO_6
            };

            await CriarDadosBase(filtroConselhoClasse);

            var consolidarConselhoClasseUseCase = RetornarConsolidarConselhoClasseUseCase();

            var retorno = await consolidarConselhoClasseUseCase.Executar(int.Parse(DRE_ID_1.ToString()));
            
            retorno.ShouldBeTrue();
        }
    }
}