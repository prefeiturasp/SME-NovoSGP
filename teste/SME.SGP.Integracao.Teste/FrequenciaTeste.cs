//using SME.SGP.Infra;
//using Xunit;

//namespace SME.SGP.Integracao.Teste
//{
//TODO retornar quando teste integrado de usuário funcionar
//    [Collection("Testserver collection")]
//    public class FrequenciaTeste
//    {
//        private TestServerFixture _fixture;

//        public FrequenciaTeste(TestServerFixture fixture)
//        {
//            _fixture = fixture;
//        }

//        [Fact]
//        public void Deve_Obter_Frequencia()
//        {
//            _fixture = TesteBase.ObtenhaCabecalhoAuthentication(_fixture, new Permissao[] { Permissao.PDA_C, Permissao.TCE_I, Permissao.PE_I, Permissao.CP_I });

//            TesteBase.AdicionarTipoCalendario(_fixture);
//            TesteBase.AdicionarPeriodoEscolar(_fixture);
//            TesteBase.AdicionarAula(_fixture);

//            var resposta = TesteBase.ExecuteGetAsync(_fixture, $"/api/v1/calendarios/frequencias?aulaId=1");
//            Assert.True(resposta.IsSuccessStatusCode);
//        }
//    }
//}