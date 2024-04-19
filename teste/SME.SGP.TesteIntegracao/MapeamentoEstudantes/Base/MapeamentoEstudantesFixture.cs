using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base
{
    public class MapeamentoEstudantesFixture : CollectionFixture
    {
        private readonly TesteBaseUtils baseComum;
        public MapeamentoEstudantesFixture()
        {
            baseComum = new TesteBaseUtils(this);
            Task.Run(() => CriarDadosBase()).Wait();
        }

        public async Task CriarDadosBase()
        {
            ExecutarScripts(new List<ScriptCarga> { ScriptCarga.CARGA_QUESTIONARIO_MAPEAMENTO_ESTUDANTE });

            await baseComum.CriarDreUePerfilComponenteCurricular();

            baseComum.CriarClaimUsuario(baseComum.ObterPerfilProfessor());

            await baseComum.CriarUsuarios();

            await baseComum.CriarTipoCalendario(Dominio.ModalidadeTipoCalendario.FundamentalMedio, false);

            await baseComum.CriarTurma(Dominio.Modalidade.Fundamental, TesteBaseConstantes.ANO_4, false);
        }
    }

    [CollectionDefinition(nameof(MapeamentoEstudantesFixture))]
    public class MapeamentoEstudantesCollection : ICollectionFixture<MapeamentoEstudantesFixture>
    { }
}
