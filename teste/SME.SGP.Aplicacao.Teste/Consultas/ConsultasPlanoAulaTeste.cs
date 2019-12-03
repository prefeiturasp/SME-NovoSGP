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

            repositorioPlanoAula.Setup(a => a.ObterPlanoAulaPorAula(It.IsAny<long>()))
                .Returns(Task.FromResult(planoAula));

            // Objetivos Aula
            objetivos = new List<ObjetivoAprendizagemAula>()
            {
                new ObjetivoAprendizagemAula() 
                { 
                    PlanoAulaId = 1, 
                    ObjetivoAprendizagemPlanoId = 1, 
                    ObjetivoAprendizagemPlano = new ObjetivoAprendizagemPlano()
                    {
                        ComponenteCurricularId = 1,
                        ObjetivoAprendizagemJuremaId = 1,
                        PlanoId = 1
                    }
                }
            };

            consultasObjetivosAprendizagemAula.Setup(a => a.ObterObjetivosPlanoAula(It.IsAny<long>()))
                .Returns(Task.FromResult(objetivos));

            // Aula
            aula = new AulaConsultaDto()
            {
                Id = 1,
                DataAula = new DateTime(2019, 11, 1),
                DisciplinaId = "7",
                Quantidade = 3,
                UeId = "1",
                TurmaId = "1"
            };

            consultasAula.Setup(a => a.BuscarPorId(It.IsAny<long>()))
                .Returns(aula);

            // Plano anual
            var planoAnual = new PlanoAnualCompletoDto()
            {
                ObjetivosAprendizagem = new List<ObjetivoAprendizagemDto>()
                {
                    new ObjetivoAprendizagemDto() { Id = 1, Ano = 2019, Codigo = "1", Descricao = "Objetivo 1", IdComponenteCurricular = 1 }
                }
            };

            consultasPlanoAnual.Setup(a => a.ObterPorEscolaTurmaAnoEBimestre(It.IsAny<FiltroPlanoAnualDto>()))
                .Returns(Task.FromResult(planoAnual));
        }

        [Fact]
        public async void Deve_Obter_Por_Turma_Disciplina()
        {
            // ACT
            var planoAula = await consultasPlanoAula.ObterPlanoAulaPorAula(1);

            // ASSERT
            Assert.False(planoAula == null);

            Assert.True(planoAula.ObjetivosAprendizagemAula.Any());

            Assert.True(planoAula.QtdAulas > 0);
        }
    }
}
