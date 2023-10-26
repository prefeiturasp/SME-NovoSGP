﻿using MediatR;
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasNotasConceitosTeste
    {
        private readonly Mock<IConsultaAtividadeAvaliativa> consultaAtividadeAvaliativa;
        private readonly Mock<IConsultasFechamentoTurmaDisciplina> consultasFechamentoTurmaDisciplina;
        private readonly Mock<IConsultasDisciplina> consultasDisciplina;
        private readonly Mock<IConsultasPeriodoFechamento> consultasFechamento;
        private readonly ConsultasNotasConceitos consultasNotasConceito;
        private readonly Mock<IRepositorioAtividadeAvaliativa> repositorioAtividadeAvaliativa;
        private readonly Mock<IRepositorioFrequenciaConsulta> repositorioFrequencia;
        private readonly Mock<IRepositorioNotaParametro> repositorioNotaParametro;
        private readonly Mock<IRepositorioNotasConceitosConsulta> repositorioNotasConceitos;
        private readonly Mock<IRepositorioTipoCalendarioConsulta> repositorioTipoCalendario;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> repositorioFrequenciaAluno;
        private readonly Mock<IRepositorioConceitoConsulta> repositorioConceito;
        private readonly Mock<IServicoAluno> servicoAluno;
        private readonly Mock<IServicoDeNotasConceitos> servicoDeNotasConceitos;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IRepositorioAtividadeAvaliativaDisciplina> repositorioAtividadeAvaliativaDisciplina;
        private readonly Mock<IRepositorioPeriodoEscolarConsulta> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioParametrosSistema> repositorioParametrosSistema;
        private readonly Mock<IRepositorioTipoAvaliacao> repositorioTipoAvaliacao;
        private readonly Mock<IRepositorioTurma> repositorioTurma;
        private readonly Mock<IRepositorioUe> repositorioUe;
        private readonly Mock<IRepositorioDre> repositorioDre;
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IRepositorioAtividadeAvaliativaRegencia> repositorioAtividadeAvaliativaRegencia;
        private readonly Mock<IRepositorioComponenteCurricularConsulta> repositorioComponenteCurricular;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IConsultasTurma> consultasTurma;

        public ConsultasNotasConceitosTeste()
        {
            consultaAtividadeAvaliativa = new Mock<IConsultaAtividadeAvaliativa>();
            consultasFechamentoTurmaDisciplina = new Mock<IConsultasFechamentoTurmaDisciplina>();
            consultasDisciplina = new Mock<IConsultasDisciplina>();
            consultasFechamento = new Mock<IConsultasPeriodoFechamento>();
            servicoDeNotasConceitos = new Mock<IServicoDeNotasConceitos>();
            repositorioNotasConceitos = new Mock<IRepositorioNotasConceitosConsulta>();
            repositorioFrequencia = new Mock<IRepositorioFrequenciaConsulta>();
            repositorioFrequenciaAluno = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            servicoUsuario = new Mock<IServicoUsuario>();
            servicoAluno = new Mock<IServicoAluno>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendarioConsulta>();
            repositorioNotaParametro = new Mock<IRepositorioNotaParametro>();
            repositorioAtividadeAvaliativa = new Mock<IRepositorioAtividadeAvaliativa>();
            repositorioAtividadeAvaliativaDisciplina = new Mock<IRepositorioAtividadeAvaliativaDisciplina>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolarConsulta>();
            repositorioConceito = new Mock<IRepositorioConceitoConsulta>();
            repositorioParametrosSistema = new Mock<IRepositorioParametrosSistema>();
            repositorioTipoAvaliacao = new Mock<IRepositorioTipoAvaliacao>();
            repositorioTurma = new Mock<IRepositorioTurma>();
            repositorioUe = new Mock<IRepositorioUe>();
            repositorioDre = new Mock<IRepositorioDre>();
            repositorioEvento = new Mock<IRepositorioEvento>();
            repositorioAtividadeAvaliativaRegencia = new Mock<IRepositorioAtividadeAvaliativaRegencia>();
            repositorioComponenteCurricular = new Mock<IRepositorioComponenteCurricularConsulta>();
            mediator = new Mock<IMediator>();
            consultasTurma = new Mock<IConsultasTurma>();

            consultasNotasConceito = new ConsultasNotasConceitos(
                    consultaAtividadeAvaliativa.Object,
                    consultasFechamentoTurmaDisciplina.Object,
                    consultasDisciplina.Object,
                    consultasFechamento.Object,
                    servicoDeNotasConceitos.Object,
                    repositorioNotasConceitos.Object,
                    repositorioFrequencia.Object,
                    repositorioFrequenciaAluno.Object,
                    servicoUsuario.Object,
                    servicoAluno.Object,
                    repositorioTipoCalendario.Object,
                    repositorioNotaParametro.Object,
                    repositorioAtividadeAvaliativa.Object,
                    repositorioConceito.Object,
                    repositorioPeriodoEscolar.Object,
                    repositorioParametrosSistema.Object,
                    repositorioTipoAvaliacao.Object,
                    repositorioAtividadeAvaliativaDisciplina.Object,
                    repositorioTurma.Object,
                    repositorioUe.Object,
                    repositorioDre.Object,
                    repositorioEvento.Object,
                    repositorioAtividadeAvaliativaRegencia.Object,
                    repositorioComponenteCurricular.Object,
                    mediator.Object,
                    consultasTurma.Object);

            
        }

        [Theory]
        [InlineData(2.4, 0.5, 2.5)]
        [InlineData(2.6, 0.5, 3)]
        [InlineData(3, 0.5, 3)]
        [InlineData(3.5, 0.5, 3.5)]
        [InlineData(7.15, 0.6, 7.6)]
        [InlineData(8.05, 0.04, 9)]
        public async Task Deve_Arredondar_Nota(double nota, double arredondamento, double esperado)
        {
            repositorioAtividadeAvaliativa.Setup(a => a.ObterPorIdAsync(1)).ReturnsAsync(new Dominio.AtividadeAvaliativa());
            repositorioNotaParametro.Setup(a => a.ObterPorDataAvaliacao(It.IsAny<DateTime>())).ReturnsAsync(new Dominio.NotaParametro() { Incremento = arredondamento });

            var valorArredondado = await consultasNotasConceito.ObterValorArredondado(1, nota);

            Assert.True(esperado == valorArredondado);
        }
    }
}