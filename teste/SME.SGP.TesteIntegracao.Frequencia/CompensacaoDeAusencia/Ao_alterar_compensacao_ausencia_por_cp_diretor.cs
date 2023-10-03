using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_alterar_compensacao_ausencia_por_cp_diretor : Ao_lancar_compensacao_de_ausencia_base
    {
        public Ao_alterar_compensacao_ausencia_por_cp_diretor(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Compensação de Ausência - Deve alterar a compensações pelo perfil cp")]
        public async Task Deve_alterar_compensacao_perfil_cp()
        {
            var dtoBase = ObtenhaDtoDadoBase(ObterPerfilCP(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecuteAlterarCompensacaoAusenciaSemAulasSelecionadas(dtoBase);
        }

        [Fact(DisplayName = "Compensação de Ausência - Deve alterar a compensações pelo perfil diretor")]
        public async Task Deve_alterar_compensacao_perfil_diretor()
        {
            var dtoBase = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecuteAlterarCompensacaoAusenciaSemAulasSelecionadas(dtoBase);
        }
    }
}