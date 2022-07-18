using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamento
{
    public class Ao_Lancar_nota_conceito : NotaFechamentoTesteBase
    {
        public Ao_Lancar_nota_conceito(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_diferente_de_regencia()
        {
            await ExecuteTesteConceito(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.Infantil);
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_de_regencia_Fundamental() 
        {
            await ExecuteTesteConceito(ObterPerfilProfessor(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, Modalidade.Fundamental, ModalidadeTipoCalendario.Infantil);
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_de_regencia_EJA()
        {
            await ExecuteTesteConceito(ObterPerfilProfessor(), COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114, Modalidade.EJA, ModalidadeTipoCalendario.EJA);
        }


        [Fact]
        public async Task Deve_Lancar_nota_conceito_cp() 
        {
            await ExecuteTesteConceito(ObterPerfilCP(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.Infantil);
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_diretor()
        {
            await ExecuteTesteConceito(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.Infantil);
        }
    }
}
