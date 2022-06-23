using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaDevolutiva
{
    public class Ao_obter_pendencias : TesteBase
    {
        public Ao_obter_pendencias(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_retornar_pendencia_devolutiva()
        {
            await CriarItensBasicosPendenciaDevolutiva();

            var useCase = ServiceProvider.GetService<IObterPendenciasUseCase>();

            var resultados = await useCase.Executar("1", (int)TipoPendencia.Devolutiva, "Devolutiva - CEMEI LEILA GALLACCI METZKER, PROFA (DRE  BT) - REGÊNCIA INFANTIL EMEI 4H");

            resultados.Items.ShouldNotBeNull();
            resultados.Items.Count().ShouldBe(1);
        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1
            });

            await InserirNaBase(new Turma()
            {
                Id = 1,
                UeId = 1,
                Ano = "1",
                CodigoTurma = "1",
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = 2022
            });

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = 2022,
                Nome = "Calendário Teste 2022",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Periodo = Periodo.Anual,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });

            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 3,
                PeriodoInicio = new DateTime(2022, 06, 01),
                PeriodoFim = new DateTime(2022, 08, 30),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                Migrado = false
            });
        }

        private async Task CriarUsuarioLogadoCP()
        {
            await InserirNaBase(new Usuario()
            {
                Id = 1,
                Login = "8888888",
                CodigoRf = "8888888",
                Nome = "Usuario CP",
                CriadoPor = "Sistema",
                CriadoRF = "0",
                AlteradoRF = "0"
            });

            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();

            var variaveis = new Dictionary<string, object>
            {
                { "NomeUsuario", "Usuario CP" },
                { "UsuarioLogado", "8888888" },
                { "RF", "8888888" },
                { "login", "8888888" },
                {
                    "Claims", new List<InternalClaim> {
                        new InternalClaim { Value = "8888888", Type = "rf" },
                        new InternalClaim { Value = "44E1E074-37D6-E911-ABD6-F81654FE895D", Type = "perfil" }
                    }
                }
            };

            contextoAplicacao.AdicionarVariaveis(variaveis);
        }


        private async Task CriarItensBasicosPendenciaDevolutiva()
        {
            await CriarUsuarioLogadoCP();
            await CriarItensBasicos();

            await InserirNaBase(new Pendencia(TipoPendencia.Devolutiva)
            {
                Id = 1,
                Titulo = "Devolutiva - CEMEI LEILA GALLACCI METZKER, PROFA (DRE  BT) - REGÊNCIA INFANTIL EMEI 4H",
                Descricao = "O componente REGÊNCIA INFANTIL EMEI 4H da turma EI-7G da CEMEI LEILA GALLACCI METZKER, PROFA (DRE  BT) está há mais de 25 dias sem registro de devolutiva para os diários de bordo.",
                Situacao = SituacaoPendencia.Pendente,
                Excluido = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0",
                Instrucao = "Esta pendência será resolvida automaticamente quando o registro da devolutiva for regularizado.",
                UeId = 1
            });

            var pendenciaPerfil = new PendenciaPerfil()
            {
                Id = 1,
                PerfilCodigo = PerfilUsuario.CP,
                PendenciaId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            };

            pendenciaPerfil.AdicionaPendenciaPerfilUsuario(new PendenciaPerfilUsuario(1, 1, PerfilUsuario.CP)
            {
                Id = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });

            await InserirNaBase(pendenciaPerfil);

            await InserirNaBase(new PendenciaUsuario()
            {
                Id = 1,
                UsuarioId = 1,
                PendenciaId = 1,
                CriadoPor = "Sistema",
                CriadoRF = "0",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });

            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Area Conhecimento 1'");

            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo Matriz 1'");

            await InserirNaBase("componente_curricular", "512", "512", "1", "1", "'ED.INF. EMEI 4 HS'", "false", "false", "false", "false", "true", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 4H'");

            await InserirNaBase(new Dominio.PendenciaDevolutiva()
            {
                Id = 1,
                PedenciaId = 1,
                ComponenteCurricularId = 512,
                TurmaId = 1
            });

            await InserirNaBase("pendencia_registro_individual", "default", "''", "'0'", "'2022-06-07'", "'Sistema'", "'0'", "'2022-06-07'", "1", "1");
        }
    }
}
