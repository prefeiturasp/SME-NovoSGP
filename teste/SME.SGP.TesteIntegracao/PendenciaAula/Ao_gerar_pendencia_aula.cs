using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;
using Xunit;
using SME.SGP.Infra;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using MediatR;
using System.Linq;

namespace SME.SGP.TesteIntegracao.TestarPendenciaAula
{
    public class Ao_gerar_pendencia_aula : TesteBase
    {
        public Ao_gerar_pendencia_aula(CollectionFixture collectionFixture) : base(collectionFixture)
        {

        }

        [Fact]
        public async Task Deve_retornar_true_se_a_data_atual_for_maior_ou_igual_ao_do_parametro_sistema()
        {
            var useCase = ServiceProvider.GetService<IPendenciaAulaUseCase>();

            await InserirNaBase(new ParametrosSistema
            {
                Id = 1,
                Tipo = TipoParametroSistema.DataInicioGeracaoPendencias,
                Ativo = true,
                CriadoPor = "",
                CriadoRF = "",
                Valor = "01/02/2022",
                Nome = "DataInicioGeracaoPendencias",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Descricao = "Data de início da geração de pendências"
            });

            var retorno = await useCase.Executar(new MensagemRabbit(""));

            retorno.ShouldBeTrue();
        }

        [Fact]
        public async Task Deve_retornar_false_se_a_data_atual_for_menor_do_parametro_sistema()
        {
            var useCase = ServiceProvider.GetService<IPendenciaAulaUseCase>();

            await InserirNaBase(new ParametrosSistema
            {
                Id = 1,
                Tipo = TipoParametroSistema.DataInicioGeracaoPendencias,
                Ativo = true,
                CriadoPor = "",
                CriadoRF = "",
                Valor = new DateTime(DateTime.Now.Year, 12, 31).ToString(),
                Nome = "DataInicioGeracaoPendencias",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Descricao = "Data de início da geração de pendências"
            });

            var retorno = await useCase.Executar(new MensagemRabbit(""));

            retorno.ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_retornar_pendencia_id_para_o_professor()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await InserirNaBase(new Pendencia()
            {
                Tipo = TipoPendencia.PlanoAula,
                Descricao = "Contém pendência de plano de aula para as seguintes aulas:",
                Titulo = "Aulas com pendência de plano de aula:  01/03",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 03, 01)
            });

            await InserirNaBase(new Dre()
            {
                Nome = "Dre Teste",
                CodigoDre = "11",
                Abreviacao = "DT"
            });

            await InserirNaBase(new Ue()
            {
                Nome = "Ue Teste",
                DreId = 1,
                TipoEscola = TipoEscola.EMEF,
                CodigoUe = "22"
            });

            await InserirNaBase(new Turma()
            {
                Nome = "7P",
                CodigoTurma = "2372753",
                Ano = "1",
                AnoLetivo = 2022,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                Situacao = true,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 10),
                Nome = "Calendário Escolar 2022",
                Periodo = Periodo.Anual,
                AnoLetivo = 2022,
                Excluido = false
            });

            await InserirNaBase(new Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(2022, 01, 03),
                ProfessorRf = "Sistema",
                DisciplinaId = "512",
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = "2372753",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 03, 01),
            });

            await InserirNaBase(new PendenciaAula()
            {
                Id = 1,
                AulaId = 1,
                PendenciaId = 1
            });

            await InserirNaBase(new Usuario()
            {
                Id = 1,
                CodigoRf = "7111111",
                Login = "7111111",
                Nome = "Usuario Teste",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new PendenciaUsuario()
            {
                Id = 1,
                UsuarioId = 1,
                PendenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Bimestre = 1,
                PeriodoFim = new DateTime(2022, 04, 20),
                PeriodoInicio = new DateTime(2022, 02, 01),
                TipoCalendarioId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });


            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");

            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");

            await InserirNaBase("componente_curricular", "512", "512", "1", "1", "'ED.INF. EMEI 4 HS'", "false", "false", "true", "false", "false", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 4H'");

            await InserirNaBase("componente_curricular", "513", "512", "1", "1", "'ED.INF. EMEI 2 HS'", "false", "false", "true", "false", "false", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 2H'");


            var retorno = await mediator.Send(new ObterPendenciaIdPorComponenteProfessorBimestreQuery(512, "7111111", 1, TipoPendencia.PlanoAula, "2372753", 1));

            retorno.Any().ShouldBeTrue();

            retorno.Any(f => f.PendenciaId == 1).ShouldBeTrue();
        }
    }
}
