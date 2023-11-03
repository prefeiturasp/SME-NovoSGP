using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.IO;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Aula.Aula.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Aula.Aula
{
    public class Ao_gerar_aulas_automaticas_regencia_infantil : AulaTeste
    {
        private const string CRIADO_ALTERADO_POR_SISTEMA = "Sistema";
        private const string CRIADO_ALTERADO_RF_SISTEMA = "0";

        public Ao_gerar_aulas_automaticas_regencia_infantil(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.AddTransient<ObterTurmasInfantilNaoDeProgramaQueryHandler>();
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandFakeCriarAulasInfantilERegenciaAutomaticamente), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery, IEnumerable<DadosTurmaAulasAutomaticaDto>>), typeof(ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQueryHandlerFakeSyncRegencia), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>), typeof(ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandleFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_obter_somente_turma_regulares_e_nao_de_programa()
        {
            await InserirDados();
            var turmasInseridas = ObterTodos<Dominio.Turma>();
            var queryMediator = ServiceProvider.GetRequiredService<ObterTurmasInfantilNaoDeProgramaQueryHandler>();
            var retorno = await queryMediator.Handle(new ObterTurmasInfantilNaoDeProgramaQuery(DateTimeExtension.HorarioBrasilia().Year), new System.Threading.CancellationToken());

            Assert.NotNull(retorno);
            retorno.Any(r => r.TipoTurma == TipoTurma.Programa).ShouldBeFalse();
            retorno.Count().ShouldBe(2);
        }

        [Fact]
        public async Task Deve_criar_aulas_infantil_sem_aulas_cadastradas_previamente_recuperando_diario_bordo_com_aula_excluida()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var dataInicioPeriodo = new DateTime(anoAtual, 2, 5);
            var dataFimPerioodo = new DateTime(anoAtual, 4, 30);
            var useCase = ServiceProvider.GetService<ICriarAulasInfantilAutomaticamenteUseCase>();

            var camposTabelaParametros = new string[] { "nome", "tipo", "descricao", "valor", "ativo", "criado_em", "criado_por", "criado_rf" };
            var valoresCamposTabelaParametros = new string[]
            {
                "'aulasInfantil'", "26", "'aulas Infantil'", "1", "true",
                $"'{DateTimeExtension.HorarioBrasilia():yyyy-MM-dd}'", $"'{CRIADO_ALTERADO_POR_SISTEMA}'", $"'{CRIADO_ALTERADO_RF_SISTEMA}'"
            };

            await InserirNaBase("parametros_sistema", camposTabelaParametros, valoresCamposTabelaParametros);

            await InserirNaBase(new TipoCalendario()
            {
                AnoLetivo = anoAtual,
                Nome = "tipo cal infantil",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                Situacao = true
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = dataInicioPeriodo,
                PeriodoFim = dataFimPerioodo,
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new Dre()
            {
                CodigoDre = "1",
                DataAtualizacao = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new Ue()
            {
                CodigoUe = "1",
                DreId = 1,
                DataAtualizacao = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new Dominio.Turma()
            {
                CodigoTurma = "1",
                AnoLetivo = anoAtual,
                UeId = 1,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                DataAtualizacao = DateTimeExtension.HorarioBrasilia(),
                Historica = false,
                TipoTurma = TipoTurma.Regular,
                Ano = "1",
            });

            await InserirNaBase("componente_curricular_grupo_matriz", new string[] { "id", "nome" }, new string[] { "1", "'gm1'" });
            await InserirNaBase("componente_curricular_area_conhecimento", new string[] { "id", "nome" }, new string[] { "1", "'ac1'" });
            await InserirNaBase("componente_curricular", new[] { "id", "grupo_matriz_id", "area_conhecimento_id" }, new[] { "512", "1", "1" });

            await InserirNaBase(new Dominio.Aula()
            {
                UeId = "1",
                DisciplinaId = "512",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = CRIADO_ALTERADO_POR_SISTEMA,
                Quantidade = 1,
                DataAula = await ObterDataNoMesConsiderandoSomenteDiasUteis(3, anoAtual),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                Excluido = true
            });

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                AulaId = 1,
                Planejamento = "teste",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                ComponenteCurricularId = 512,
                TurmaId = 1
            });

            var resultado = await useCase.Executar(new MensagemRabbit());

            resultado.ShouldBeTrue();

            var aulasCriadas = ObterTodos<Dominio.Aula>();
            var diariosBordo = ObterTodos<Dominio.DiarioBordo>();

            aulasCriadas.Any().ShouldBeTrue();
            aulasCriadas.Count(a => a.CriadoPor
                .Equals(CRIADO_ALTERADO_POR_SISTEMA, StringComparison.InvariantCultureIgnoreCase))
                    .ShouldBe(await ObterQuantidadeDiasUteisNoPeriodo(dataInicioPeriodo, dataFimPerioodo));
            aulasCriadas.Single(a => a.Id == 1).Excluido.ShouldBeFalse();
            diariosBordo.Any().ShouldBeTrue();
            diariosBordo.Single(db => db.AulaId == 1).Excluido.ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_criar_aulas_regencia_sem_aulas_cadastradas_previamente_recuperando_diario_bordo_com_aula_excluida()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var dataInicioPeriodo = new DateTime(anoAtual, 2, 5);
            var dataFimPerioodo = new DateTime(anoAtual, 4, 30);
            var useCase = ServiceProvider.GetService<ICarregarUesTurmasRegenciaAulaAutomaticaUseCase>();

            await InserirNaBase(new TipoCalendario()
            {
                AnoLetivo = anoAtual,
                Nome = "tipo cal fundametal",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                Situacao = true
            });

            await InserirNaBase(new TipoCalendario()
            {
                AnoLetivo = anoAtual,
                Nome = "tipo cal eja",
                Periodo = Periodo.Semestral,
                Modalidade = ModalidadeTipoCalendario.EJA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                Situacao = true,
                Semestre = 1
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = dataInicioPeriodo,
                PeriodoFim = dataFimPerioodo,
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                TipoCalendarioId = 2,
                Bimestre = 1,
                PeriodoInicio = dataInicioPeriodo,
                PeriodoFim = dataFimPerioodo,
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new Dre()
            {
                CodigoDre = "1",
                DataAtualizacao = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new Ue()
            {
                CodigoUe = "1",
                DreId = 1,
                DataAtualizacao = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase(new Dominio.Turma()
            {
                CodigoTurma = "1",
                AnoLetivo = anoAtual,
                UeId = 1,
                ModalidadeCodigo = Modalidade.Fundamental,
                DataAtualizacao = DateTimeExtension.HorarioBrasilia(),
                Historica = false,
                TipoTurma = TipoTurma.Regular,
                Ano = "1",
            });

            await InserirNaBase(new Dominio.Turma()
            {
                CodigoTurma = "2",
                AnoLetivo = anoAtual,
                UeId = 1,
                ModalidadeCodigo = Modalidade.EJA,
                DataAtualizacao = DateTimeExtension.HorarioBrasilia(),
                Historica = false,
                TipoTurma = TipoTurma.Regular,
                Ano = "1",
            });

            await InserirNaBase("componente_curricular_grupo_matriz", new string[] { "id", "nome" }, new string[] { "1", "'gm1'" });
            await InserirNaBase("componente_curricular_area_conhecimento", new string[] { "id", "nome" }, new string[] { "1", "'ac1'" });
            await InserirNaBase("componente_curricular", new[] { "id", "grupo_matriz_id", "area_conhecimento_id" }, new[] { "1105", "1", "1" });
            await InserirNaBase("componente_curricular", new[] { "id", "grupo_matriz_id", "area_conhecimento_id" }, new[] { "1113", "1", "1" });

            await InserirNaBase(new Dominio.Aula()
            {
                UeId = "1",
                DisciplinaId = "1105",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = CRIADO_ALTERADO_POR_SISTEMA,
                Quantidade = 1,
                DataAula = await ObterDataNoMesConsiderandoSomenteDiasUteis(3, anoAtual),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                Excluido = true
            });

            await InserirNaBase(new Dominio.Aula()
            {
                UeId = "1",
                DisciplinaId = "1113",
                TurmaId = "2",
                TipoCalendarioId = 2,
                ProfessorRf = CRIADO_ALTERADO_POR_SISTEMA,
                Quantidade = 1,
                DataAula = await ObterDataNoMesConsiderandoSomenteDiasUteis(3, anoAtual),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                Excluido = true
            });

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                AulaId = 1,
                Planejamento = "teste",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                ComponenteCurricularId = 1105,
                TurmaId = 1
            });

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                AulaId = 2,
                Planejamento = "teste",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = CRIADO_ALTERADO_POR_SISTEMA,
                CriadoRF = CRIADO_ALTERADO_RF_SISTEMA,
                ComponenteCurricularId = 1113,
                TurmaId = 2
            });

            var resultado = await useCase.Executar(new MensagemRabbit());

            resultado.ShouldBeTrue();

            var aulasCriadas = ObterTodos<Dominio.Aula>();
            var diariosBordo = ObterTodos<Dominio.DiarioBordo>();

            aulasCriadas.Any().ShouldBeTrue();
            aulasCriadas.Count(a => a.CriadoPor
                .Equals(CRIADO_ALTERADO_POR_SISTEMA, StringComparison.InvariantCultureIgnoreCase))
                    .ShouldBe((await ObterQuantidadeDiasUteisNoPeriodo(dataInicioPeriodo, dataFimPerioodo)) * 2);
            aulasCriadas.Single(a => a.Id == 1).Excluido.ShouldBeFalse();
            aulasCriadas.Single(a => a.Id == 2).Excluido.ShouldBeFalse();
            diariosBordo.Any().ShouldBeTrue();
            diariosBordo.Single(db => db.AulaId == 1).Excluido.ShouldBeFalse();
            diariosBordo.Single(db => db.AulaId == 2).Excluido.ShouldBeFalse();
        }

        private async Task InserirDados()
        {
            await InserirNaBase(new Dre()
            {
                Id = 1
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1
            });
            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = TipoTurma.Regular,
                UeId = 1,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ano = "1"
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 2,
                CodigoTurma = "2",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = TipoTurma.Regular,
                UeId = 1,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ano = "1"
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 3,
                CodigoTurma = "3",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = TipoTurma.Programa,
                UeId = 1,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Ano = "1"
            });
        }

        private async Task<DateTime> ObterDataNoMesConsiderandoSomenteDiasUteis(int mes, int ano)
        {
            var dia = new Random().Next(1, DateTime.DaysInMonth(ano, mes));
            var data = new DateTime(ano, mes, dia);

            if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                return await ObterDataNoMesConsiderandoSomenteDiasUteis(mes, ano);

            return data;
        }

        private static async Task<int> ObterQuantidadeDiasUteisNoPeriodo(DateTime inicio, DateTime fim)
        {
            var contador = 0;
            var diasFimSemana = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

            for (var dataCorrente = inicio; dataCorrente <= fim; dataCorrente = dataCorrente.AddDays(1))
            {
                if (!diasFimSemana.Contains(dataCorrente.DayOfWeek))
                    contador++;
            }

            return await Task.FromResult(contador);
        }
    }
}
