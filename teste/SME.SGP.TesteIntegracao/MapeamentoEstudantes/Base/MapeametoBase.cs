using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base
{
    public class MapeamentoBase : TesteBaseComuns
    {
        public MapeamentoBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            
        }

        protected async Task CriarDadosBase()
        {
            ExecutarScripts(new List<ScriptCarga> { ScriptCarga.CARGA_QUESTIONARIO_MAPEAMENTO_ESTUDANTE });

            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await CriarTipoCalendario(Dominio.ModalidadeTipoCalendario.FundamentalMedio, false);

            await CriarTurma(Dominio.Modalidade.Fundamental, ANO_4, false, tipoTurno: 2);
        }
    }
}
