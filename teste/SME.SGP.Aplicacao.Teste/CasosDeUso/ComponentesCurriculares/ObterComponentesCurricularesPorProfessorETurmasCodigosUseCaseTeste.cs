using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ComponentesCurriculares
{
    public class ObterComponentesCurricularesPorProfessorETurmasCodigosUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterComponentesCurricularesPorProfessorETurmasCodigosUseCase _useCase;

        public ObterComponentesCurricularesPorProfessorETurmasCodigosUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterComponentesCurricularesPorProfessorETurmasCodigosUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Lista_De_Turmas_Vazia_Deve_Retornar_Lista_Vazia()
        {
            var turmasCodigos = Enumerable.Empty<string>();
            var usuarioLogado = new Usuario { CodigoRf = "123456" };

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, default))
                         .ReturnsAsync(usuarioLogado);

            var resultado = await _useCase.Executar(turmasCodigos);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Componentes_Encontrados_Deve_Retornar_Lista_Distinta()
        {
            var turmasCodigos = new string[] { "T1", "T2", "T3" };
            var usuarioLogado = new Usuario { CodigoRf = "123456" };

            var componentesT1 = new List<DisciplinaNomeDto>
            {
                new DisciplinaNomeDto { Codigo = "1", Nome = "Matemática" },
                new DisciplinaNomeDto { Codigo = "2", Nome = "Língua Portuguesa" }
            };

            var componentesT2 = new List<DisciplinaNomeDto>
            {
                new DisciplinaNomeDto { Codigo = "2", Nome = "Língua Portuguesa" },
                new DisciplinaNomeDto { Codigo = "3", Nome = "Ciências" }
            };

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, default))
                         .ReturnsAsync(usuarioLogado);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery>(q => q.TurmaCodigo == "T1"), default))
                         .ReturnsAsync(componentesT1);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery>(q => q.TurmaCodigo == "T2"), default))
                         .ReturnsAsync(componentesT2);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery>(q => q.TurmaCodigo == "T3"), default))
                         .ReturnsAsync(new List<DisciplinaNomeDto>());

            var resultado = await _useCase.Executar(turmasCodigos);

            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count());
            Assert.Contains(resultado, c => c.Codigo == "1");
            Assert.Contains(resultado, c => c.Codigo == "2");
            Assert.Contains(resultado, c => c.Codigo == "3");
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery>(), default), Times.Exactly(3));
        }
    }
}
