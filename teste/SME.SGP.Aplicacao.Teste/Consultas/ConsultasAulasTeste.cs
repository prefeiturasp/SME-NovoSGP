﻿using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasAulasTeste
    {
        private readonly ConsultasAula consultas;
        private readonly Mock<IConsultasFrequencia> consultasFrequencia;
        private readonly Mock<IConsultasPeriodoEscolar> consultasPeriodoEscolar;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly Mock<IRepositorioPlanoAula> repositorioPlanoAula;
        private readonly Mock<IServicoEOL> servicoEol;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private Mock<IConsultasDisciplina> consultasDisciplinas;

        public ConsultasAulasTeste()
        {
            repositorioAula = new Mock<IRepositorioAula>();
            servicoUsuario = new Mock<IServicoUsuario>();
            consultasPeriodoEscolar = new Mock<IConsultasPeriodoEscolar>();
            repositorioPlanoAula = new Mock<IRepositorioPlanoAula>();
            consultasFrequencia = new Mock<IConsultasFrequencia>();
            servicoEol = new Mock<IServicoEOL>();
            consultasDisciplinas = new Mock<IConsultasDisciplina>();
            consultas = new ConsultasAula(repositorioAula.Object, consultasPeriodoEscolar.Object, consultasFrequencia.Object, repositorioPlanoAula.Object, servicoUsuario.Object, servicoEol.Object, consultasDisciplinas.Object);

            Setup();
        }

        [Fact]
        public async Task DeveObterAulaPorId()
        {
            var aulaDto = await consultas.BuscarPorId(1);

            Assert.NotNull(aulaDto);
            Assert.True(aulaDto.Id == 1);
            Assert.True(aulaDto.Quantidade == 3);
        }

        [Fact]
        public async Task DeveObterQuantidadeAulas()
        {
            var qtd = await consultas.ObterQuantidadeAulasTurmaSemanaProfessor("123", "7", 3, null);

            Assert.True(qtd == 4);
        }

        private void Setup()
        {
            // Mock para testar o metodo ObterPorId
            var aula = new Aula()
            {
                Id = 1,
                DataAula = new DateTime(2019, 11, 15),
                ProfessorRf = "123",
                Quantidade = 3
            };

            repositorioAula.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(aula);

            // Mock das aulas por turma e disciplina
            IEnumerable<AulasPorTurmaDisciplinaDto> aulas = new List<AulasPorTurmaDisciplinaDto>()
            {
                new AulasPorTurmaDisciplinaDto() { ProfessorId = 1, Quantidade = 1, DataAula = new System.DateTime(2019,11,12) },
                new AulasPorTurmaDisciplinaDto() { ProfessorId = 1, Quantidade = 3, DataAula = new System.DateTime(2019,11,15) },
            };

            repositorioAula.Setup(c => c.ObterAulasTurmaDisciplinaSemanaProfessor(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), null))
                .Returns(Task.FromResult(aulas));
        }
    }
}