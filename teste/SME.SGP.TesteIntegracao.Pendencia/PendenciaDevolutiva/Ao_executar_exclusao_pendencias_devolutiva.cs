using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaDevolutiva
{
    public class Ao_executar_exclusao_pendencias_devolutiva : TesteBase
    {
        public Ao_executar_exclusao_pendencias_devolutiva(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_excluir_pendencias_devolutiva_sem_diario_bordo()
        {
            await CriarParametros();
            await CriarItensBasicos();
            await CriarAula();
            await CriarPendenciaDevolutiva();

            var useCase = ServiceProvider.GetService<IExecutarExclusaoPendenciasDevolutivaUseCase>();

            var json = JsonConvert.SerializeObject(new FiltroExclusaoPendenciasDevolutivaDto
            { 
                TurmaId = 1,
                ComponenteId = 512
            });

            var retorno = await useCase.Executar(new MensagemRabbit(json));

            retorno.ShouldBeTrue();
        }

        private async Task CriarParametros()
        {
            await InserirNaBase(new ParametrosSistema
            {
                Id = 1,
                Tipo = TipoParametroSistema.PeriodoDeDiasDevolutiva,
                Ativo = true,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0",
                Valor = "8",
                Nome = "PeriodoDeDiasDevolutiva",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Descricao = "Periodo considerado para consolidação das devolutivas"
            });

        }

        private async Task CriarPendenciaDevolutiva()
        {
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

            await InserirNaBase(new Dominio.PendenciaDevolutiva()
            {
                Id = 1,
                PedenciaId = 1,
                ComponenteCurricularId = 512,
                TurmaId = 1
            });
        }

        private async Task CriarAula()
        {
            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 3,
                PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddDays(-7),
                PeriodoFim = DateTimeExtension.HorarioBrasilia().AddDays(60),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                Migrado = false
            });

            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Area Conhecimento 1'");

            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo Matriz 1'");

            await InserirNaBase("componente_curricular", "512", "512", "1", "1", "'ED.INF. EMEI 4 HS'", "false", "false", "false", "false", "true", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 4H'");

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 1,
                UeId = "1",
                DisciplinaId = "512",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "Sistema",
                Quantidade = 1,
                DataAula = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                CriadoEm = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                CriadoPor = "Sistema",
                CriadoRF = "Sistema",
                Excluido = false,
                Migrado = false,
                Status = EntidadeStatus.Aprovado,
                AulaCJ = false
            });

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                Id = 1,
                AulaId = 1,
                ComponenteCurricularId = 512,
                TurmaId = 1,
                DevolutivaId = null,
                Planejamento = "Planejado",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
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

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                UeId = 1,
                Ano = "1",
                CodigoTurma = "1",
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year
            });

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Calendário Teste Ano Atual",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Periodo = Periodo.Anual,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
        }
    }
}
