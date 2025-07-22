using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados.SolicitarInclusaoComunicados
{
    public class SolicitarInclusaoComunicadoEscolaAquiUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SolicitarInclusaoComunicadoEscolaAquiUseCase useCase;

        private static readonly Guid PerfilSme = Guid.NewGuid();
        private static readonly Guid PerfilDre = Guid.NewGuid();
        private static readonly Guid PerfilUe = Guid.NewGuid();

        public SolicitarInclusaoComunicadoEscolaAquiUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SolicitarInclusaoComunicadoEscolaAquiUseCase(mediatorMock.Object);
        }

        private Usuario CriarUsuarioComPerfil(params TipoPerfil[] perfis)
        {
            var usuario = new Usuario();
            var perfisUsuario = perfis.Select(p => new PrioridadePerfil { Tipo = p, CodigoPerfil = p == TipoPerfil.SME ? PerfilSme : p == TipoPerfil.DRE ? PerfilDre : PerfilUe, Ordem = 1 }).ToList();
            usuario.DefinirPerfis(perfisUsuario);
            usuario.DefinirPerfilAtual(perfisUsuario.First().CodigoPerfil);
            usuario.Login = "usuarioTeste";
            return usuario;
        }

        private ComunicadoInserirDto CriarComunicado()
        {
            return new ComunicadoInserirDto
            {
                CodigoDre = "01",
                CodigoUe = "001",
                Turmas = new List<string> { "101" },
                Modalidades = new int[] { 1 },
                TiposEscolas = new int[] { 1 },
                AlunoEspecificado = false,
                Alunos = new List<string>(),
                Titulo = "Teste",
                Descricao = "Descricao teste",
                AnoLetivo = 2025,
                DataEnvio = DateTime.Now
            };
        }

        [Fact]
        public async Task Executar_Deve_Expandir_Modalidades_E_Tipos_Escolas_Quando_Contiver_Todas()
        {
            var comunicado = CriarComunicado();
            comunicado.Modalidades = new int[] { -99 };
            comunicado.TiposEscolas = new int[] { -99 };

            var usuario = CriarUsuarioComPerfil(TipoPerfil.SME);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<InserirComunicadoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(comunicado);

            Assert.Equal("Comunicado criado com sucesso!", resultado);
            Assert.DoesNotContain(-99, comunicado.Modalidades);
            Assert.DoesNotContain(-99, comunicado.TiposEscolas);

            Assert.NotEmpty(comunicado.Modalidades);
            Assert.NotEmpty(comunicado.TiposEscolas);
        }

        [Fact]
        public async Task Executar_Deve_Lanca_Erro_Quando_Inserir_Comunicado_Command_Retornar_Falso()
        {
            var comunicado = CriarComunicado();
            var usuario = CriarUsuarioComPerfil(TipoPerfil.SME);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            mediatorMock.Setup(m => m.Send(It.IsAny<InserirComunicadoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(comunicado));
        }

        [Theory(DisplayName = "ValidarInsercao - Deve lançar exceção para combinações inválidas de abrangência")]
        [InlineData("-99", "001", "Não é possível especificar uma escola quando o comunicado é para todas as DREs")]
        [InlineData("01", "-99", "Não é possível especificar uma turma quando o comunicado é para todas as UEs")]
        public async Task Validar_Insercao_Deve_Lancar_Excecao_Quando_Abrangencia_Invalida(string codigoDre, string codigoUe, string mensagemEsperada)
        {
            var comunicado = CriarComunicado();
            comunicado.CodigoDre = codigoDre;
            comunicado.CodigoUe = codigoUe;
            comunicado.Turmas = codigoUe == "-99" ? new List<string> { "100" } : new List<string> { "-99" };
            comunicado.AlunoEspecificado = false;
            comunicado.Alunos = new List<string>();

            var usuario = CriarUsuarioComPerfil(TipoPerfil.SME);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(comunicado));
            Assert.Equal(mensagemEsperada, ex.Message);
        }

        [Fact]
        public async Task Validar_Insercao_Deve_Lancar_Erro_Quando_Aluno_Especificado_Para_Todas_Turmas()
        {
            var comunicado = CriarComunicado();
            comunicado.CodigoDre = "01";
            comunicado.CodigoUe = "001";
            comunicado.Turmas = new List<string> { "-99" };
            comunicado.AlunoEspecificado = true;
            comunicado.Alunos = new List<string> { "aluno1" };

            var usuario = CriarUsuarioComPerfil(TipoPerfil.SME);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(comunicado));
            Assert.Equal("Não é possível especificar alunos quando o comunicado é para todas as Turmas", ex.Message);
        }

        [Fact]
        public async Task Validar_Abrangencia_Usuario_Deve_Lancar_Erro_Quando_Usuario_Nao_Sme_Enviar_Para_Todas_Dres()
        {
            var comunicado = CriarComunicado();
            comunicado.CodigoDre = "-99";
            comunicado.CodigoUe = "001";

            var usuario = CriarUsuarioComPerfil(TipoPerfil.DRE);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(comunicado));
            Assert.Equal("Apenas usuários SME podem realizar envio de Comunicados para todas as DREs", ex.Message);
        }

        [Fact]
        public async Task Validar_Abrangencia_Usuario_Deve_Lancar_Erro_Quando_Usuario_Nao_Sme_Ou_Dre_Enviar_Para_Todas_Escolas()
        {
            var comunicado = CriarComunicado();
            comunicado.CodigoDre = "01";
            comunicado.CodigoUe = "-99";

            var usuario = CriarUsuarioComPerfil(TipoPerfil.UE);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(comunicado));
            Assert.Equal("Apenas usuários SME e DRE podem realizar envio de Comunicados para todas as Escolas", ex.Message);
        }

        [Fact]
        public async Task Validar_Abrangencia_Usuario_Deve_Validar_Abrangencia_Dre_Quando_Usuario_Dre_E_Com_Codigo_Dre_Especifico()
        {
            var comunicado = CriarComunicado();
            comunicado.CodigoDre = "01"; // não TODAS
            comunicado.CodigoUe = "001";

            var usuario = CriarUsuarioComPerfil(TipoPerfil.DRE);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.Is<ObterAbrangenciaDresPorLoginEPerfilQuery>(q =>
                    q.Login == usuario.Login && q.Perfil == usuario.PerfilAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaDreRetornoDto>
                {
                new AbrangenciaDreRetornoDto { Codigo = "01" }
                });

            mediatorMock.Setup(m => m.Send(It.IsAny<InserirComunicadoCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var resultado = await useCase.Executar(comunicado);

            Assert.Equal("Comunicado criado com sucesso!", resultado);
        }

        [Fact]
        public async Task Validar_Abrangencia_Usuario_Deve_Lancar_Erro_Quando_Usuario_Dre_Nao_Possuir_Permissao()
        {
            var comunicado = CriarComunicado();
            comunicado.CodigoDre = "99";
            comunicado.CodigoUe = "001";

            var usuario = CriarUsuarioComPerfil(TipoPerfil.DRE);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbrangenciaDresPorLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaDreRetornoDto>());

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(comunicado));
            Assert.Equal("Usuário não possui permissão para enviar comunicados para a DRE com codigo 99", ex.Message);
        }

        [Fact]
        public async Task Validar_Abrangencia_Usuario_Deve_Validar_Abrangencia_Ue_E_Turma_Quando_Usuario_Ue_E_Com_Codigo_Ue_Especifico()
        {
            var comunicado = CriarComunicado();
            comunicado.CodigoDre = "01";
            comunicado.CodigoUe = "001";
            comunicado.Turmas = new List<string> { "101", "102" };

            var usuario = CriarUsuarioComPerfil(TipoPerfil.UE);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AbrangenciaCompactaVigenteRetornoEOLDTO
                {
                    Abrangencia = new AbrangenciaCargoRetornoEolDTO
                    {
                        Abrangencia = Infra.Enumerados.Abrangencia.UE
                    }
                });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAbrangenciaUesPorLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaUeRetorno>
                {
                  new AbrangenciaUeRetorno { Codigo = "001" }
                });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAbrangenciaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AbrangenciaFiltroRetorno { CodigoTurma = "101" });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<InserirComunicadoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(comunicado);

            Assert.Equal("Comunicado criado com sucesso!", resultado);
        }

        [Fact]
        public async Task Validar_Abrangencia_Usuario_Deve_Lancar_Erro_Quando_Usuario_Ue_Nao_Possuir_Permissao()
        {
            var comunicado = CriarComunicado();
            comunicado.CodigoDre = "01";
            comunicado.CodigoUe = "999";

            var usuario = CriarUsuarioComPerfil(TipoPerfil.UE);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbrangenciaUesPorLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaUeRetorno>());

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(comunicado));
            Assert.Equal("Usuário não possui permissão para enviar comunicados para a UE com codigo 999", ex.Message);
        }

        [Fact]
        public async Task Validar_Abrangencia_Usuario_Deve_Lancar_Erro_Quando_Usuario_Nao_Possuir_Permissao_Turma()
        {
            var comunicado = CriarComunicado();
            comunicado.CodigoDre = "01";
            comunicado.CodigoUe = "001";
            comunicado.Turmas = new List<string> { "101", "102" };

            var usuario = CriarUsuarioComPerfil(TipoPerfil.UE);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AbrangenciaCompactaVigenteRetornoEOLDTO
                {
                    Abrangencia = new AbrangenciaCargoRetornoEolDTO
                    {
                        Abrangencia = Infra.Enumerados.Abrangencia.UE
                    }
                });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAbrangenciaUesPorLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AbrangenciaUeRetorno> { new AbrangenciaUeRetorno { Codigo = "001" } });

            mediatorMock
                .SetupSequence(m => m.Send(It.IsAny<ObterAbrangenciaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AbrangenciaFiltroRetorno { CodigoTurma = "101" })
                .ReturnsAsync((AbrangenciaFiltroRetorno)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(comunicado));

            ex.Message.Should().Be("Usuário não possui permissão para enviar comunicados para a Turma com código 102");
        }
    }
}
