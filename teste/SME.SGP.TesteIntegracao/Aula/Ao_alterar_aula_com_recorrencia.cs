using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Aula
{
    public class Ao_alterar_aula_com_recorrencia : AulaMockComponentePortugues
    {
        public Ao_alterar_aula_com_recorrencia(CollectionFixture collectionFixture) : base(collectionFixture) { }



        [Fact]
        public async Task Altera_aula_com_recorrencia()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), new System.DateTime(2022, 02, 10), RecorrenciaAula.RepetirBimestreAtual);
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual);
        }
    }
}
