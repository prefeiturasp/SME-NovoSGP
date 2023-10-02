using Elastic.Apm.Api;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Frequencia.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_regularizar_frequencia_presencas_maior_quantidade_aulas : FrequenciaTesteBase
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;
        private readonly IRepositorioCache repositorioCache;
        private readonly IMediator mediator;

        public Ao_regularizar_frequencia_presencas_maior_quantidade_aulas(CollectionFixture collectionFixture)
            : base(collectionFixture)
        {
            repositorioRegistroFrequenciaAluno = ServiceProvider.GetService<IRepositorioRegistroFrequenciaAluno>();
            repositorioCache = ServiceProvider.GetService<IRepositorioCache>();
            mediator = ServiceProvider.GetService<IMediator>();
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFakePresencasMaiorTotalAulas), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandHandlerFakePresencasMaiorTotalAulas), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Frequência - Regularizar frequência de aluno ativo com presenças maior que a quantidade de aulas, excluindo registros frequência inválidos e acionando o cálculo")]
        public async Task Regularizar_frequencia_aluno_ativo_presencas_maior_quantidade_aulas_excluir_registros_acionar_calculo()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();

            (DateTime inicioPeriodo, DateTime fimPeriodo) = (new DateTime(dataAtual.Year, dataAtual.Month, 1), 
                new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month)));

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = dataAtual.Year,
                Nome = "tipo cal teste",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                CriadoEm = dataAtual
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 1,
                CodigoAluno = "1",
                TurmaId = "1",
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                DisciplinaId = "1",
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                TotalCompensacoes = 0,
                TotalPresencas = 2,
                TotalRemotos = 0,
                PeriodoEscolarId = 1
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 2,
                CodigoAluno = "1",
                TurmaId = "1",
                Tipo = TipoFrequenciaAluno.Geral,
                DisciplinaId = string.Empty,
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                TotalCompensacoes = 0,
                TotalPresencas = 2,
                TotalRemotos = 0,
                PeriodoEscolarId = 1
            });

            await InserirNaBase(new Dre()
            {
                Id = 1,
                CodigoDre = "1",
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                UeId = 1,
                DataAtualizacao = dataAtual,
                TipoTurma = TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = dataAtual.Year
            });

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 1,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1",
                Quantidade = 1,
                DataAula = new DateTime(dataAtual.Year, dataAtual.Month, 1),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 2,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1",
                Quantidade = 1,
                DataAula = new DateTime(dataAtual.Year, dataAtual.Month, 11),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequencia()
            {
                Id = 1,
                AulaId = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequencia()
            {
                Id = 2,
                AulaId = 2,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Id = 1,
                Valor = 1,
                CodigoAluno = "1",
                NumeroAula = 1,
                RegistroFrequenciaId = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                AulaId = 1
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Id = 1,
                Valor = 1,
                CodigoAluno = "1",
                NumeroAula = 1,
                RegistroFrequenciaId = 2,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                AulaId = 2
            });

            var commandHandler = new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommandHandler(repositorioRegistroFrequenciaAluno, repositorioCache, mediator);

            var resultado =  await commandHandler
                .Handle(new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand(1), It.IsAny<CancellationToken>());

            resultado.ShouldBeTrue();

            var registrosFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();

            registrosFrequenciaAluno.ShouldNotBeEmpty();
            registrosFrequenciaAluno.Count(x => x.CodigoAluno == "1").ShouldBe(1);
            registrosFrequenciaAluno.SingleOrDefault(x => x.AulaId == 2).ShouldNotBeNull();

            var frequenciasAluno = ObterTodos<Dominio.FrequenciaAluno>();

            frequenciasAluno.ShouldNotBeEmpty();
            frequenciasAluno.Count(x => x.CodigoAluno == "1" && x.TurmaId == "1").ShouldBe(2);

            var frequenciaPorDisciplina = frequenciasAluno.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina);
            frequenciaPorDisciplina.ShouldNotBeNull();
            frequenciaPorDisciplina.TotalPresencas.ShouldBeLessThanOrEqualTo(frequenciaPorDisciplina.TotalAulas);

            var frequenciaGeral = frequenciasAluno.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.Geral);
            frequenciaGeral.ShouldNotBeNull();
            frequenciaGeral.TotalPresencas.ShouldBeLessThanOrEqualTo(frequenciaPorDisciplina.TotalAulas);
        }

        [Fact(DisplayName = "Frequência - Regularizar frequência de aluno inativo com presenças maior que a quantidade de aulas, excluindo registros frequência inválidos e acionando o cálculo")]
        public async Task Regularizar_frequencia_aluno_inativo_presencas_maior_quantidade_aulas_excluir_registros_acionar_calculo()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();

            (DateTime inicioPeriodo, DateTime fimPeriodo) = (new DateTime(dataAtual.Year, dataAtual.Month, 1),
                new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month)));

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = dataAtual.Year,
                Nome = "tipo cal teste",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                CriadoEm = dataAtual
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 1,
                CodigoAluno = "2",
                TurmaId = "1",
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                DisciplinaId = "1",
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                TotalCompensacoes = 0,
                TotalPresencas = 2,
                TotalRemotos = 0,
                PeriodoEscolarId = 1
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 2,
                CodigoAluno = "2",
                TurmaId = "1",
                Tipo = TipoFrequenciaAluno.Geral,
                DisciplinaId = string.Empty,
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                TotalCompensacoes = 0,
                TotalPresencas = 2,
                TotalRemotos = 0,
                PeriodoEscolarId = 1
            });

            await InserirNaBase(new Dre()
            {
                Id = 1,
                CodigoDre = "1",
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                UeId = 1,
                DataAtualizacao = dataAtual,
                TipoTurma = TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = dataAtual.Year
            });

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 1,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1",
                Quantidade = 1,
                DataAula = new DateTime(dataAtual.Year, dataAtual.Month, 1),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 2,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1",
                Quantidade = 1,
                DataAula = new DateTime(dataAtual.Year, dataAtual.Month, 11),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 3,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1",
                Quantidade = 1,
                DataAula = new DateTime(dataAtual.Year, dataAtual.Month, 21),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequencia()
            {
                Id = 1,
                AulaId = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequencia()
            {
                Id = 2,
                AulaId = 2,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequencia()
            {
                Id = 3,
                AulaId = 3,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Id = 1,
                Valor = 1,
                CodigoAluno = "2",
                NumeroAula = 1,
                RegistroFrequenciaId = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                AulaId = 1
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Id = 1,
                Valor = 1,
                CodigoAluno = "2",
                NumeroAula = 1,
                RegistroFrequenciaId = 2,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                AulaId = 2
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Id = 3,
                Valor = 1,
                CodigoAluno = "2",
                NumeroAula = 1,
                RegistroFrequenciaId = 3,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                AulaId = 3
            });

            var commandHandler = new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommandHandler(repositorioRegistroFrequenciaAluno, repositorioCache, mediator);

            var resultado = await commandHandler
                .Handle(new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand(1), It.IsAny<CancellationToken>());

            resultado.ShouldBeTrue();

            var registrosFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();

            registrosFrequenciaAluno.ShouldNotBeEmpty();
            registrosFrequenciaAluno.Count(x => x.CodigoAluno == "2").ShouldBe(1);
            registrosFrequenciaAluno.SingleOrDefault(x => x.AulaId == 2).ShouldNotBeNull();

            var frequenciasAluno = ObterTodos<Dominio.FrequenciaAluno>();

            frequenciasAluno.ShouldNotBeEmpty();
            frequenciasAluno.Count(x => x.CodigoAluno == "2" && x.TurmaId == "1").ShouldBe(2);

            var frequenciaPorDisciplina = frequenciasAluno.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina);
            frequenciaPorDisciplina.ShouldNotBeNull();
            frequenciaPorDisciplina.TotalPresencas.ShouldBeLessThanOrEqualTo(frequenciaPorDisciplina.TotalAulas);

            var frequenciaGeral = frequenciasAluno.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.Geral);
            frequenciaGeral.ShouldNotBeNull();
            frequenciaGeral.TotalPresencas.ShouldBeLessThanOrEqualTo(frequenciaPorDisciplina.TotalAulas);
        }

        [Fact(DisplayName = "Frequência - Regularizar frequência de aluno ativo com presenças maior que a quantidade de aulas, sem excluir registros frequência inválidos e acionando o cálculo")]
        public async Task Regularizar_frequencia_aluno_ativo_presencas_maior_quantidade_aulas_nao_excluir_registros_acionar_calculo()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();

            (DateTime inicioPeriodo, DateTime fimPeriodo) = (new DateTime(dataAtual.Year, dataAtual.Month, 1),
                new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month)));

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = dataAtual.Year,
                Nome = "tipo cal teste",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                CriadoEm = dataAtual
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 1,
                CodigoAluno = "1",
                TurmaId = "1",
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                DisciplinaId = "1",
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                TotalCompensacoes = 0,
                TotalPresencas = 2,
                TotalRemotos = 0,
                PeriodoEscolarId = 1
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 2,
                CodigoAluno = "1",
                TurmaId = "1",
                Tipo = TipoFrequenciaAluno.Geral,
                DisciplinaId = string.Empty,
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                TotalCompensacoes = 0,
                TotalPresencas = 2,
                TotalRemotos = 0,
                PeriodoEscolarId = 1
            });

            await InserirNaBase(new Dre()
            {
                Id = 1,
                CodigoDre = "1",
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                UeId = 1,
                DataAtualizacao = dataAtual,
                TipoTurma = TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = dataAtual.Year
            });           

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 1,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1",
                Quantidade = 1,
                DataAula = new DateTime(dataAtual.Year, dataAtual.Month, 11),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });          

            await InserirNaBase(new RegistroFrequencia()
            {
                Id = 1,
                AulaId = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });           

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Id = 1,
                Valor = 1,
                CodigoAluno = "1",
                NumeroAula = 1,
                RegistroFrequenciaId = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                AulaId = 1
            });           

            var commandHandler = new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommandHandler(repositorioRegistroFrequenciaAluno, repositorioCache, mediator);

            var resultado = await commandHandler
                .Handle(new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand(1), It.IsAny<CancellationToken>());

            resultado.ShouldBeTrue();

            var registrosFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();

            registrosFrequenciaAluno.ShouldNotBeEmpty();
            registrosFrequenciaAluno.Count(x => x.CodigoAluno == "1").ShouldBe(1);

            var frequenciasAluno = ObterTodos<Dominio.FrequenciaAluno>();

            frequenciasAluno.ShouldNotBeEmpty();
            frequenciasAluno.Count(x => x.CodigoAluno == "1" && x.TurmaId == "1").ShouldBe(2);

            var frequenciaPorDisciplina = frequenciasAluno.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina);
            frequenciaPorDisciplina.ShouldNotBeNull();
            frequenciaPorDisciplina.TotalPresencas.ShouldBeLessThanOrEqualTo(frequenciaPorDisciplina.TotalAulas);

            var frequenciaGeral = frequenciasAluno.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.Geral);
            frequenciaGeral.ShouldNotBeNull();
            frequenciaGeral.TotalPresencas.ShouldBeLessThanOrEqualTo(frequenciaPorDisciplina.TotalAulas);
        }

        [Fact(DisplayName = "Frequência - Regularizar frequência de aluno ativo com presenças maior que a quantidade de aulas, excluindo todos registros frequência inválidos e acionando o cálculo")]
        public async Task Regularizar_frequencia_aluno_ativo_presencas_maior_quantidade_aulas_excluir_todos_registros_acionar_calculo()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();

            (DateTime inicioPeriodo, DateTime fimPeriodo) = (new DateTime(dataAtual.Year, dataAtual.Month, 1),
                new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month)));

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = dataAtual.Year,
                Nome = "tipo cal teste",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                CriadoEm = dataAtual
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 1,
                CodigoAluno = "1",
                TurmaId = "1",
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                DisciplinaId = "1",
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                TotalCompensacoes = 0,
                TotalPresencas = 2,
                TotalRemotos = 0,
                PeriodoEscolarId = 1
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 2,
                CodigoAluno = "1",
                TurmaId = "1",
                Tipo = TipoFrequenciaAluno.Geral,
                DisciplinaId = string.Empty,
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                TotalCompensacoes = 0,
                TotalPresencas = 2,
                TotalRemotos = 0,
                PeriodoEscolarId = 1
            });

            await InserirNaBase(new Dre()
            {
                Id = 1,
                CodigoDre = "1",
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                UeId = 1,
                DataAtualizacao = dataAtual,
                TipoTurma = TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = dataAtual.Year
            });

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 1,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1",
                Quantidade = 1,
                DataAula = new DateTime(dataAtual.Year, dataAtual.Month, 1),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequencia()
            {
                Id = 1,
                AulaId = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Id = 1,
                Valor = 1,
                CodigoAluno = "1",
                NumeroAula = 1,
                RegistroFrequenciaId = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                AulaId = 1
            });

            var commandHandler = new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommandHandler(repositorioRegistroFrequenciaAluno, repositorioCache, mediator);

            var resultado = await commandHandler
                .Handle(new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand(1), It.IsAny<CancellationToken>());

            resultado.ShouldBeTrue();

            var registrosFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();

            registrosFrequenciaAluno.Any(x => x.CodigoAluno == "1").ShouldBeFalse();

            var frequenciasAluno = ObterTodos<Dominio.FrequenciaAluno>();

            frequenciasAluno.Any(x => x.CodigoAluno == "1" && x.TurmaId == "1").ShouldBeFalse();
        }

        [Fact(DisplayName = "Frequência - Regularizar frequência de aluno inativo com presenças maior que a quantidade de aulas, excluindo todos registros frequência inválidos e acionando o cálculo")]
        public async Task Regularizar_frequencia_aluno_inativo_presencas_maior_quantidade_aulas_excluir_todos_registros_acionar_calculo()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();

            (DateTime inicioPeriodo, DateTime fimPeriodo) = (new DateTime(dataAtual.Year, dataAtual.Month, 1),
                new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month)));

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = dataAtual.Year,
                Nome = "tipo cal teste",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                CriadoEm = dataAtual
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 1,
                CodigoAluno = "2",
                TurmaId = "1",
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                DisciplinaId = "1",
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                TotalCompensacoes = 0,
                TotalPresencas = 2,
                TotalRemotos = 0,
                PeriodoEscolarId = 1
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 2,
                CodigoAluno = "2",
                TurmaId = "1",
                Tipo = TipoFrequenciaAluno.Geral,
                DisciplinaId = string.Empty,
                PeriodoInicio = inicioPeriodo,
                PeriodoFim = fimPeriodo,
                Bimestre = 1,
                TotalAulas = 1,
                TotalAusencias = 0,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                TotalCompensacoes = 0,
                TotalPresencas = 2,
                TotalRemotos = 0,
                PeriodoEscolarId = 1
            });

            await InserirNaBase(new Dre()
            {
                Id = 1,
                CodigoDre = "1",
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                UeId = 1,
                DataAtualizacao = dataAtual,
                TipoTurma = TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = dataAtual.Year
            });

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 1,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1",
                Quantidade = 1,
                DataAula = new DateTime(dataAtual.Year, dataAtual.Month, 1),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 2,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1",
                Quantidade = 1,
                DataAula = new DateTime(dataAtual.Year, dataAtual.Month, 21),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequencia()
            {
                Id = 1,
                AulaId = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequencia()
            {
                Id = 2,
                AulaId = 2,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Id = 1,
                Valor = 1,
                CodigoAluno = "2",
                NumeroAula = 1,
                RegistroFrequenciaId = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                AulaId = 1
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Id = 1,
                Valor = 1,
                CodigoAluno = "2",
                NumeroAula = 1,
                RegistroFrequenciaId = 2,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                AulaId = 2
            });

            var commandHandler = new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommandHandler(repositorioRegistroFrequenciaAluno, repositorioCache, mediator);

            var resultado = await commandHandler
                .Handle(new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand(1), It.IsAny<CancellationToken>());

            resultado.ShouldBeTrue();

            var registrosFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();

            registrosFrequenciaAluno.Any(x => x.CodigoAluno == "2").ShouldBeFalse();

            var frequenciasAluno = ObterTodos<Dominio.FrequenciaAluno>();

            frequenciasAluno.Any(x => x.CodigoAluno == "2" && x.TurmaId == "1").ShouldBeFalse();
        }
    }
}
