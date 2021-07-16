using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class TrataSincronizacaoInstitucionalTurmaCommandHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioTurma> repositorioTurma;
        private readonly TrataSincronizacaoInstitucionalTurmaCommandHandler trataSincronizacaoInstitucionalTurmaCommandHandler;

        public TrataSincronizacaoInstitucionalTurmaCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioTurma = new Mock<IRepositorioTurma>();
            trataSincronizacaoInstitucionalTurmaCommandHandler = new TrataSincronizacaoInstitucionalTurmaCommandHandler(repositorioTurma.Object, mediator.Object);
        }

        [Fact(DisplayName = "Valida o tratamento de turma concluída para atualizar para histórica")]
        public async Task Tratar_Turma_Concluida_Deve_Atualizar_Para_Historica()
        {
            //Arrange
            var turmaEol = new TurmaParaSyncInstitucionalDto()
            {
                Ano = "4",
                AnoLetivo = 2021,
                Codigo = 2258053,
                TipoTurma = 1,
                CodigoModalidade = Dominio.Modalidade.EducacaoInfantil,
                NomeTurma = "4D",
                Semestre = 0,
                DuracaoTurno = 10,
                TipoTurno = 6,
                DataFim = null,
                EnsinoEspecial = false,
                SerieEnsino = "MINI GRUPO II",
                DataInicioTurma = DateTime.Parse("2021-02-10"),
                Extinta = false,
                Situacao = "C",
                UeCodigo = "094765",
                DataAtualizacao = DateTime.Parse("2021-02-10"),
                DataStatusTurmaEscola = DateTime.Parse("2021-02-10"),
            };
            mediator.Setup(a => a.Send(It.IsAny<ObterUeComDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Ue() { Id = 1 });

            var turmaSgp = new Turma()
            {
                Ano = "4",
                AnoLetivo = 2021,
                CodigoTurma = "2258053",
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Dominio.Modalidade.EducacaoInfantil,
                Nome = "4D",
                Semestre = 0,
                QuantidadeDuracaoAula = 10,
                TipoTurno = 6,
                DataFim = null,
                EnsinoEspecial = false,
                SerieEnsino = "MINI GRUPO II",
                DataInicio = DateTime.Parse("2021-02-10"),
                Extinta = false,               
                DataAtualizacao = DateTime.Parse("2021-02-10"),                
            };

            //Act  
            await trataSincronizacaoInstitucionalTurmaCommandHandler.Handle(new TrataSincronizacaoInstitucionalTurmaCommand(turmaEol, turmaSgp), new CancellationToken());

            //Assert
            repositorioTurma.Verify(r => r.AtualizarTurmaParaHistorica(turmaEol.Codigo.ToString()), Times.Once);
        }

        [Fact(DisplayName = "Valida o tratamento de turma para inserir na base")]
        public async Task Tratar_Turma_Deve_Incluir()
        {
            //Arrange
            var turmaEol = new TurmaParaSyncInstitucionalDto()
            {
                Ano = "4",
                AnoLetivo = 2021,
                Codigo = 2258053,
                TipoTurma = 1,
                CodigoModalidade = Dominio.Modalidade.EducacaoInfantil,
                NomeTurma = "4D",
                Semestre = 0,
                DuracaoTurno = 10,
                TipoTurno = 6,
                DataFim = null,
                EnsinoEspecial = false,
                SerieEnsino = "MINI GRUPO II",
                DataInicioTurma = DateTime.Parse("2021-02-10"),
                Extinta = false,
                Situacao = "O",
                UeCodigo = "094765",
                DataAtualizacao = DateTime.Parse("2021-02-10"),
                DataStatusTurmaEscola = DateTime.Parse("2021-02-10"),
            };            

            mediator.Setup(a => a.Send(It.IsAny<ObterUeComDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Ue() { Id = 1 });

            //Act  
            await trataSincronizacaoInstitucionalTurmaCommandHandler.Handle(new TrataSincronizacaoInstitucionalTurmaCommand(turmaEol, null), new CancellationToken());

            //Assert
            repositorioTurma.Verify(r => r.SalvarAsync(turmaEol, 1), Times.Once);
        }

        [Fact(DisplayName = "Valida o tratamento de turma para Atualizar na base")]
        public async Task Tratar_Turma_Deve_Atualizar()
        {
            //Arrange
            var turmaEol = new TurmaParaSyncInstitucionalDto()
            {
                Ano = "4",
                AnoLetivo = 2021,
                Codigo = 2258053,
                TipoTurma = 1,
                CodigoModalidade = Dominio.Modalidade.EducacaoInfantil,
                NomeTurma = "4D",
                Semestre = 0,
                DuracaoTurno = 10,
                TipoTurno = 6,
                DataFim = null,
                EnsinoEspecial = false,
                SerieEnsino = "MINI GRUPO II",
                DataInicioTurma = DateTime.Parse("2021-02-10"),
                Extinta = false,
                Situacao = "O",
                UeCodigo = "094765",
                DataAtualizacao = DateTime.Parse("2021-02-10"),
                DataStatusTurmaEscola = DateTime.Parse("2021-02-10"),
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterUeComDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Ue() { Id = 1 });           

            var turmaSgp = new Turma()
            {
                Ano = "4",
                AnoLetivo = 2021,
                CodigoTurma = "2258053",
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Dominio.Modalidade.EducacaoInfantil,
                Nome = "4DTeste",
                Semestre = 0,
                QuantidadeDuracaoAula = 10,
                TipoTurno = 6,
                DataFim = null,
                EnsinoEspecial = false,
                SerieEnsino = "MINI GRUPO II Teste",
                DataInicio = DateTime.Parse("2021-02-10"),
                Extinta = false,
                DataAtualizacao = DateTime.Parse("2021-02-10"),
            };

            //Act  
            await trataSincronizacaoInstitucionalTurmaCommandHandler.Handle(new TrataSincronizacaoInstitucionalTurmaCommand(turmaEol, turmaSgp), new CancellationToken());

            //Assert
            repositorioTurma.Verify(r => r.AtualizarTurmaSincronizacaoInstitucionalAsync(turmaEol, false), Times.Once);
        }

        //[Fact(DisplayName = "Valida o tratamento de turma extinta antes da criação do cadendario para excluír da base")]
        //public async Task Tratar_Turma_Extinta_Sem_TipoCalencarioId_Deve_Exluir()
        //{
        //    //Arrange
        //    var turmaEol = new TurmaParaSyncInstitucionalDto()
        //    {
        //        Ano = "4",
        //        AnoLetivo = 2021,
        //        Codigo = 2258053,
        //        TipoTurma = 1,
        //        CodigoModalidade = Dominio.Modalidade.InfantilPreEscola,
        //        NomeTurma = "4D",
        //        Semestre = 0,
        //        DuracaoTurno = 10,
        //        TipoTurno = 6,
        //        DataFim = null,
        //        EnsinoEspecial = false,
        //        SerieEnsino = "MINI GRUPO II",
        //        DataInicioTurma = DateTime.Parse("2021-02-10"),
        //        Extinta = true,
        //        Situacao = "E",
        //        UeCodigo = "400240",
        //        DataAtualizacao = DateTime.Parse("2021-02-10"),
        //        DataStatusTurmaEscola = DateTime.Parse("2021-02-10"),
        //    };

        //    var turmaSgp = new Turma()
        //    {
        //        Ano = "4",
        //        AnoLetivo = 2021,
        //        CodigoTurma = "2258053",
        //        TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
        //        ModalidadeCodigo = Dominio.Modalidade.InfantilPreEscola,
        //        Nome = "4D",
        //        Semestre = 0,
        //        QuantidadeDuracaoAula = 10,
        //        TipoTurno = 6,
        //        DataFim = null,
        //        EnsinoEspecial = false,
        //        SerieEnsino = "MINI GRUPO II",
        //        DataInicio = DateTime.Parse("2021-02-10"),
        //        Extinta = false,
        //        DataAtualizacao = DateTime.Parse("2021-02-10"),
        //    };

        //    mediator.Setup(a => a.Send(It.IsAny<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(0);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new List<PeriodoEscolar>());

        //    //Act  
        //    var retorno = await trataSincronizacaoInstitucionalTurmaCommandHandler.Handle(new TrataSincronizacaoInstitucionalTurmaCommand(turmaEol, turmaSgp), new CancellationToken());

        //    //Assert
        //    repositorioTurma.Verify(r => r.ExcluirTurmaExtintaAsync(turmaEol.Codigo.ToString(), 1),  Times.Once);
        //    Assert.True(retorno);
        //}

        //[Fact(DisplayName = "Valida o tratamento de turma extinta antes do inicio do ano letivo para excluír da base")]
        //public async Task Tratar_Turma_Extinta_Antes_Inicio_Ano_Letivo_Deve_Exluir()
        //{
        //    //Arrange
        //    var turmaEol = new TurmaParaSyncInstitucionalDto()
        //    {
        //        Ano = "4",
        //        AnoLetivo = 2021,
        //        Codigo = 2258053,
        //        TipoTurma = 1,                
        //        CodigoModalidade = Dominio.Modalidade.InfantilPreEscola,
        //        NomeTurma = "4D",
        //        Semestre = 0,
        //        DuracaoTurno = 10,
        //        TipoTurno = 6,
        //        DataFim = null,
        //        EnsinoEspecial = false,                
        //        SerieEnsino = "MINI GRUPO II",
        //        DataInicioTurma = DateTime.Parse("2021-02-10"),
        //        Extinta = true,
        //        Situacao = "E",
        //        UeCodigo = "400240",
        //        DataAtualizacao = DateTime.Parse("2021-02-10"),
        //        DataStatusTurmaEscola = DateTime.Parse("2021-02-10"),
        //    };

        //    var turmaSgp = new Turma()
        //    {
        //        Ano = "4",
        //        AnoLetivo = 2021,
        //        CodigoTurma = "2258053",
        //        TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
        //        ModalidadeCodigo = Dominio.Modalidade.InfantilPreEscola,
        //        Nome = "4D",
        //        Semestre = 0,
        //        QuantidadeDuracaoAula = 10,
        //        TipoTurno = 6,
        //        DataFim = null,
        //        EnsinoEspecial = false,
        //        SerieEnsino = "MINI GRUPO II",
        //        DataInicio = DateTime.Parse("2021-02-10"),
        //        Extinta = false,
        //        DataAtualizacao = DateTime.Parse("2021-02-10"),
        //    };

        //    mediator.Setup(a => a.Send(It.IsAny<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(1);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new List<PeriodoEscolar>()
        //        {
        //            new PeriodoEscolar
        //            {
        //                Bimestre = 1,
        //                Migrado = false,
        //                PeriodoInicio = DateTime.Parse("2021-02-20"),
        //                PeriodoFim = DateTime.Parse("2021-04-20"),
        //            },
        //            new PeriodoEscolar
        //            {
        //                Bimestre = 2,
        //                Migrado = false,
        //                PeriodoInicio = DateTime.Parse("2021-04-20"),
        //                PeriodoFim = DateTime.Parse("2021-06-20"),
        //            }
        //        }); ;

        //    //Act  
        //    var retorno = await trataSincronizacaoInstitucionalTurmaCommandHandler.Handle(new TrataSincronizacaoInstitucionalTurmaCommand(turmaEol, turmaSgp), new CancellationToken());

        //    //Assert
        //    repositorioTurma.Verify(r => r.ExcluirTurmaExtintaAsync(turmaEol.Codigo.ToString(), 1), Times.Once);
        //    Assert.True(retorno);
        //}

        [Fact(DisplayName = "Valida o tratamento de turma extinta após o inicio do ano letivo para marcar como histórica")]
        public async Task Tratar_Turma_Extinta_Apos_Inicio_Ano_Letivo_Deve_Marcar_Historica()
        {
            //Arrange
            var turmaEol = new TurmaParaSyncInstitucionalDto()
            {
                Ano = "4",
                AnoLetivo = 2021,
                Codigo = 2340746,
                TipoTurma = 1,                
                CodigoModalidade = Dominio.Modalidade.EducacaoInfantil,
                NomeTurma = "4D",
                Semestre = 0,
                DuracaoTurno = 10,
                TipoTurno = 6,
                DataFim = null,
                EnsinoEspecial = false,                
                SerieEnsino = "MINI GRUPO II",
                DataInicioTurma = DateTime.Parse("2021-02-10"),
                Extinta = true,
                Situacao = "E",
                UeCodigo = "400240",
                DataAtualizacao = DateTime.Parse("2021-02-10"),
                DataStatusTurmaEscola = DateTime.Parse("2021-02-10"),
            };

            var turmaSgp = new Turma()
            {
                Ano = "4",
                AnoLetivo = 2021,
                CodigoTurma = "2258053",
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Dominio.Modalidade.EducacaoInfantil,
                Nome = "4D",
                Semestre = 0,
                QuantidadeDuracaoAula = 10,
                TipoTurno = 6,
                DataFim = null,
                EnsinoEspecial = false,
                SerieEnsino = "MINI GRUPO II",
                DataInicio = DateTime.Parse("2021-02-10"),
                Extinta = false,
                DataAtualizacao = DateTime.Parse("2021-02-10"),
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(a => a.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>()
                {
                    new PeriodoEscolar
                    {
                        Bimestre = 1,
                        Migrado = false,
                        PeriodoInicio = DateTime.Parse("2021-02-01"),
                        PeriodoFim = DateTime.Parse("2021-04-20"),
                    },
                    new PeriodoEscolar
                    {
                        Bimestre = 2,
                        Migrado = false,
                        PeriodoInicio = DateTime.Parse("2021-04-20"),
                        PeriodoFim = DateTime.Parse("2021-06-20"),
                    }
                }); ;

            //Act  
            await trataSincronizacaoInstitucionalTurmaCommandHandler.Handle(new TrataSincronizacaoInstitucionalTurmaCommand(turmaEol, turmaSgp), new CancellationToken());

            //Assert
            repositorioTurma.Verify(r => r.AtualizarTurmaSincronizacaoInstitucionalAsync(turmaEol, true), Times.Once);
        }
    }
}
