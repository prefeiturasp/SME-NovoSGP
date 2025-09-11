using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.TesteIntegracao.DiarioBordo;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Aula.DiarioBordo
{
    public class Ao_obter_diario_por_periodo : DiarioBordoTesteBase
    {
        public Ao_obter_diario_por_periodo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_diario_bordo_devolutiva_unificada()
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

            var useCase = ServiceProvider.GetService<IObterDiariosDeBordoPorPeriodoUseCase>();
            var parametro = new FiltroTurmaComponentePeriodoDto(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_512, filtroDiarioBordo.DataAulaDiarioBordo.Date, filtroDiarioBordo.DataAulaDiarioBordo.AddDays(5).Date);
            var dto = await useCase.Executar(parametro);

            dto.ShouldNotBeNull();
            var item = dto.Items.FirstOrDefault();
            item.Data.ShouldBe(filtroDiarioBordo.DataAulaDiarioBordo);
            item.PlanejamentoSimples.ShouldContain("REGÊNCIA INFANTIL EMEI 2H");
            item.PlanejamentoSimples.ShouldContain("REGÊNCIA INFANTIL EMEI 4H");
        }

        [Fact]
        public async Task Ao_diario_bordo_devolutiva()
        {
            var filtroDiarioBordo = new FiltroDiarioBordoDto
            {
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                DataAulaDiarioBordo = DateTimeExtension.HorarioBrasilia().Date
            };

            await CriarDadosBasicos(filtroDiarioBordo, false);

            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_4,
                Historica = false,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                AnoLetivo = 2023,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = TipoTurma.Regular
            });

            await CriarAula(filtroDiarioBordo.DataAulaDiarioBordo, RecorrenciaAula.AulaUnica,
                TipoAula.Normal,
                USUARIO_PROFESSOR_CODIGO_RF_1111111,
                TURMA_CODIGO_4, UE_CODIGO_1,
                COMPONENTE_CURRICULAR_512.ToString(), TIPO_CALENDARIO_1);

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                Id = DIARIO_BORDO_ID_1,
                AulaId = AULA_ID_2,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                TurmaId = TURMA_ID_4,
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
                AulaId = AULA_ID_2,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_513,
                TurmaId = TURMA_ID_4,
                DevolutivaId = null,
                Planejamento = "Planejado 513",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            var useCase = ServiceProvider.GetService<IObterDiariosDeBordoPorPeriodoUseCase>();
            var parametro = new FiltroTurmaComponentePeriodoDto(TURMA_CODIGO_4, COMPONENTE_CURRICULAR_512, filtroDiarioBordo.DataAulaDiarioBordo.Date, filtroDiarioBordo.DataAulaDiarioBordo.AddDays(5).Date);
            var dto = await useCase.Executar(parametro);

            dto.ShouldNotBeNull();
            var item = dto.Items.FirstOrDefault();
            item.Data.ShouldBe(filtroDiarioBordo.DataAulaDiarioBordo);
            item.PlanejamentoSimples.ShouldBe("<b>Planejamento</b><br/>Planejado 512<br/>");
        }
    }
}
