using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.DashboardBuscaAtiva
{
    public class Ao_obter_grafico_busca_ativa_por_reflexo_frequencia : RegistroAcaoBuscaAtivaTesteBase
    {
        private const string REFLEXO_FREQUENCIA_AUMENTO = "Crianças/estudantes com aumento no percentual de frequência";
        private const string REFLEXO_FREQUENCIA_DIMINUICAO = "Crianças/estudantes com diminuição no percentual de frequência";

        public Ao_obter_grafico_busca_ativa_por_reflexo_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_obter_qdade_busca_ativa_por_reflexo_frequencia_por_mes()
        {
            await CriarDreUePerfilComponenteCurricular();
            await InserirConsolidacoesReflexoFrequencia(10);
            await CriarTurma(Modalidade.Fundamental, "6", TURMA_CODIGO_1, TipoTurma.Regular, UE_ID_1, DateTimeExtension.HorarioBrasilia().Year, nomeTurma: TURMA_NOME_1);
            await CriarTurma(Modalidade.Fundamental, "7", TURMA_CODIGO_2, TipoTurma.Regular, UE_ID_1, DateTimeExtension.HorarioBrasilia().Year, nomeTurma: TURMA_NOME_2);
            await CriarTurma(Modalidade.Fundamental, "8", TURMA_CODIGO_3, TipoTurma.Regular, UE_ID_2, DateTimeExtension.HorarioBrasilia().Year);
            await CriarTurma(Modalidade.Fundamental, "9", TURMA_CODIGO_4, TipoTurma.Regular, UE_ID_3, DateTimeExtension.HorarioBrasilia().Year);

            var useCase = ServiceProvider.GetService<IObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase>();
            var dto = new FiltroGraficoReflexoFrequenciaBuscaAtivaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Fundamental,
                Mes = 10
            };
            var retorno = await useCase.Executar(dto);

            retorno.DataUltimaConsolidacao.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(-5));
            retorno.Graficos.Any(gf => gf.Grupo.Equals("6º ano")).ShouldBeTrue();
            retorno.Graficos.Any(gf => gf.Grupo.Equals("7º ano")).ShouldBeTrue();
            retorno.Graficos.Any(gf => gf.Grupo.Equals("8º ano")).ShouldBeTrue();
            retorno.Graficos.Any(gf => gf.Grupo.Equals("9º ano")).ShouldBeTrue();

            retorno.Graficos.Where(gf => gf.Grupo.Equals("6º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO) && gf.Quantidade == 2);
            retorno.Graficos.Where(gf => gf.Grupo.Equals("6º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO) && gf.Quantidade == 1);

            retorno.Graficos.Where(gf => gf.Grupo.Equals("7º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO) && gf.Quantidade == 1);
            retorno.Graficos.Where(gf => gf.Grupo.Equals("7º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO)).ShouldBeFalse();

            retorno.Graficos.Where(gf => gf.Grupo.Equals("8º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO) && gf.Quantidade == 1);
            retorno.Graficos.Where(gf => gf.Grupo.Equals("8º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO)).ShouldBeFalse();
            
            retorno.Graficos.Where(gf => gf.Grupo.Equals("9º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO)).ShouldBeFalse();
            retorno.Graficos.Where(gf => gf.Grupo.Equals("9º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO) && gf.Quantidade == 1);

            dto = new FiltroGraficoReflexoFrequenciaBuscaAtivaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Fundamental,
                UeId = UE_ID_1,
                Mes = 10
            };
            retorno = await useCase.Executar(dto);

            retorno.DataUltimaConsolidacao.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(-5));
            retorno.Graficos.Any(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_1}")).ShouldBeTrue();
            retorno.Graficos.Any(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_2}")).ShouldBeTrue();

            retorno.Graficos.Where(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_1}")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO) && gf.Quantidade == 2);
            retorno.Graficos.Where(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_1}")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO) && gf.Quantidade == 1);

            retorno.Graficos.Where(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_2}")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO) && gf.Quantidade == 1);
            retorno.Graficos.Where(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_2}")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO)).ShouldBeFalse();
        }

        [Fact]
        public async Task Ao_obter_qdade_busca_ativa_por_reflexo_frequencia_acumulado_anual()
        {
            await CriarDreUePerfilComponenteCurricular();
            await InserirConsolidacoesReflexoFrequencia();
            await CriarTurma(Modalidade.Fundamental, "6", TURMA_CODIGO_1, TipoTurma.Regular, UE_ID_1, DateTimeExtension.HorarioBrasilia().Year, nomeTurma: TURMA_NOME_1);
            await CriarTurma(Modalidade.Fundamental, "7", TURMA_CODIGO_2, TipoTurma.Regular, UE_ID_1, DateTimeExtension.HorarioBrasilia().Year, nomeTurma: TURMA_NOME_2);
            await CriarTurma(Modalidade.Fundamental, "8", TURMA_CODIGO_3, TipoTurma.Regular, UE_ID_2, DateTimeExtension.HorarioBrasilia().Year);
            await CriarTurma(Modalidade.Fundamental, "9", TURMA_CODIGO_4, TipoTurma.Regular, UE_ID_3, DateTimeExtension.HorarioBrasilia().Year);

            var useCase = ServiceProvider.GetService<IObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase>();
            var dto = new FiltroGraficoReflexoFrequenciaBuscaAtivaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Fundamental,
                Mes = 0
            };
            var retorno = await useCase.Executar(dto);

            retorno.DataUltimaConsolidacao.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(-5));
            retorno.Graficos.Any(gf => gf.Grupo.Equals("6º ano")).ShouldBeTrue();
            retorno.Graficos.Any(gf => gf.Grupo.Equals("7º ano")).ShouldBeTrue();
            retorno.Graficos.Any(gf => gf.Grupo.Equals("8º ano")).ShouldBeTrue();
            retorno.Graficos.Any(gf => gf.Grupo.Equals("9º ano")).ShouldBeTrue();

            retorno.Graficos.Where(gf => gf.Grupo.Equals("6º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO) && gf.Quantidade == 2);
            retorno.Graficos.Where(gf => gf.Grupo.Equals("6º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO) && gf.Quantidade == 1);

            retorno.Graficos.Where(gf => gf.Grupo.Equals("7º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO) && gf.Quantidade == 1);
            retorno.Graficos.Where(gf => gf.Grupo.Equals("7º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO)).ShouldBeFalse();

            retorno.Graficos.Where(gf => gf.Grupo.Equals("8º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO) && gf.Quantidade == 1);
            retorno.Graficos.Where(gf => gf.Grupo.Equals("8º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO)).ShouldBeFalse();

            retorno.Graficos.Where(gf => gf.Grupo.Equals("9º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO)).ShouldBeFalse();
            retorno.Graficos.Where(gf => gf.Grupo.Equals("9º ano")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO) && gf.Quantidade == 1);

            dto = new FiltroGraficoReflexoFrequenciaBuscaAtivaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Fundamental,
                UeId = UE_ID_1,
                Mes = 0
            };
            retorno = await useCase.Executar(dto);

            retorno.DataUltimaConsolidacao.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(-5));
            retorno.Graficos.Any(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_1}")).ShouldBeTrue();
            retorno.Graficos.Any(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_2}")).ShouldBeTrue();

            retorno.Graficos.Where(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_1}")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO) && gf.Quantidade == 2);
            retorno.Graficos.Where(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_1}")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO) && gf.Quantidade == 1);

            retorno.Graficos.Where(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_2}")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_AUMENTO) && gf.Quantidade == 1);
            retorno.Graficos.Where(gf => gf.Grupo.Equals($"{dto.Modalidade.ObterNomeCurto()}-{TURMA_NOME_2}")).Any(gf => gf.Descricao.Equals(REFLEXO_FREQUENCIA_DIMINUICAO)).ShouldBeFalse();
        }

        private async Task InserirConsolidacoesReflexoFrequencia(int mes = 0)
        {
            await InserirNaBase(new SME.SGP.Dominio.ConsolidacaoReflexoFrequenciaBuscaAtivaAluno()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNome = ALUNO_NOME_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaCodigo = TURMA_CODIGO_1,
                UeCodigo = UE_CODIGO_1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DataBuscaAtiva = DateTimeExtension.HorarioBrasilia(),
                Modalidade = Modalidade.Fundamental,
                Mes = mes,
                PercFrequenciaAntesAcao = 50,
                PercFrequenciaAposAcao = 85,
            });
            await InserirNaBase(new SME.SGP.Dominio.ConsolidacaoReflexoFrequenciaBuscaAtivaAluno()
            {
                AlunoCodigo = CODIGO_ALUNO_2,
                AlunoNome = ALUNO_NOME_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaCodigo = TURMA_CODIGO_1,
                UeCodigo = UE_CODIGO_1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DataBuscaAtiva = DateTimeExtension.HorarioBrasilia(),
                Modalidade = Modalidade.Fundamental,
                Mes = mes,
                PercFrequenciaAntesAcao = 0,
                PercFrequenciaAposAcao = 20,
            });
            await InserirNaBase(new SME.SGP.Dominio.ConsolidacaoReflexoFrequenciaBuscaAtivaAluno()
            {
                AlunoCodigo = CODIGO_ALUNO_3,
                AlunoNome = ALUNO_NOME_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaCodigo = TURMA_CODIGO_1,
                UeCodigo = UE_CODIGO_1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DataBuscaAtiva = DateTimeExtension.HorarioBrasilia(),
                Modalidade = Modalidade.Fundamental,
                Mes = mes,
                PercFrequenciaAntesAcao = 40,
                PercFrequenciaAposAcao = 20,
            });
            await InserirNaBase(new SME.SGP.Dominio.ConsolidacaoReflexoFrequenciaBuscaAtivaAluno()
            {
                AlunoCodigo = CODIGO_ALUNO_4,
                AlunoNome = ALUNO_NOME_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaCodigo = TURMA_CODIGO_2,
                UeCodigo = UE_CODIGO_1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DataBuscaAtiva = DateTimeExtension.HorarioBrasilia(),
                Modalidade = Modalidade.Fundamental,
                Mes = mes,
                PercFrequenciaAntesAcao = 50,
                PercFrequenciaAposAcao = 85,
            });
            await InserirNaBase(new SME.SGP.Dominio.ConsolidacaoReflexoFrequenciaBuscaAtivaAluno()
            {
                AlunoCodigo = CODIGO_ALUNO_5,
                AlunoNome = ALUNO_NOME_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaCodigo = TURMA_CODIGO_3,
                UeCodigo = UE_CODIGO_2,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DataBuscaAtiva = DateTimeExtension.HorarioBrasilia(),
                Modalidade = Modalidade.Fundamental,
                Mes = mes,
                PercFrequenciaAntesAcao = 50,
                PercFrequenciaAposAcao = 85,
            });
            await InserirNaBase(new SME.SGP.Dominio.ConsolidacaoReflexoFrequenciaBuscaAtivaAluno()
            {
                AlunoCodigo = CODIGO_ALUNO_6,
                AlunoNome = ALUNO_NOME_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaCodigo = TURMA_CODIGO_4,
                UeCodigo = UE_CODIGO_3,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DataBuscaAtiva = DateTimeExtension.HorarioBrasilia(),
                Modalidade = Modalidade.Fundamental,
                Mes = mes,
                PercFrequenciaAntesAcao = 70,
                PercFrequenciaAposAcao = 30,
            });
        }
    }

}
