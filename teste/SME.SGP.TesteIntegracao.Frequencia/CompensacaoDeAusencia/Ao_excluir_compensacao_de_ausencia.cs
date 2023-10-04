using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_excluir_compensacao_de_ausencia : Ao_lancar_compensacao_de_ausencia_base
    {
        public Ao_excluir_compensacao_de_ausencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Compensação de Ausência - Deve excluir a compensações pelo perfil professor titular")]
        public async Task Deve_excluir_compensacao_pelo_professor_titular()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecutarTesteRemoverAlunoCompensacao(dtoDadoBase);
        }

        [Fact(DisplayName = "Compensação de Ausência - Deve excluir a compensações pelo perfil professor cj")]
        public async Task Deve_excluir_compensacao_pelo_cj()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilCJ(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecutarTesteRemoverAlunoCompensacao(dtoDadoBase);
        }

        [Fact(DisplayName = "Compensação de Ausência - Deve excluir a compensações pelo perfil cp")]
        public async Task Deve_excluir_compensacao_pelo_cp()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilCP(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecutarTesteRemoverAlunoCompensacao(dtoDadoBase);
        }

        [Fact(DisplayName = "Compensação de Ausência - Deve excluir a compensações pelo perfil diretor")]
        public async Task Deve_excluir_compensacao_pelo_diretor()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecutarTesteRemoverAlunoCompensacao(dtoDadoBase);
        }
    }
}
