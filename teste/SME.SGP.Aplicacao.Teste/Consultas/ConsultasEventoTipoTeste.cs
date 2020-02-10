using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasEventoTipoTeste
    {
        private readonly Mock<IHttpContextAccessor> httpContext;
        private readonly Mock<IRepositorioEventoTipo> repositorioEventoTipo;
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private IConsultasEventoTipo consultasEventoTipo;

        public ConsultasEventoTipoTeste()
        {
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
            httpContext = new Mock<IHttpContextAccessor>();
            repositorioEvento = new Mock<IRepositorioEvento>();
            consultasEventoTipo = new ConsultasEventoTipo(repositorioEventoTipo.Object, new ContextoHttp(httpContext.Object), repositorioEvento.Object, servicoUsuario.Object);
        }

        [Fact(DisplayName = "Deve_Buscar_Evento_Tipo_Por_Id")]
        public void Deve_Buscar_Evento_Tipo_Por_Id()
        {
            repositorioEventoTipo.Setup(r => r.ObterPorId(It.IsAny<long>())).Returns(It.IsAny<EventoTipo>());
            repositorioEvento.Setup(r => r.ExisteEventoPorEventoTipoId(It.IsAny<long>())).Returns(false);

            consultasEventoTipo.ObterPorId(1);
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias")]
        public void Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias()
        {
            Assert.Throws<ArgumentNullException>(() => consultasEventoTipo = new ConsultasEventoTipo(null, new ContextoHttp(httpContext.Object), repositorioEvento.Object, servicoUsuario.Object));
            Assert.Throws<ArgumentNullException>(() => consultasEventoTipo = new ConsultasEventoTipo(repositorioEventoTipo.Object, null, repositorioEvento.Object, servicoUsuario.Object));
        }

        [Fact(DisplayName = "Deve_Listar_Tipos")]
        public void Deve_Listar_Tipos()
        {
            repositorioEventoTipo.Setup(r => r.ListarTipos(It.IsAny<EventoLocalOcorrencia>(), It.IsAny<EventoLetivo>(), It.IsAny<String>(), It.IsAny<Paginacao>()))
                .Returns(It.IsAny<Task<PaginacaoResultadoDto<EventoTipo>>>());
            repositorioEvento.Setup(r => r.ExisteEventoPorEventoTipoId(It.IsAny<long>())).Returns(false);

            consultasEventoTipo.Listar(new FiltroEventoTipoDto());
            consultasEventoTipo.Listar(new FiltroEventoTipoDto { Letivo = EventoLetivo.Sim });
            consultasEventoTipo.Listar(new FiltroEventoTipoDto { LocalOcorrencia = EventoLocalOcorrencia.DRE });
            consultasEventoTipo.Listar(new FiltroEventoTipoDto { Descricao = "Teste" });
        }
    }
}