using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Abrangencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
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

        // Cria DRE/UE/turma (turma com ano = ANO_7) e concede abrangência direta ao usuário (perfil comum).
        private async Task CriarCenarioComTurmaAno7PerfilComum()
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
        }

        // Cria DRE/UE/turma (turma com ano = ANO_7) e concede a abrangência via supervisor_escola_dre,
        // forçando o resultado a vir pelo caminho de AcrescentarUesSupervisor (filtro em memória).
        private async Task CriarCenarioComTurmaAno7PerfilSupervisor()
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
        }

        private Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string[] anosTurma)
        {
            var useCase = ServiceProvider.GetService<IObterUEsPorDreUseCase>();

            return useCase.Executar(new UEsPorDreDto()
            {
                CodigoDre = DRE_CODIGO_1,
                AnoLetivo = DateTime.Now.Year,
                Modalidade = Modalidade.Medio,
                AnosTurma = anosTurma
            });
        }

        [Fact(DisplayName = "Abrangência - ObterUes não deve retornar UE cuja turma não está na lista de anosTurma informada (perfil comum)")]
        public async Task Nao_deve_retornar_ue_de_turma_fora_do_filtro_anosTurma_perfil_comum()
        {
            await CriarCenarioComTurmaAno7PerfilComum();

            var ues = await ObterUes(new[] { ANO_1, ANO_2, ANO_3 });

            ues.ShouldBeEmpty();
        }

        [Fact(DisplayName = "Abrangência - ObterUes deve retornar UE cuja turma está na lista de anosTurma informada (perfil comum)")]
        public async Task Deve_retornar_ue_de_turma_dentro_do_filtro_anosTurma_perfil_comum()
        {
            await CriarCenarioComTurmaAno7PerfilComum();

            var ues = await ObterUes(new[] { ANO_7 });

            ues.ShouldNotBeNull();
            ues.Count().ShouldBe(1);
            ues.First().Id.ShouldBe(UE_ID_1);
        }

        [Fact(DisplayName = "Abrangência - ObterUes não deve retornar UE cuja turma não está na lista de anosTurma informada (perfil supervisor)")]
        public async Task Nao_deve_retornar_ue_de_turma_fora_do_filtro_anosTurma_perfil_supervisor()
        {
            await CriarCenarioComTurmaAno7PerfilSupervisor();

            var ues = await ObterUes(new[] { ANO_1, ANO_2, ANO_3 });

            ues.ShouldBeEmpty();
        }

        [Fact(DisplayName = "Abrangência - ObterUes deve retornar UE cuja turma está na lista de anosTurma informada (perfil supervisor)")]
        public async Task Deve_retornar_ue_de_turma_dentro_do_filtro_anosTurma_perfil_supervisor()
        {
            await CriarCenarioComTurmaAno7PerfilSupervisor();

            var ues = await ObterUes(new[] { ANO_7 });

            ues.ShouldNotBeNull();
            ues.Count().ShouldBe(1);
            ues.First().Id.ShouldBe(UE_ID_1);
        }
    }
}
