using Moq;
using SME.SGP.Aplicacao.Comandos;
using SME.SGP.Aplicacao.Interfaces.Comandos;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosEventoTipoTeste
    {
        private IComandosEventoTipo comandosEventoTipo;
        private Mock<IRepositorioEventoTipo> repositorioEventoTipo;

        public ComandosEventoTipoTeste()
        {
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
            comandosEventoTipo = new ComandosEventoTipo(repositorioEventoTipo.Object);
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencia")]
        public void Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencia()
        {
            Assert.Throws<ArgumentNullException>(() => comandosEventoTipo = new ComandosEventoTipo(null));
        }

        [Fact(DisplayName = "Deve_Salvar_Evento_Tipo")]
        public void Deve_Salvar_Evento_Tipo()
        {

           repositorioEventoTipo.Setup(x => x.Salvar(It.IsAny<EventoTipo>())).Returns(1);

           var eventoTipoDto = ObterDto();

            comandosEventoTipo.Salvar(eventoTipoDto);
        }

        [Fact(DisplayName = "Deve_Remover_Evento_Tipo")]
        public void Deve_Remover_Evento_Tipo()
        {
            repositorioEventoTipo.Setup(x => x.Remover(It.IsAny<int>()));

            comandosEventoTipo.Remover(It.IsAny<long>());
        }

        private EventoTipoDto ObterDto()
        {
            return new EventoTipoDto
            {
                Ativo = true,
                Concomitancia = true,
                Dependencia = false,
                Letivo = EventoLetivo.Opcional,
                LocalOcorrencia = EventoLocalOcorrencia.Todos,
                Descricao = "Teste 123",
                TipoData = EventoTipoData.Unico
            };
        }
    }
}
