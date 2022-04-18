using Shouldly;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Commands
{
    public class ComandosNotasConceitosTest : TesteBase
    {
        private readonly TestFixture testFixture;
        public ComandosNotasConceitosTest(TestFixture testFixture) : base(testFixture)
        {
            this.testFixture = testFixture;
        }

        [Fact]
        public async Task Lancar_Conceito_Para_Componente_Regencia_de_Classe_Eja()
        {
            var usuarioLogado = testFixture.Autenticar("6926886", "Sgp@1234");
            var listNota = new List<NotaConceitoDto>()
            {
             new NotaConceitoDto()
                 {
                     AlunoId = "7128291",
                     AtividadeAvaliativaId = 13143296,
                     Conceito = 2,
                     Nota=null
                 },
            };
            var dto = new NotaConceitoListaDto
            {
                DisciplinaId = "1114",
                TurmaId = "2366531",
                NotasConceitos =listNota
            };
            var response = await testFixture.Client.PostAsJsonAsync("api/v1/avaliacoes/notas/", dto);
            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
