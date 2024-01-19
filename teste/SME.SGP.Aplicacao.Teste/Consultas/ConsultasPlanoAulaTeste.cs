using MediatR;
using Moq;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasPlanoAulaTeste
    {
        private readonly Mock<IConsultasAula> consultasAula;
        private readonly Mock<IConsultasObjetivoAprendizagemAula> consultasObjetivosAprendizagemAula;
        private readonly Mock<IConsultasPeriodoEscolar> consultasPeriodoEscolar;
        private readonly Mock<IConsultasPlanoAnual> consultasPlanoAnual;
        private readonly ConsultasPlanoAula consultasPlanoAula;
        private readonly Mock<IRepositorioAtividadeAvaliativa> repositorioAtividadeAvaliativa;
        private readonly Mock<IRepositorioPlanoAula> repositorioPlanoAula;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IMediator> mediator;
        private AulaConsultaDto aula;
        private IEnumerable<ObjetivoAprendizagemAula> objetivos;
        private PlanoAula planoAula;

        public ConsultasPlanoAulaTeste()
        {
            repositorioPlanoAula = new Mock<IRepositorioPlanoAula>();
            repositorioAtividadeAvaliativa = new Mock<IRepositorioAtividadeAvaliativa>();
            consultasPlanoAnual = new Mock<IConsultasPlanoAnual>();
            consultasObjetivosAprendizagemAula = new Mock<IConsultasObjetivoAprendizagemAula>();
            consultasPeriodoEscolar = new Mock<IConsultasPeriodoEscolar>();
            consultasAula = new Mock<IConsultasAula>();
            servicoUsuario = new Mock<IServicoUsuario>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            mediator = new Mock<IMediator>();

            consultasPlanoAula = new ConsultasPlanoAula(repositorioPlanoAula.Object,
                                                servicoUsuario.Object,
                                                mediator.Object);
            Setup();
        }

        //[Fact]
        //public async void Deve_Obter_Por_Turma_Disciplina()
        //{
        //    // ACT
        //    var planoAula = await consultasPlanoAula.ObterPlanoAulaPorAula(1);

        //    // ASSERT
        //    Assert.False(planoAula.EhNulo());

        //    Assert.True(planoAula.ObjetivosAprendizagemAula.Any());

        //    Assert.True(planoAula.QtdAulas > 0);
        //}

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
                    ObjetivoAprendizagemId = 1,
                    ObjetivoAprendizagem = new ObjetivoAprendizagem()
                    {
                        ComponenteCurricularId = 1,
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

            consultasAula.Setup(a => a.BuscarPorId(It.IsAny<long>()).Result)
                .Returns(aula);

            // Plano anual
            var planoAnual = new PlanoAnualCompletoDto()
            {
                ObjetivosAprendizagem = new List<ObjetivoAprendizagemDto>()
                {
                    new ObjetivoAprendizagemDto() { Id = 1, Ano = "2019", Codigo = "1", Descricao = "Objetivo 1", IdComponenteCurricular = 1 }
                }
            };

            consultasPlanoAnual.Setup(a => a.ObterPorEscolaTurmaAnoEBimestre(It.IsAny<FiltroPlanoAnualDto>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(planoAnual));
        }
    }
}