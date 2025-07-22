using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados.SolicitarAlteracaoComunicados
{
    public class SolicitarAlteracaoComunicadoEscolaAquiUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SolicitarAlteracaoComunicadoEscolaAquiUseCase useCase;

        public SolicitarAlteracaoComunicadoEscolaAquiUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SolicitarAlteracaoComunicadoEscolaAquiUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Permitir_Alteracao_Para_Todas_Quando_SME()
        {
            var usuario = CriarUsuarioSME();
            var dto = CriarDto("todas", "todas");

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<AlterarComunicadoCommand>(), default))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(1, dto);

            resultado.Should().Be("Comunicado alterado com sucesso");
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_SME_Para_Todas_DREs()
        {
            var usuario = CriarUsuarioDRE();
            var dto = CriarDto("todas", "0000001");

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(1, dto));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_SME_Ou_DRE_Para_Todas_UEs()
        {
            var usuario = CriarUsuarioUE();
            var dto = CriarDto("0000001", "todas");

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(1, dto));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_UE_Fora_Da_Abrangencia()
        {
            var usuario = CriarUsuarioUE();
            var dto = CriarDto("0000001", "0000002");

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbrangenciaUesPorLoginEPerfilQuery>(), default))
                        .ReturnsAsync(new List<AbrangenciaUeRetorno>());

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(1, dto));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Turma_Fora_Da_Abrangencia()
        {
            var usuario = CriarUsuarioUE();
            var dto = CriarDto("0000001", "0000002", new[] { "TURMA01" });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbrangenciaUesPorLoginEPerfilQuery>(), default))
                        .ReturnsAsync(new List<AbrangenciaUeRetorno> { new AbrangenciaUeRetorno { Codigo = "0000002" } });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery>(), default))
                        .ReturnsAsync(new AbrangenciaCompactaVigenteRetornoEOLDTO
                        {
                            Abrangencia = new AbrangenciaCargoRetornoEolDTO { Abrangencia = Infra.Enumerados.Abrangencia.UE }
                        });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbrangenciaTurmaQuery>(), default))
                        .ReturnsAsync((AbrangenciaFiltroRetorno)null);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(1, dto));
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Se_Comando_Falhar()
        {
            var usuario = CriarUsuarioSME();
            var dto = CriarDto("0000001", "0000002");

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                        .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<AlterarComunicadoCommand>(), default))
                        .ReturnsAsync(false);

            var resultado = await useCase.Executar(1, dto);

            resultado.Should().Be("Erro na alteração do Comunicado");
        }

        #region Helpers

        private static ComunicadoAlterarDto CriarDto(string dre, string ue, string[] turmas = null)
        {
            return new ComunicadoAlterarDto
            {
                AnoLetivo = 2025,
                CodigoDre = dre,
                CodigoUe = ue,
                DataEnvio = DateTime.Today,
                Descricao = "Descricao teste",
                Titulo = "Titulo comunicado teste",
                Modalidades = new[] { 1 },
                Turmas = turmas ?? new string[0]
            };
        }

        private static Usuario CriarUsuarioSME()
        {
            var usuario = new Usuario
            {
                CodigoRf = "123",
                Login = "user",
                Nome = "Usuário SME",
                PerfilAtual = Perfis.PERFIL_ADMSME
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil { CodigoPerfil = Perfis.PERFIL_ADMSME, Tipo = TipoPerfil.SME }
            });
            return usuario;
        }

        private static Usuario CriarUsuarioDRE()
        {
            var usuario = new Usuario
            {
                CodigoRf = "123",
                Login = "user",
                Nome = "Usuário DRE",
                PerfilAtual = Perfis.PERFIL_ADMDRE
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil { CodigoPerfil = Perfis.PERFIL_ADMDRE, Tipo = TipoPerfil.DRE }
            });
            return usuario;
        }

        private static Usuario CriarUsuarioUE()
        {
            var usuario = new Usuario
            {
                CodigoRf = "123",
                Login = "user",
                Nome = "Usuário UE",
                PerfilAtual = Perfis.PERFIL_ADMUE
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil { CodigoPerfil = Perfis.PERFIL_ADMUE, Tipo = TipoPerfil.UE }
            });
            return usuario;
        }
        #endregion
    }
}
