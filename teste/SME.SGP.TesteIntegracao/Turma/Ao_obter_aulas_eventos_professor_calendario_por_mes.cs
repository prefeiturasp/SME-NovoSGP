using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Turma
{
    public class Ao_obter_aulas_eventos_professor_calendario_por_mes : TesteBaseComuns
    {
        public Ao_obter_aulas_eventos_professor_calendario_por_mes(CollectionFixture collectionFixture)
            : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Turma - Obter aulas do calendário por mês obtendo os componentes curriculares do infantil com agrupamento")]
        public async Task Obter_aulas_calendario_por_mes_componentes_infantil_agrupado()
        {
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>), typeof(ObterUsuarioLogadoQueryHandlerFake), ServiceLifetime.Scoped));
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerComponenteInfantilFake), ServiceLifetime.Scoped));
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery, IEnumerable<ProfessorTitularDisciplinaEol>>), typeof(ObterProfessoresTitularesDisciplinasEolQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            _collectionFixture.BuildServiceProvider();

            var dataAtual = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = dataAtual.Year,
                Nome = "tipo cal infantil",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                Situacao = true,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
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
                DataAula = dataAtual.Date,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
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
                DataAtualizacao = dataAtual,
                DreId = 1
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                DataAtualizacao = dataAtual,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                UeId = 1,
                AnoLetivo = dataAtual.Year,
                ModalidadeCodigo = Modalidade.EducacaoInfantil
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = new DateTime(dataAtual.Year, dataAtual.Month, 1),
                PeriodoFim = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month)),
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                CriadoEm = dataAtual
            });

            var useCase = ServiceProvider.GetService<IObterAulasEventosProfessorCalendarioPorMesUseCase>();

            var dto = new FiltroAulasEventosCalendarioDto() { DreCodigo = "1", UeCodigo = "1", AnoLetivo = dataAtual.Year, TurmaCodigo = "1" };
            var retorno = await useCase.Executar(dto, 1, dataAtual.Month);

            retorno.ShouldNotBeNull();
            retorno.ShouldNotBeEmpty();
            retorno.Count().ShouldBe(DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));
            retorno.Count(x => x.TemAula).ShouldBe(1);
            retorno.Single(x => x.TemAula).Dia.ShouldBe(dataAtual.Day);
        }
    }
}
