using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Contexto;
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
        private readonly Mock<IConsultasAbrangencia> consultasAbrangencia;

        //public ConsultasEventoTeste()
        //{
        //    repositorioEvento = new Mock<IRepositorioEvento>();
        //    repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
        //    var context = new DefaultHttpContext();
        //    var httpContextAcessorObj = new HttpContextAccessor();
        //    httpContextAcessorObj.HttpContext = context;
        //    servicoUsuario = new Mock<IServicoUsuario>();
        //    repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
        //    consultasAbrangencia = new Mock<IConsultasAbrangencia>();

        //    consultaEventos = new ConsultasEvento(repositorioEvento.Object, new ContextoHttp(httpContextAcessorObj), servicoUsuario.Object, repositorioEventoTipo.Object, consultasAbrangencia.Object);
        //}

        //[Fact]
        //public async Task DeveListarEventos()
        //{
        //    var listaEventos = new List<Evento>
        //    {
        //        new Evento
        //        {
        //            Id = 1
        //        }
        //    };
        //    var paginado = new PaginacaoResultadoDto<Evento>();
        //    paginado.Items = listaEventos;
        //    var usuario = new Usuario();
        //    repositorioEvento.Setup(c => c.Listar(It.IsAny<long?>(), It.IsAny<long?>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<Paginacao>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), usuario, It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
        //        .Returns(Task.FromResult(paginado));

        //    var eventosPaginados = await consultaEventos.Listar(new FiltroEventosDto());

        //    Assert.NotNull(eventosPaginados);
        //    Assert.Contains(eventosPaginados.Items, c => c.Id == 1);
        //    repositorioEvento.Verify(c => c.Listar(It.IsAny<long?>(), It.IsAny<long?>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<Paginacao>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), usuario, It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
        //}

        [Fact]
        public async Task DeveObterEvento()
        {
            repositorioEvento.Setup(c => c.ObterPorIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Evento
                {
                    Id = 1
                });

            repositorioEventoTipo.Setup(c => c.ObterPorIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EventoTipo
                {
                    Id = 1
                });

            var usuario = new Usuario()
            {
                CodigoRf = "123",
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>());

            servicoUsuario.Setup(a => a.ObterUsuarioLogado())
                .ReturnsAsync(usuario);


            var eventoDto = await consultaEventos.ObterPorId(1);

            Assert.NotNull(eventoDto);
            Assert.Equal(1, eventoDto.Id);
            repositorioEvento.Verify(c => c.ObterPorIdAsync(It.IsAny<long>()), Times.Once);
        }
    }
}