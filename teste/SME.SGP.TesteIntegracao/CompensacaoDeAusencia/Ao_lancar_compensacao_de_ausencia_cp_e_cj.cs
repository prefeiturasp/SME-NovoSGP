using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_de_ausencia_cp_e_cj : CompensacaoDeAusenciaTesteBase
    {
        public Ao_lancar_compensacao_de_ausencia_cp_e_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_lancar_ausencia_para_cp()
        {
            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            var dto = ObtenhaCompensacaoAusenciaDto(
                            COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                            BIMESTRE_1,
                            ObtenhaListaDeAlunos());

            await comando.Inserir(dto); 
        }

        private List<CompensacaoAusenciaAlunoDto> ObtenhaListaDeAlunos()
        {
            return new List<CompensacaoAusenciaAlunoDto>();
        }
    }
}
