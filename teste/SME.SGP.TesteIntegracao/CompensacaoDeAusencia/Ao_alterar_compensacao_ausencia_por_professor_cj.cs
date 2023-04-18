using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_alterar_compensacao_ausencia_por_professor_cj : Ao_lancar_compensacao_de_ausencia_base
    {
        public Ao_alterar_compensacao_ausencia_por_professor_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Compensação de Ausência - Deve alterar a compensações pelo perfil professor cj")]
        public async Task Deve_alterar_compensacao()
        {
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            var dtoDadoBase = ObterDtoDadoBase(ObterPerfilCJ(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecuteAlterarCompensacaoAusenciaComAulasSelecionadas(dtoDadoBase);
        }
    }
}