using Moq;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Aplicacao.Interfaces.Consultas;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasEventoTipoTeste
    {
        private IConsultasEventoTipo consultasEventoTipo;
        private Mock<IRepositorioEventoTipo> repositorioEventoTipo;

        public ConsultasEventoTipoTeste()
        {
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
            consultasEventoTipo = new ConsultasEventoTipo(repositorioEventoTipo.Object);
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias")]
        public void Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencias()
        {
            Assert.Throws<ArgumentNullException>(() => consultasEventoTipo = new ConsultasEventoTipo(null));
        }

        [Fact(DisplayName = "Deve_Buscar_Evento_Tipo_Por_Id")]
        public void Deve_Buscar_Evento_Tipo_Por_Id()
        {
            repositorioEventoTipo.Setup(r => r.ObterPorId(It.IsAny<long>())).Returns(It.IsAny<EventoTipo>());

            consultasEventoTipo.ObtenhaPorId(1);
        }

        [Fact(DisplayName = "Deve_Listar_Tipos")]
        public void Deve_Listar_Tipos()
        {
            repositorioEventoTipo.Setup(r => r.ListarTipos(It.IsAny<EventoLocalOcorrencia>(), It.IsAny<EventoLetivo>(), It.IsAny<String>()))
                .Returns(It.IsAny<IList<EventoTipo>>());

            consultasEventoTipo.Listar(new FiltroEventoTipoDto());
            consultasEventoTipo.Listar(new FiltroEventoTipoDto { Letivo = EventoLetivo.Sim});
            consultasEventoTipo.Listar(new FiltroEventoTipoDto { LocalOcorrencia = EventoLocalOcorrencia.DRE});
            consultasEventoTipo.Listar(new FiltroEventoTipoDto { Descricao = "Teste"});
        }
    }
}
