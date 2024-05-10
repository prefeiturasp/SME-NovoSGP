using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoEventoTeste
    {
        private readonly Mock<IComandosWorkflowAprovacao> comandosWorkflowAprovacao;
        private readonly Mock<IRepositorioAbrangencia> repositorioAbrangencia;
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IRepositorioEventoTipo> repositorioEventoTipo;
        private readonly Mock<IRepositorioFeriadoCalendario> repositorioFeriadoCalendario;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendario;
        private readonly Mock<IServicoDiaLetivo> servicoDiaLetivo;
        private readonly ServicoEvento servicoEvento;
        private readonly Mock<IServicoLog> servicoLog;
        private readonly Mock<IServicoNotificacao> servicoNotificacao;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public ServicoEventoTeste()
        {
            repositorioEvento = new Mock<IRepositorioEvento>();
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            servicoUsuario = new Mock<IServicoUsuario>();
            repositorioFeriadoCalendario = new Mock<IRepositorioFeriadoCalendario>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendario>();
            servicoLog = new Mock<IServicoLog>();
            servicoNotificacao = new Mock<IServicoNotificacao>();
            unitOfWork = new Mock<IUnitOfWork>();
            repositorioAbrangencia = new Mock<IRepositorioAbrangencia>();
            var mockConfiguration = new Mock<IConfiguration>();
            comandosWorkflowAprovacao = new Mock<IComandosWorkflowAprovacao>();
            servicoEvento = new ServicoEvento(repositorioEvento.Object,
                                              repositorioEventoTipo.Object,
                                              repositorioPeriodoEscolar.Object,
                                              servicoUsuario.Object,
                                              repositorioFeriadoCalendario.Object,
                                              repositorioTipoCalendario.Object,
                                              comandosWorkflowAprovacao.Object,
                                              repositorioAbrangencia.Object,
                                              mockConfiguration.Object,
                                              unitOfWork.Object,
                                              servicoNotificacao.Object,
                                              servicoLog.Object,
                                              servicoDiaLetivo.Object);
        }

        [Fact]
        public async Task DeveCriarEvento()
        {
            repositorioEventoTipo.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(new EventoTipo
                {
                    Id = 1,
                    TipoData = EventoTipoData.Unico,
                    LocalOcorrencia = EventoLocalOcorrencia.UE
                });

            repositorioTipoCalendario.Setup(c => c.ObterPorId(It.IsAny<long>()))
               .Returns(new TipoCalendario
               {
                   Id = 1,
               });

            var usuario = new Usuario();
            var perfilProfessor = new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse("40E1E074-37D6-E911-ABD6-F81654FE895D")
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                perfilProfessor
            });
            servicoUsuario.Setup(c => c.ObterUsuarioLogado())
                .Returns(Task.FromResult(usuario));

            await servicoEvento.Salvar(new Evento
            {
                TipoCalendarioId = 1,
                TipoEventoId = 1,
                DataInicio = DateTime.Now,
                Letivo = EventoLetivo.Sim,
                DreId = "123",
                UeId = "123"
            });
            repositorioEvento.Verify(c => c.Salvar(It.IsAny<Evento>()), Times.Once);
        }

        [Theory]
        [InlineData(TipoPerfil.DRE, false)]
        [InlineData(TipoPerfil.UE, false)]
        [InlineData(TipoPerfil.SME, true)]
        public async Task DeveCriarEventoEValidarParticularidadesSME_LiberacaoExcepcional(TipoPerfil tipoPerfil, bool sucesso)
        {
            //ARRANGE
            var tipoEvento = new EventoTipo
            {
                Id = 1,
                Codigo = (int)TipoEvento.LiberacaoExcepcional,
                TipoData = EventoTipoData.InicioFim,
                LocalOcorrencia = EventoLocalOcorrencia.UE
            };
            repositorioEventoTipo.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(tipoEvento);

            var tipoCalendario = new TipoCalendario
            {
                Id = 1
            };

            repositorioTipoCalendario.Setup(c => c.ObterPorId(It.IsAny<long>()))
               .Returns(tipoCalendario);

            var listaPeriodoEscolar = new List<PeriodoEscolar>() { new PeriodoEscolar() { PeriodoInicio = DateTime.Today, PeriodoFim = DateTime.Today.AddDays(7) } };

            repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendario(tipoCalendario.Id)).Returns(listaPeriodoEscolar);

            var usuario = new Usuario();

            var perfil = new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse("23A1E074-37D6-E911-ABD6-F81654FE895D"),
                Tipo = tipoPerfil
            };

            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                perfil
            });

            servicoUsuario.Setup(c => c.ObterUsuarioLogado())
                .Returns(Task.FromResult(usuario));

            var evento = new Evento
            {
                TipoCalendarioId = tipoCalendario.Id,
                DreId = "1",
                UeId = "2",
                TipoEvento = tipoEvento,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(2),
                Letivo = EventoLetivo.Sim
            };

            var ue = new AbrangenciaUeRetorno();
            repositorioAbrangencia.Setup(a => a.ObterUe(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(ue));

            //ASSERT
            try
            {
                await servicoEvento.Salvar(evento, true);
                Assert.True(true);
            }
            catch (Exception)
            {
                if (sucesso)
                    Assert.True(false);
            }
        }

        [Fact]
        public async Task DeveCriarEventoEValidarParticularidadesSME_LiberacaoExcepcional_ConfirmaData()
        {
            //ARRANGE
            var tipoEvento = new EventoTipo
            {
                Id = 1,
                Codigo = (int)TipoEvento.LiberacaoExcepcional,
                TipoData = EventoTipoData.InicioFim,
                LocalOcorrencia = EventoLocalOcorrencia.UE
            };
            repositorioEventoTipo.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(tipoEvento);

            var tipoCalendario = new TipoCalendario
            {
                Id = 1
            };

            repositorioTipoCalendario.Setup(c => c.ObterPorId(It.IsAny<long>()))
               .Returns(tipoCalendario);

            var listaPeriodoEscolar = new List<PeriodoEscolar>() { new PeriodoEscolar() { PeriodoInicio = DateTime.Today, PeriodoFim = DateTime.Today.AddDays(7) } };

            repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendario(tipoCalendario.Id)).Returns(listaPeriodoEscolar);

            var usuario = new Usuario();

            var perfil = new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse("23A1E074-37D6-E911-ABD6-F81654FE895D"),
                Tipo = TipoPerfil.DRE
            };

            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                perfil
            });

            servicoUsuario.Setup(c => c.ObterUsuarioLogado())
                .Returns(Task.FromResult(usuario));

            var evento = new Evento
            {
                TipoCalendarioId = tipoCalendario.Id,
                DreId = "1",
                UeId = "2",
                TipoEvento = tipoEvento,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(2),
                Letivo = EventoLetivo.Sim
            };

            //ASSERT
            await Assert.ThrowsAsync<NegocioException>(() => servicoEvento.Salvar(evento, false));
        }

        [Fact]
        public async Task DeveCriarEventoEValidarParticularidadesSME_LiberacaoExcepcional_Periodo_Alertar()
        {
            //ARRANGE
            var tipoEvento = new EventoTipo
            {
                Id = 1,
                Codigo = (int)TipoEvento.LiberacaoExcepcional,
                TipoData = EventoTipoData.InicioFim,
                LocalOcorrencia = EventoLocalOcorrencia.UE
            };
            repositorioEventoTipo.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(tipoEvento);

            var tipoCalendario = new TipoCalendario
            {
                Id = 1
            };

            repositorioTipoCalendario.Setup(c => c.ObterPorId(It.IsAny<long>()))
               .Returns(tipoCalendario);

            var listaPeriodoEscolar = new List<PeriodoEscolar>() { new PeriodoEscolar() { PeriodoInicio = DateTime.Today, PeriodoFim = DateTime.Today.AddDays(7) } };

            repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendario(tipoCalendario.Id)).Returns(listaPeriodoEscolar);

            var usuario = new Usuario();

            var perfil = new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse("23A1E074-37D6-E911-ABD6-F81654FE895D"),
                Tipo = TipoPerfil.DRE
            };

            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                perfil
            });

            servicoUsuario.Setup(c => c.ObterUsuarioLogado())
                .Returns(Task.FromResult(usuario));

            var evento = new Evento
            {
                TipoCalendarioId = tipoCalendario.Id,
                DreId = "1",
                UeId = "2",
                TipoEvento = tipoEvento,
                DataInicio = DateTime.Now.AddMonths(1),
                DataFim = DateTime.Now.AddMonths(1).AddDays(2),
                Letivo = EventoLetivo.Sim
            };

            //ASSERT
            await Assert.ThrowsAsync<NegocioException>(() => servicoEvento.Salvar(evento));
        }

        [Fact]
        public async Task DeveCriarEventoEValidarParticularidadesSME_OrganizacaoSME_PerfilSME()
        {
            //ARRANGE
            var tipoEvento = new EventoTipo
            {
                Id = 8,
                Codigo = 8,
                TipoData = EventoTipoData.InicioFim,
                LocalOcorrencia = EventoLocalOcorrencia.SME
            };
            repositorioEventoTipo.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(tipoEvento);

            repositorioTipoCalendario.Setup(c => c.ObterPorId(It.IsAny<long>()))
               .Returns(new TipoCalendario
               {
                   Id = 1
               });

            IEnumerable<Evento> listaEventosParaValidarPeriodo = new List<Evento>() { new Evento() { DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(1) } };

            repositorioEvento.Setup(a => a.ObterEventosPorTipoETipoCalendario(tipoEvento.Codigo, 8)).Returns(Task.FromResult(listaEventosParaValidarPeriodo));

            var usuario = new Usuario();
            var perfilProfessor = new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse("23E1E074-37D6-E911-ABD6-F81654FE895D"),
                Tipo = TipoPerfil.UE
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                perfilProfessor
            });

            servicoUsuario.Setup(c => c.ObterUsuarioLogado())
                .Returns(Task.FromResult(usuario));

            var evento = new Evento
            {
                TipoCalendarioId = 8,
                TipoEvento = tipoEvento,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(2),
                Letivo = EventoLetivo.Sim
            };

            //ASSERT
            await Assert.ThrowsAsync<NegocioException>(() => servicoEvento.Salvar(evento));
        }

        [Fact]
        public async Task DeveCriarEventoEValidarParticularidadesSME_OrganizacaoSME_Periodo()
        {
            //ARRANGE
            var tipoEvento = new EventoTipo
            {
                Id = 1,
                Codigo = 1,
                TipoData = EventoTipoData.InicioFim,
                LocalOcorrencia = EventoLocalOcorrencia.UE
            };
            repositorioEventoTipo.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(tipoEvento);

            repositorioTipoCalendario.Setup(c => c.ObterPorId(It.IsAny<long>()))
               .Returns(new TipoCalendario
               {
                   Id = 1
               });

            IEnumerable<Evento> listaEventosParaValidarPeriodo = new List<Evento>() {
                new Evento() { DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(1), Nome = "teste" },
                new Evento() { DataInicio = DateTime.Now.AddDays(1), DataFim = DateTime.Now.AddDays(3), Nome = "teste" }};

            repositorioEvento.Setup(a => a.ObterEventosPorTipoETipoCalendario((long)TipoEvento.OrganizacaoEscolar, 8)).Returns(Task.FromResult(listaEventosParaValidarPeriodo));

            var usuario = new Usuario();
            var perfilSme = new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse("23E1E074-37D6-E911-ABD6-F81654FE895D"),
                Tipo = TipoPerfil.UE
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                perfilSme
            });

            servicoUsuario.Setup(c => c.ObterUsuarioLogado())
                .Returns(Task.FromResult(usuario));

            var eventoQueNaoDevePassar = new Evento
            {
                TipoCalendarioId = 8,
                DreId = "1",
                UeId = "2",
                TipoEvento = tipoEvento,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(2),
                Letivo = EventoLetivo.Sim
            };

            var eventoQueDevePassar = new Evento
            {
                TipoCalendarioId = 8,
                DreId = "1",
                UeId = "2",
                TipoEvento = tipoEvento,
                DataInicio = DateTime.Now.AddDays(4),
                DataFim = DateTime.Now.AddDays(6),
                Letivo = EventoLetivo.Sim
            };

            //ASSERT
            Task task() => servicoEvento.Salvar(eventoQueNaoDevePassar);
            await Assert.ThrowsAsync<NegocioException>(task);

            await servicoEvento.Salvar(eventoQueDevePassar);

            repositorioEvento.Verify(c => c.Salvar(It.IsAny<Evento>()), Times.Once);
        }
    }
}