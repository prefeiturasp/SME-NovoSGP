using Moq;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoEventoTeste
    {
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IRepositorioEventoTipo> repositorioEventoTipo;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendario;
        private readonly ServicoEvento servicoEvento;
        private readonly Mock<IServicoUsuario> servicoUsuario;

        public ServicoEventoTeste()
        {
            repositorioEvento = new Mock<IRepositorioEvento>();
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            servicoUsuario = new Mock<IServicoUsuario>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendario>();
            servicoEvento = new ServicoEvento(repositorioEvento.Object, repositorioEventoTipo.Object, repositorioPeriodoEscolar.Object, servicoUsuario.Object, repositorioTipoCalendario.Object);
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
    }
}