using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Aula.DiarioBordo.ServicosFakes;
using SME.SGP.TesteIntegracao.DiarioBordo;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Aula.DiarioBordo
{
    public class Ao_inserir_devolutiva : DiarioBordoTesteBase
    {
        public Ao_inserir_devolutiva(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakeServicoEol), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_Inserir_devolutiva_diario()
        {
            var filtroDiarioBordo = new FiltroDiarioBordoDto
            {
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                DataAulaDiarioBordo = DateTimeExtension.HorarioBrasilia().Date
            };

            await CriarDadosBasicos(filtroDiarioBordo, false);

            await CriarAula(filtroDiarioBordo.DataAulaDiarioBordo, RecorrenciaAula.AulaUnica,
                TipoAula.Normal,
                USUARIO_PROFESSOR_CODIGO_RF_1111111,
                TURMA_CODIGO_1, UE_CODIGO_1,
                COMPONENTE_CURRICULAR_513.ToString(), TIPO_CALENDARIO_1);

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                Id = DIARIO_BORDO_ID_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                TurmaId = TURMA_ID_1,
                DevolutivaId = null,
                Planejamento = "Planejado 512",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                Id = DIARIO_BORDO_ID_2,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_513,
                TurmaId = TURMA_ID_1,
                DevolutivaId = null,
                Planejamento = "Planejado 513",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            var useCase = ServiceProvider.GetService<IInserirDevolutivaUseCase>();
            var parametro = new InserirDevolutivaDto() { 
                CodigoComponenteCurricular = COMPONENTE_CURRICULAR_512,
                Descricao = @"xxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxxxxxxxxxxxxxx 
                              xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxxxxxxxxxxxxxx 
                              xxxxxxxxxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx     
                              xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx xxxx",
                TurmaCodigo = TURMA_CODIGO_1,
                PeriodoInicio = filtroDiarioBordo.DataAulaDiarioBordo,
                PeriodoFim = filtroDiarioBordo.DataAulaDiarioBordo.AddDays(2),
            };

            var dto = await useCase.Executar(parametro);
            dto.ShouldNotBeNull();
            var devolutivas = ObterTodos<Dominio.Devolutiva>();
            devolutivas.ShouldNotBeNull();
            devolutivas.Count.ShouldBe(1);
            devolutivas.Exists(x => x.CodigoComponenteCurricular == COMPONENTE_CURRICULAR_512).ShouldBeTrue();
        }
    }
}
