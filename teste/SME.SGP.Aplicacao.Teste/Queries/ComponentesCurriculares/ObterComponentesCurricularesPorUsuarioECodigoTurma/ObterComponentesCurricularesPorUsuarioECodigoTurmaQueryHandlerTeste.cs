using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorUsuarioECodigoTurma
{
    public class ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandler handler;

        public ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandlerTeste()
        {
            mediatorMock = new Mock<IMediator>();
            handler = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandler(mediatorMock.Object);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Mediator_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandler(null));
        }

        [Fact]
        public async Task Deve_Obter_Componentes_Atribuicao_Cj_Quando_Usuario_For_Professor_Cj()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessorCj(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var atribuicoes = ObterAtribuicoes();
            var disciplinasEol = ObterDisciplinasEol();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(atribuicoes);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(disciplinasEol);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("1", resultado.First().Codigo);
            Assert.Equal("Matemática", resultado.First().Nome);
            Assert.Equal("2", resultado.Last().Codigo);
            Assert.Equal("Português", resultado.Last().Nome);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(),
                               It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(),
                               It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Professor_Cj_Nao_Possui_Atribuicoes()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessorCj(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync((IEnumerable<AtribuicaoCJ>)null);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.Null(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(),
                               It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Professor_Cj_Possui_Atribuicoes_Vazias()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessorCj(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new List<AtribuicaoCJ>());

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Obter_Componentes_Curriculares_Usuario_Quando_Nao_For_Professor_Cj()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessor(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var turma = ObterTurma(2024);
            var componentesCurriculares = ObterComponentesCurriculares();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(componentesCurriculares);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Geografia", resultado.First().Nome);
            Assert.Equal("História", resultado.Last().Nome);
        }

        [Fact]
        public async Task Deve_Usar_Login_Quando_CodigoRf_For_Nulo()
        {
            var login = "usuario.login";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoSemCodigoRf(login);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var turma = ObterTurma(2024);
            var componentesCurriculares = ObterComponentesCurriculares();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(componentesCurriculares);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            mediatorMock.Verify(m => m.Send(
                It.Is<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(
                    q => q.Login == login),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Realizar_Agrupamento_Quando_Ano_Letivo_Menor_Ou_Igual_2024()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessor(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var turma = ObterTurma(2024);
            var componentesCurriculares = ObterComponentesCurriculares();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(componentesCurriculares);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            mediatorMock.Verify(m => m.Send(
                It.Is<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(
                    q => q.RealizarAgrupamentoComponente == true),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Realizar_Agrupamento_Quando_Ano_Letivo_Maior_Que_2024()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessor(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var turma = ObterTurma(2025);
            var componentesCurriculares = ObterComponentesCurriculares();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(componentesCurriculares);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            mediatorMock.Verify(m => m.Send(
                It.Is<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(
                    q => q.RealizarAgrupamentoComponente == false),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Nao_Encontrar_Componentes_Curriculares()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessor(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var turma = ObterTurma(2024);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync((IEnumerable<ComponenteCurricularEol>)null);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Componentes_Curriculares_Estiver_Vazio()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessor(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var turma = ObterTurma(2024);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new List<ComponenteCurricularEol>());

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Usar_Descricao_Componente_Infantil_Quando_Modalidade_For_Educacao_Infantil_E_ExibirComponenteEOL_True()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessor(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var turma = ObterTurmaEducacaoInfantil();
            var componentesCurriculares = ObterComponentesCurricularesEducacaoInfantil();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(componentesCurriculares);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal("Brincar e Interagir", resultado.First().Nome);
        }

        [Fact]
        public async Task Deve_Usar_Descricao_Normal_Quando_ExibirComponenteEOL_False()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessor(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var turma = ObterTurmaEducacaoInfantil();
            var componentesCurriculares = ObterComponentesCurricularesComExibirComponenteEolFalse();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(componentesCurriculares);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal("Descrição Normal", resultado.First().Nome);
        }

        [Fact]
        public async Task Deve_Ordenar_Componentes_Por_Nome()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessorCj(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var atribuicoes = ObterAtribuicoes();
            var disciplinasEol = ObterDisciplinasEolDesordenadas();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(atribuicoes);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(disciplinasEol);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal("Ciências", resultado.First().Nome);
            Assert.Equal("Português", resultado.Last().Nome);
        }

        [Fact]
        public async Task Deve_Remover_Disciplinas_Duplicadas_Na_Atribuicao_Cj()
        {
            var codigoRf = "1234567";
            var turmaCodigo = "123456";
            var usuarioLogado = ObterUsuarioLogadoProfessorCj(codigoRf);
            var request = new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo);

            var atribuicoesDuplicadas = ObterAtribuicoesComDisciplinasDuplicadas();
            var disciplinasEol = ObterDisciplinasEol();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(atribuicoesDuplicadas);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(),
                              It.IsAny<CancellationToken>()))
                       .ReturnsAsync(disciplinasEol);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            mediatorMock.Verify(m => m.Send(
                It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(
                    q => q.Ids.Length == 2),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        private Usuario ObterUsuarioLogadoProfessorCj(string codigoRf)
        {
            var usuario = new Usuario { CodigoRf = codigoRf, PerfilAtual = Perfis.PERFIL_CJ, };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    CodigoPerfil = Perfis.PERFIL_CJ,
                    Tipo = TipoPerfil.UE,
                    Ordem = 0,
                    NomePerfil = "Professor CJ"
                }
            });

            return usuario;
        }

        private Usuario ObterUsuarioLogadoProfessor(string codigoRf)
        {
            var usuario = new Usuario { CodigoRf = codigoRf, PerfilAtual = Perfis.PERFIL_PROFESSOR };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    CodigoPerfil = Perfis.PERFIL_PROFESSOR,
                    Tipo = TipoPerfil.UE,
                    Ordem = 0,
                    NomePerfil = "Professor"
                }
            });

            return usuario;
        }

        private Usuario ObterUsuarioLogadoSemCodigoRf(string login)
        {
            var usuario = new Usuario { CodigoRf = null, Login = login, PerfilAtual = Perfis.PERFIL_PROFESSOR };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    CodigoPerfil = Perfis.PERFIL_PROFESSOR,
                    Tipo = TipoPerfil.UE,
                    Ordem = 0,
                    NomePerfil = "Professor"
                }
            });

            return usuario;
        }

        private Turma ObterTurma(int anoLetivo)
        {
            return new Turma
            {
                AnoLetivo = anoLetivo,
                ModalidadeCodigo = Modalidade.Fundamental,
                CodigoTurma = "123456"
            };
        }

        private Turma ObterTurmaEducacaoInfantil()
        {
            return new Turma
            {
                AnoLetivo = 2024,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                CodigoTurma = "123456"
            };
        }

        private IEnumerable<ComponenteCurricularEol> ObterComponentesCurriculares()
        {
            return new List<ComponenteCurricularEol>
            {
                new ComponenteCurricularEol { Codigo = 10, Descricao = "História", ExibirComponenteEOL = false },
                new ComponenteCurricularEol { Codigo = 20, Descricao = "Geografia", ExibirComponenteEOL = false }
            };
        }

        private IEnumerable<ComponenteCurricularEol> ObterComponentesCurricularesEducacaoInfantil()
        {
            return new List<ComponenteCurricularEol>
            {
                new ComponenteCurricularEol
                {
                    Codigo = 1,
                    Descricao = "Descrição Normal",
                    DescricaoComponenteInfantil = "Brincar e Interagir",
                    ExibirComponenteEOL = true
                }
            };
        }

        private IEnumerable<ComponenteCurricularEol> ObterComponentesCurricularesComExibirComponenteEolFalse()
        {
            return new List<ComponenteCurricularEol>
            {
                new ComponenteCurricularEol
                {
                    Codigo = 1,
                    Descricao = "Descrição Normal",
                    DescricaoComponenteInfantil = "Brincar e Interagir",
                    ExibirComponenteEOL = false
                }
            };
        }

        private IEnumerable<AtribuicaoCJ> ObterAtribuicoes()
        {
            return new List<AtribuicaoCJ>
            {
                new AtribuicaoCJ { DisciplinaId = 1 },
                new AtribuicaoCJ { DisciplinaId = 2 }
            };
        }

        private IEnumerable<AtribuicaoCJ> ObterAtribuicoesComDisciplinasDuplicadas()
        {
            return new List<AtribuicaoCJ>
            {
                new AtribuicaoCJ { DisciplinaId = 1 },
                new AtribuicaoCJ { DisciplinaId = 2 },
                new AtribuicaoCJ { DisciplinaId = 1 },
                new AtribuicaoCJ { DisciplinaId = 2 }
            };
        }

        private IEnumerable<DisciplinaDto> ObterDisciplinasEol()
        {
            return new List<DisciplinaDto>
            {
                new DisciplinaDto { Id = 1, Nome = "Matemática" },
                new DisciplinaDto { Id = 2, Nome = "Português" }
            };
        }

        private IEnumerable<DisciplinaDto> ObterDisciplinasEolDesordenadas()
        {
            return new List<DisciplinaDto>
            {
                new DisciplinaDto { Id = 1, Nome = "Português" },
                new DisciplinaDto { Id = 2, Nome = "Matemática" },
                new DisciplinaDto { Id = 3, Nome = "Ciências" }
            };
        }
    }
}
