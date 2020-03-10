using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasEventoTeste
    {
        private readonly ConsultasEvento consultaEventos;
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IRepositorioEventoTipo> repositorioEventoTipo;
        private readonly Mock<IServicoUsuario> servicoUsuario;

        public ConsultasEventoTeste()
        {
            repositorioEvento = new Mock<IRepositorioEvento>();
            var context = new DefaultHttpContext();
            var httpContextAcessorObj = new HttpContextAccessor();
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
            httpContextAcessorObj.HttpContext = context;
            servicoUsuario = new Mock<IServicoUsuario>();
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();

<<<<<<< HEAD
            consultaEventos = new ConsultasEvento(repositorioEvento.Object, new ContextoHttp(httpContextAcessorObj), repositorioEventoTipo.Object, servicoUsuario.Object);
=======
            consultaEventos = new ConsultasEvento(repositorioEvento.Object, new ContextoHttp(httpContextAcessorObj), servicoUsuario.Object, repositorioEventoTipo.Object);
>>>>>>> master
        }

        [Fact]
        public async Task DeveListarEventos()
        {
            var listaEventos = new List<Evento>
            {
                new Evento
                {
                    Id = 1
                }
            };
            var paginado = new PaginacaoResultadoDto<Evento>();
            paginado.Items = listaEventos;
            var usuario = new Usuario();
            repositorioEvento.Setup(c => c.Listar(It.IsAny<long?>(), It.IsAny<long?>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<Paginacao>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), usuario, It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(paginado));

            var eventosPaginados = await consultaEventos.Listar(new FiltroEventosDto());

            Assert.NotNull(eventosPaginados);
            Assert.Contains(eventosPaginados.Items, c => c.Id == 1);
            repositorioEvento.Verify(c => c.Listar(It.IsAny<long?>(), It.IsAny<long?>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<Paginacao>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), usuario, It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void DeveObterEvento()
        {
            repositorioEvento.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(new Evento
                {
                    Id = 1
                });
            var eventoDto = consultaEventos.ObterPorId(1);
            Assert.NotNull(eventoDto);
            Assert.Equal(1, eventoDto.Id);
            repositorioEvento.Verify(c => c.ObterPorId(It.IsAny<long>()), Times.Once);
        }
    }
}