using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Aula
{
    public class Ao_registrar_aula_recorrente : AulaTeste
    {
        public Ao_registrar_aula_recorrente(CollectionFixture collectionFixture)
            : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Aula - Ao inserir aula recorrente considerar componentes de infantil agrupados")]
        public async Task Ao_registrar_aula_recorrente_considerando_componentes_infantil_agrupados()
        {
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>), typeof(ObterUsuarioLogadoQueryHandlerFake), ServiceLifetime.Scoped));
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerComponenteInfantilFake), ServiceLifetime.Scoped));
            _collectionFixture.BuildServiceProvider();

            var dataAtual = DateTimeExtension.HorarioBrasilia();

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

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                UeId = 1,
                DataAtualizacao = dataAtual,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                AnoLetivo = dataAtual.Year
            });

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = dataAtual.Year,
                Nome = "tipo cal infantil",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                CriadoEm = dataAtual,
                CriadoPor = "sistema",
                CriadoRF = "1234",
                Situacao = true
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = new DateTime(dataAtual.Year, dataAtual.Month, 1),
                PeriodoFim = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month)),
                CriadoPor = "sistema",
                CriadoRF = "1234",
                CriadoEm = dataAtual
            });

            var useCase = ServiceProvider.GetService<IInserirAulaRecorrenteUseCase>();

            var usuario = await ServiceProvider.GetService<IMediator>().Send(ObterUsuarioLogadoQuery.Instance);
            var command = new InserirAulaRecorrenteCommand(usuario, dataAtual, 1, "1", 1, "comp infantil", 1, TipoAula.Normal, "1", true, RecorrenciaAula.RepetirBimestreAtual, null);
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(command));
            var resultado = await useCase.Executar(mensagem);

            resultado.ShouldBeTrue();

            var aulas = ObterTodos<Dominio.Aula>();
            aulas.ShouldNotBeEmpty();
            aulas.Count.ShouldBe(1);
            aulas.SingleOrDefault(x => x.TipoCalendarioId == 1 && x.DisciplinaId == "1" && x.RecorrenciaAula == RecorrenciaAula.RepetirBimestreAtual && x.TurmaId == "1").ShouldNotBeNull();
        }
    }
}
