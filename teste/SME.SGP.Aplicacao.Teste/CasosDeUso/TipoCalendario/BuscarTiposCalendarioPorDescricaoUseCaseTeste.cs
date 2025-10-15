using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.TipoCalendario
{
    public class BuscarTiposCalendarioPorDescricaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IConsultasAbrangencia> consultasAbrangencia;
        private readonly BuscarTiposCalendarioPorDescricaoUseCase buscarTiposCalendarioPorDescricaoUseCaseTeste;

        public BuscarTiposCalendarioPorDescricaoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            consultasAbrangencia = new Mock<IConsultasAbrangencia>();
            buscarTiposCalendarioPorDescricaoUseCaseTeste = new BuscarTiposCalendarioPorDescricaoUseCase(mediator.Object, consultasAbrangencia.Object);
        }

        [Fact]
        public async Task Deve_Buscar_Lista_Tipo_Calendario_Pela_Busca()
        {
            var mockRetorno = new List<TipoCalendarioBuscaDto> {
                    new TipoCalendarioBuscaDto{
                        AnoLetivo = 2020,
                        Descricao = "2020 - Calendário Fundamental",
                        Id = 1,
                        Nome = "Calendário Fundamental",
                        Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                        Periodo = Periodo.Anual,
                        Migrado = false,
                        Situacao = true
                    },
                    new TipoCalendarioBuscaDto{
                        AnoLetivo = 2020,
                        Descricao = "2020 - Calendário Médio",
                        Id = 1,
                        Nome = "Calendário Médio",
                        Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                        Periodo = Periodo.Anual,
                        Migrado = false,
                        Situacao = true
                    },
                    new TipoCalendarioBuscaDto{
                        AnoLetivo = 2020,
                        Descricao = "2020 - Calendário EJA",
                        Id = 1,
                        Nome = "Calendário EJA",
                        Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                        Periodo = Periodo.Anual,
                        Migrado = false,
                        Situacao = true
                    }
                };

            mediator.Setup(a => a.Send(It.IsAny<ObterTiposCalendariosPorBuscaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetorno);

            var usuario = new Usuario();
            usuario.DefinirPerfis(new List<PrioridadePerfil>());
            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(usuario));

            var retorno = await buscarTiposCalendarioPorDescricaoUseCaseTeste.Executar("2020");

            mediator.Verify(x => x.Send(It.IsAny<ObterTiposCalendariosPorBuscaQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }

        [Fact]
        public async Task Deve_Buscar_Tipos_Calendario_Quando_Usuario_Tem_Permissao_UE()
        {
            var mockRetornoAbrangencia = new List<AbrangenciaFiltroRetorno>
            {
                new AbrangenciaFiltroRetorno { CodigoUe = "123456" },
                new AbrangenciaFiltroRetorno { CodigoUe = "789012" }
            };

            var mockRetornoModalidades = new List<Modalidade> { Modalidade.Fundamental, Modalidade.Medio };

            var mockRetornoAnosLetivos = new List<int> { 2020, 2021, 2022 };

            var mockRetornoTiposCalendario = new List<TipoCalendarioBuscaDto>
            {
                new TipoCalendarioBuscaDto
                {
                    AnoLetivo = 2021,
                    Descricao = "2021 - Calendário Fundamental",
                    Id = 1,
                    Nome = "Calendário Fundamental",
                    Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                    Periodo = Periodo.Anual,
                    Migrado = false,
                    Situacao = true
                }
            };

            var usuario = new Usuario();
            var perfisUE = new List<PrioridadePerfil>
            {
                new PrioridadePerfil { Tipo = TipoPerfil.UE, CodigoPerfil = Dominio.Perfis.PERFIL_DIRETOR }
            };
            usuario.DefinirPerfis(perfisUE);
            usuario.DefinirPerfilAtual(Dominio.Perfis.PERFIL_DIRETOR);

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediator.Setup(x => x.Send(It.IsAny<ObterAbrangenciaPorFiltroQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetornoAbrangencia);

            consultasAbrangencia.Setup(x => x.ObterAnosLetivos(It.IsAny<bool>(), It.IsAny<int>()))
                .ReturnsAsync(mockRetornoAnosLetivos);

            mediator.Setup(x => x.Send(It.IsAny<ObterModalidadesPorCodigosUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetornoModalidades);

            mediator.Setup(x => x.Send(It.IsAny<ObterTiposCalendariosPorAnosLetivoModalidadesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetornoTiposCalendario);

            var retorno = await buscarTiposCalendarioPorDescricaoUseCaseTeste.Executar("2021");

            mediator.Verify(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterAbrangenciaPorFiltroQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            consultasAbrangencia.Verify(x => x.ObterAnosLetivos(true, 0), Times.Once);
            consultasAbrangencia.Verify(x => x.ObterAnosLetivos(false, 0), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterModalidadesPorCodigosUeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterTiposCalendariosPorAnosLetivoModalidadesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterTiposCalendariosPorBuscaQuery>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.True(retorno.Any());
        }

        [Fact]
        public async Task Deve_Buscar_Tipos_Calendario_Quando_Usuario_Tem_Permissao_Professor()
        {
            var mockRetornoAbrangencia = new List<AbrangenciaFiltroRetorno>
            {
                new AbrangenciaFiltroRetorno { CodigoUe = "111111" }
            };

            var mockRetornoModalidades = new List<Modalidade> { Modalidade.EducacaoInfantil };

            var mockRetornoAnosLetivos = new List<int> { 2023 };

            var mockRetornoTiposCalendario = new List<TipoCalendarioBuscaDto>
            {
                new TipoCalendarioBuscaDto
                {
                    AnoLetivo = 2023,
                    Descricao = "2023 - Calendário Infantil",
                    Id = 2,
                    Nome = "Calendário Infantil",
                    Modalidade = ModalidadeTipoCalendario.Infantil,
                    Periodo = Periodo.Anual,
                    Migrado = false,
                    Situacao = true
                }
            };

            var usuario = new Usuario();
            var perfisProfessor = new List<PrioridadePerfil>
            {
                new PrioridadePerfil { Tipo = TipoPerfil.UE, CodigoPerfil = Dominio.Perfis.PERFIL_PROFESSOR }
            };
            usuario.DefinirPerfis(perfisProfessor);
            usuario.DefinirPerfilAtual(Dominio.Perfis.PERFIL_PROFESSOR);

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediator.Setup(x => x.Send(It.IsAny<ObterAbrangenciaPorFiltroQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetornoAbrangencia);

            consultasAbrangencia.Setup(x => x.ObterAnosLetivos(It.IsAny<bool>(), It.IsAny<int>()))
                .ReturnsAsync(mockRetornoAnosLetivos);

            mediator.Setup(x => x.Send(It.IsAny<ObterModalidadesPorCodigosUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetornoModalidades);

            mediator.Setup(x => x.Send(It.IsAny<ObterTiposCalendariosPorAnosLetivoModalidadesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetornoTiposCalendario);

            var retorno = await buscarTiposCalendarioPorDescricaoUseCaseTeste.Executar("Infantil");

            mediator.Verify(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterAbrangenciaPorFiltroQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            consultasAbrangencia.Verify(x => x.ObterAnosLetivos(true, 0), Times.Once);
            consultasAbrangencia.Verify(x => x.ObterAnosLetivos(false, 0), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterModalidadesPorCodigosUeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterTiposCalendariosPorAnosLetivoModalidadesQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }

        [Fact]
        public async Task Deve_Buscar_Tipos_Calendario_Quando_Usuario_Tem_Permissao_CCELP()
        {
            var mockRetornoAbrangencia = new List<AbrangenciaFiltroRetorno>
            {
                new AbrangenciaFiltroRetorno { CodigoUe = "555555" }
            };

            var mockRetornoModalidades = new List<Modalidade> { Modalidade.EJA };

            var mockRetornoAnosLetivos = new List<int> { 2024 };

            var mockRetornoTiposCalendario = new List<TipoCalendarioBuscaDto>
            {
                new TipoCalendarioBuscaDto
                {
                    AnoLetivo = 2024,
                    Descricao = "2024 - Calendário EJA",
                    Id = 3,
                    Nome = "Calendário EJA",
                    Modalidade = ModalidadeTipoCalendario.EJA,
                    Periodo = Periodo.Semestral,
                    Migrado = false,
                    Situacao = true
                }
            };

            var usuario = new Usuario();
            var perfisCCELP = new List<PrioridadePerfil>
            {
                new PrioridadePerfil { Tipo = TipoPerfil.UE, CodigoPerfil = Dominio.Perfis.PERFIL_COORDENADOR_POLO_FORMACAO }
            };
            usuario.DefinirPerfis(perfisCCELP);
            usuario.DefinirPerfilAtual(Dominio.Perfis.PERFIL_COORDENADOR_POLO_FORMACAO);

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediator.Setup(x => x.Send(It.IsAny<ObterAbrangenciaPorFiltroQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetornoAbrangencia);

            consultasAbrangencia.Setup(x => x.ObterAnosLetivos(It.IsAny<bool>(), It.IsAny<int>()))
                .ReturnsAsync(mockRetornoAnosLetivos);

            mediator.Setup(x => x.Send(It.IsAny<ObterModalidadesPorCodigosUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetornoModalidades);

            mediator.Setup(x => x.Send(It.IsAny<ObterTiposCalendariosPorAnosLetivoModalidadesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetornoTiposCalendario);

            var retorno = await buscarTiposCalendarioPorDescricaoUseCaseTeste.Executar("EJA");

            mediator.Verify(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterAbrangenciaPorFiltroQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            consultasAbrangencia.Verify(x => x.ObterAnosLetivos(true, 0), Times.Once);
            consultasAbrangencia.Verify(x => x.ObterAnosLetivos(false, 0), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterModalidadesPorCodigosUeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterTiposCalendariosPorAnosLetivoModalidadesQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }

        [Fact]
        public void Deve_Lancar_Exception_Quando_Mediator_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new BuscarTiposCalendarioPorDescricaoUseCase(null, consultasAbrangencia.Object));
        }

        [Fact]
        public void Deve_Lancar_Exception_Quando_ConsultasAbrangencia_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new BuscarTiposCalendarioPorDescricaoUseCase(mediator.Object, null));
        }
    }
}