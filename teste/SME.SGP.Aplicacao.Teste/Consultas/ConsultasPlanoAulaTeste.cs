using Moq;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasPlanoAulaTeste
    {
        private readonly Mock<IRepositorioPlanoAula> repositorioPlanoAula;
        private readonly Mock<IConsultasPlanoAnual> consultasPlanoAnual;
        private readonly Mock<IConsultasObjetivoAprendizagemAula> consultasObjetivosAprendizagemAula;
        private readonly Mock<IConsultasAula> consultasAula;
        private readonly ConsultasPlanoAula consultasPlanoAula;

        private PlanoAula planoAula;
        private IEnumerable<ObjetivoAprendizagemAula> objetivos;
        private AulaConsultaDto aula;

        public ConsultasPlanoAulaTeste()
        {
            repositorioPlanoAula = new Mock<IRepositorioPlanoAula>();
            consultasPlanoAnual = new Mock<IConsultasPlanoAnual>();
            consultasObjetivosAprendizagemAula = new Mock<IConsultasObjetivoAprendizagemAula>();
            consultasAula = new Mock<IConsultasAula>();

            consultasPlanoAula = new ConsultasPlanoAula(repositorioPlanoAula.Object,
                                                consultasPlanoAnual.Object,
                                                consultasObjetivosAprendizagemAula.Object,
                                                consultasAula.Object);
            Setup();
        }

        private void Setup()
        {
            // Plano Aula
            planoAula = new PlanoAula()
            {
                Id = 1,
                AulaId = 1,
                Descricao = "Teste plano aula",
            };

            repositorioPlanoAula.Setup(a => a.ObterPlanoAulaPorDataDisciplina(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(planoAula));

            // Objetivos Aula
            objetivos = new List<ObjetivoAprendizagemAula>()
            {
                new ObjetivoAprendizagemAula() { PlanoAulaId = 1, ObjetivoAprendizagemPlanoId = 1 }
            };

            consultasObjetivosAprendizagemAula.Setup(a => a.ObterObjetivosPlanoAula(It.IsAny<long>()))
                .Returns(Task.FromResult(objetivos));

            // Aula
            aula = new AulaConsultaDto()
            {
                Id = 1,
                DataAula = new DateTime(2019, 11, 1),
                DisciplinaId = "7",
                Quantidade = 3
            };

            consultasAula.Setup(a => a.ObterAulaDataTurmaDisciplina(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(aula));
        }

        [Fact]
        public async void Deve_Obter_Por_Turma_Disciplina()
        {
            // ACT
            var planoAula = await consultasPlanoAula.ObterPlanoAulaPorTurmaDisciplina(new DateTime(2019, 11, 1), 123, "7");

            // ASSERT
            Assert.False(planoAula == null);

            Assert.True(planoAula.ObjetivosAprendizagemAula.Any());

            Assert.True(planoAula.QtdAulas > 0);
        }
    }
}
