using Moq;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosEventoTipoTeste
    {
        private readonly Mock<IRepositorioEventoTipo> repositorioEventoTipo;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private IComandosEventoTipo comandosEventoTipo;

        public ComandosEventoTipoTeste()
        {
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
            unitOfWork = new Mock<IUnitOfWork>();
            comandosEventoTipo = new ComandosEventoTipo(repositorioEventoTipo.Object, unitOfWork.Object);
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencia")]
        public void Deve_Disparar_Excecao_Ao_Instanciar_Sem_Dependencia()
        {
            Assert.Throws<ArgumentNullException>(() => comandosEventoTipo = new ComandosEventoTipo(null, unitOfWork.Object));
            Assert.Throws<ArgumentNullException>(() => comandosEventoTipo = new ComandosEventoTipo(repositorioEventoTipo.Object, null));
        }

        [Fact(DisplayName = "Deve_Disparer_Excecao_Ao_Remover_Evento_Inexistente")]
        public void Deve_Disparer_Excecao_Ao_Remover_Evento_Inexistente()
        {
            repositorioEventoTipo.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns<EventoTipo>(null);

            Assert.Throws<NegocioException>(() => comandosEventoTipo.Remover(new List<long> { 1, 2, 3 }));
        }

        [Fact(DisplayName = "Deve_Remover_Evento_Tipo")]
        public void Deve_Remover_Evento_Tipo()
        {
            repositorioEventoTipo.Setup(x => x.ObterPorId(It.IsAny<long>())).Returns(new EventoTipo());
            repositorioEventoTipo.Setup(x => x.Salvar(It.IsAny<EventoTipo>()));

            comandosEventoTipo.Remover(new List<long> { 1, 2, 3 });
        }

        [Fact(DisplayName = "Deve_Salvar_Evento_Tipo")]
        public void Deve_Salvar_Evento_Tipo()
        {
            repositorioEventoTipo.Setup(x => x.Salvar(It.IsAny<EventoTipo>())).Returns(1);

            var eventoTipoDto = ObterDto();

            comandosEventoTipo.Salvar(eventoTipoDto);
        }

        private EventoTipoInclusaoDto ObterDto()
        {
            return new EventoTipoInclusaoDto
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