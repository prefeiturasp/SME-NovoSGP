using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Abrangencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Abrangencia
{
    public class Ao_filtrar_turmas_por_anos_turma : AbrangenciaBase
    {
        public Ao_filtrar_turmas_por_anos_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        private void ConfigurarFakeEol()
        {
            var turmasEolFake = new List<TurmaApiEolDto>
            {
                new TurmaApiEolDto { Codigo = int.Parse(TURMA_CODIGO_1) }
            };

            var jsonResposta = JsonConvert.SerializeObject(turmasEolFake);
            var conteudoHttp = new StringContent(jsonResposta, System.Text.Encoding.UTF8, "application/json");

            var urlApiEol = ServiceProvider.GetService<IConfiguration>()["UrlApiEOL"];
            RegistradorDependenciasTeste.HttpHandlerFake.AdicionarCenario(urlApiEol, HttpStatusCode.OK, conteudoHttp);
        }

        // Cria DRE/UE/turma (turma com ano = ANO_7) e concede abrangência direta ao usuário (perfil comum).
        private async Task CriarCenarioComTurmaAno7PerfilComum()
        {
            ConfigurarFakeEol();

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
        // forçando o resultado a vir pelo caminho de AcrescentarTurmasSupervisor (filtro em memória).
        private async Task CriarCenarioComTurmaAno7PerfilSupervisor()
        {
            ConfigurarFakeEol();

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

        private Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string[] anosTurma)
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            return mediator.Send(
                new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(
                    UE_CODIGO_1, Modalidade.Medio, 0, false, DateTime.Now.Year, null, false, anosTurma));
        }

        [Fact(DisplayName = "Abrangência - ObterTurmas não deve retornar turma cujo ano não está na lista de anosTurma informada (perfil comum)")]
        public async Task Nao_deve_retornar_turma_de_ano_fora_do_filtro_anosTurma_perfil_comum()
        {
            await CriarCenarioComTurmaAno7PerfilComum();

            var turmas = await ObterTurmas(new[] { ANO_1, ANO_2, ANO_3 });

            turmas.ShouldBeEmpty();
        }

        [Fact(DisplayName = "Abrangência - ObterTurmas deve retornar turma cujo ano está na lista de anosTurma informada (perfil comum)")]
        public async Task Deve_retornar_turma_de_ano_dentro_do_filtro_anosTurma_perfil_comum()
        {
            await CriarCenarioComTurmaAno7PerfilComum();

            var turmas = await ObterTurmas(new[] { ANO_7 });

            turmas.ShouldNotBeNull();
            turmas.Count().ShouldBe(1);
            turmas.First().Id.ShouldBe(TURMA_ID_1);
        }

        [Fact(DisplayName = "Abrangência - ObterTurmas não deve retornar turma cujo ano não está na lista de anosTurma informada (perfil supervisor)")]
        public async Task Nao_deve_retornar_turma_de_ano_fora_do_filtro_anosTurma_perfil_supervisor()
        {
            await CriarCenarioComTurmaAno7PerfilSupervisor();

            var turmas = await ObterTurmas(new[] { ANO_1, ANO_2, ANO_3 });

            turmas.ShouldBeEmpty();
        }

        [Fact(DisplayName = "Abrangência - ObterTurmas deve retornar turma cujo ano está na lista de anosTurma informada (perfil supervisor)")]
        public async Task Deve_retornar_turma_de_ano_dentro_do_filtro_anosTurma_perfil_supervisor()
        {
            await CriarCenarioComTurmaAno7PerfilSupervisor();

            var turmas = await ObterTurmas(new[] { ANO_7 });

            turmas.ShouldNotBeNull();
            turmas.Count().ShouldBe(1);
            turmas.First().Id.ShouldBe(TURMA_ID_1);
        }
    }
}
