using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
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
            var useCase = ServiceProvider.GetService<IComandosNotasConceitos>();
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
                NotasConceitos = listNota
            };
            await useCase.Salvar(dto);
        }
    }
}
