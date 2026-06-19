using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Abrangencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Abrangencia
{
    public class Ao_filtrar_ues_por_anos_turma : AbrangenciaBase
    {
        public Ao_filtrar_ues_por_anos_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Abrangência - Obter ues filtrando por anos de turma, perfil comum")]
        public async Task Ao_obter_ues_filtrando_por_anos_turma_perfil_comum()
        {
            var filtro = new FiltroTesteDto()
            {
                Perfil = ObterPerfilPOA_Portugues(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            await InserirNaBase(new Dominio.Abrangencia()
            {
                UsuarioId = USUARIO_ID_1,
                Perfil = Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name()),
                DreId = DRE_ID_1
            });

            var useCase = ServiceProvider.GetService<IObterUEsPorDreUseCase>();

            var dtoSemCorrespondencia = new UEsPorDreDto()
            {
                CodigoDre = DRE_CODIGO_1,
                AnoLetivo = DateTime.Now.Year,
                Modalidade = Modalidade.Medio,
                AnosTurma = new[] { ANO_1, ANO_2, ANO_3 }
            };

            var uesSemCorrespondencia = await useCase.Executar(dtoSemCorrespondencia);
            uesSemCorrespondencia.ShouldBeEmpty();

            var dtoComCorrespondencia = new UEsPorDreDto()
            {
                CodigoDre = DRE_CODIGO_1,
                AnoLetivo = DateTime.Now.Year,
                Modalidade = Modalidade.Medio,
                AnosTurma = new[] { ANO_7 }
            };

            var uesComCorrespondencia = await useCase.Executar(dtoComCorrespondencia);
            uesComCorrespondencia.ShouldNotBeNull();
            uesComCorrespondencia.Count().ShouldBe(1);
            uesComCorrespondencia.First().Id.ShouldBe(UE_ID_1);
        }

        [Fact(DisplayName = "Abrangência - Obter ues filtrando por anos de turma, perfil supervisor")]
        public async Task Ao_obter_ues_filtrando_por_anos_turma_perfil_supervisor()
        {
            var filtro = new FiltroTesteDto()
            {
                Perfil = Guid.Parse(PerfilUsuario.SUPERVISOR.Name()).ToString(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            await InserirNaBase(new SupervisorEscolaDre()
            {
                DreId = DRE_CODIGO_1,
                EscolaId = UE_CODIGO_1,
                SupervisorId = USUARIO_PROFESSOR_LOGIN_2222222,
                Tipo = (int)TipoResponsavelAtribuicao.SupervisorEscolar,
                CriadoEm = DateTime.Now,
                CriadoPor = "Teste",
                CriadoRF = USUARIO_PROFESSOR_LOGIN_2222222,
                Excluido = false
            });

            var useCase = ServiceProvider.GetService<IObterUEsPorDreUseCase>();

            var dtoSemCorrespondencia = new UEsPorDreDto()
            {
                CodigoDre = DRE_CODIGO_1,
                AnoLetivo = DateTime.Now.Year,
                Modalidade = Modalidade.Medio,
                AnosTurma = new[] { ANO_1, ANO_2, ANO_3 }
            };

            var uesSemCorrespondencia = await useCase.Executar(dtoSemCorrespondencia);
            uesSemCorrespondencia.ShouldBeEmpty();

            var dtoComCorrespondencia = new UEsPorDreDto()
            {
                CodigoDre = DRE_CODIGO_1,
                AnoLetivo = DateTime.Now.Year,
                Modalidade = Modalidade.Medio,
                AnosTurma = new[] { ANO_7 }
            };

            var uesComCorrespondencia = await useCase.Executar(dtoComCorrespondencia);
            uesComCorrespondencia.ShouldNotBeNull();
            uesComCorrespondencia.Count().ShouldBe(1);
            uesComCorrespondencia.First().Id.ShouldBe(UE_ID_1);
        }
    }
}
