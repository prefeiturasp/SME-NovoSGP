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

namespace SME.SGP.TesteIntegracao.PendenciaAula
{
    public class Ao_gerar_pendencia_aula : TesteBaseComuns
    {
        public Ao_gerar_pendencia_aula(CollectionFixture collectionFixture) : base(collectionFixture)
        {

        }

        [Fact]
        public async Task Deve_retornar_true_se_a_data_atual_for_maior_ou_igual_ao_do_parametro_sistema()
        {
            var useCase = ServiceProvider.GetService<IPendenciaAulaUseCase>();

            var valorData = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01);

            await InserirNaBase(new ParametrosSistema
            {
                Id = 1,
                Tipo = TipoParametroSistema.DataInicioGeracaoPendencias,
                Ativo = true,
                CriadoPor = "",
                CriadoRF = "",
                Valor = valorData.ToString("yyyy/MM/dd"),
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
                Valor = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 12, 31).ToString(),
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
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 03, 01)
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

            await InserirNaBase(new Dominio.Turma()
            {
                Nome = "7P",
                CodigoTurma = "111",
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
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
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 10),
                Nome = "Calendário Escolar Ano Atual",
                Periodo = Periodo.Anual,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Excluido = false
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 03),
                ProfessorRf = "Sistema",
                DisciplinaId = "512",
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = "111",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 03, 01),
            });

            await InserirNaBase(new Dominio.PendenciaAula()
            {
                Id = 1,
                AulaId = 1,
                PendenciaId = 1
            });

            await InserirNaBase(new Usuario()
            {
                Id = 1,
                CodigoRf = USUARIO_LOGADO_RF,
                Login = USUARIO_LOGADO_RF,
                Nome = USUARIO_LOGADO_NOME,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Dominio.PendenciaUsuario()
            {
                Id = 1,
                UsuarioId = 1,
                PendenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Bimestre = 1,
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 20),
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 01),
                TipoCalendarioId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });


            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");

            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");

            await InserirNaBase("componente_curricular", "512", "512", "1", "1", "'ED.INF. EMEI 4 HS'", "false", "false", "true", "false", "false", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 4H'");

            await InserirNaBase("componente_curricular", "513", "512", "1", "1", "'ED.INF. EMEI 2 HS'", "false", "false", "true", "false", "false", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 2H'");


            var retorno = await mediator.Send(new ObterPendenciaIdPorComponenteProfessorBimestreQuery(512.ToString(), USUARIO_LOGADO_RF, 1, TipoPendencia.PlanoAula, "111", 1));

            retorno.Any().ShouldBeTrue();

            retorno.Any(f => f.PendenciaId == 1).ShouldBeTrue();
        }
    }
}
