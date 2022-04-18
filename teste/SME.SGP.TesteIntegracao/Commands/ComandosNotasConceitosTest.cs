using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Commands
{
    public class ComandosNotasConceitosTest : TesteBase
    {
        public ComandosNotasConceitosTest(TestFixture testFixture) : base(testFixture)
        {
        }

        [Fact]
        public async Task Lancar_Conceito_Para_Componentrse_Regencia_de_Classe_Eja()
        {
            var command = ServiceProvider.GetService<IComandosNotasConceitos>();
            var notasConceitosConsulta = ServiceProvider.GetService<IRepositorioNotasConceitosConsulta>();
            var atividadeAvaliativa = ServiceProvider.GetService<IRepositorioAtividadeAvaliativa>();

            var servicoUsuario = ServiceProvider.GetService<IServicoUsuario>();
            var servicoDeNotasConceitos = ServiceProvider.GetService<IServicoDeNotasConceitos>();

            var listaDeNotas = new List<NotaConceitoDto>()
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
                NotasConceitos = listaDeNotas
            };
            await command.Salvar(dto);
        }
    }
}
