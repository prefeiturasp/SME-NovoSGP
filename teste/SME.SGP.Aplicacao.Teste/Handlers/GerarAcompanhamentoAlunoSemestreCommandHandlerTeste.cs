using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste
{
    public class GerarAcompanhamentoAlunoSemestreCommandHandlerTeste
    {
        private readonly Mock<IRepositorioAcompanhamentoAlunoSemestre> _repositorio;
        private readonly GerarAcompanhamentoAlunoSemestreCommandHandler _handler;

        public GerarAcompanhamentoAlunoSemestreCommandHandlerTeste()
        {
            _repositorio = new Mock<IRepositorioAcompanhamentoAlunoSemestre>();
            _handler = new GerarAcompanhamentoAlunoSemestreCommandHandler(_repositorio.Object);
        }

        [Fact]
        public async Task Deve_Salvar_Acompanhamento_Com_Sucesso()
        {
            var command = new GerarAcompanhamentoAlunoSemestreCommand(1, 1, "obs", "percurso");

            var resultado = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resultado);
            _repositorio.Verify(x => x.SalvarAsync(It.IsAny<AcompanhamentoAlunoSemestre>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Acompanhamento_Com_Dados_Corretos()
        {
            var command = new GerarAcompanhamentoAlunoSemestreCommand(123, 2, "observacao", "percurso");

            var resultado = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(123, resultado.AcompanhamentoAlunoId);
            Assert.Equal(2, resultado.Semestre);
            Assert.Equal("observacao", resultado.Observacoes);
            Assert.Equal("percurso", resultado.PercursoIndividual);
        }
    }
}