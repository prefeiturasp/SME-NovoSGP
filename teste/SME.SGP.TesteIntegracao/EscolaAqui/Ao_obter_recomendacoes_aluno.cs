using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EscolaAqui
{
    public class Ao_obter_recomendacoes_aluno : TesteBaseComuns
    {

        private const string RECOMENDACOES_ALUNO_1 = "Recomendações Aluno 1";
        private const string RECOMENDACOES_FAMILIA_ALUNO_1 = "Recomendações Família Aluno 1";
        private const string ANOTACOES_PEDAGOGICAS_ALUNO_1 = "Anotações Pedagógicas Aluno 1";
        private const string RECOMENDACOES_ALUNO_PADRAO = "Recomendações Aluno PADRÃO";
        private const string RECOMENDACOES_FAMILIA_PADRAO = "Recomendações Família PADRÃO";
        private int ANO_ATUAL = DateTimeExtension.HorarioBrasilia().Year;
        public Ao_obter_recomendacoes_aluno(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);     
        }

        [Fact(DisplayName = "Retornar recomendações de conselho de classe do aluno/turma informado")]
        public async Task Deve_retornar_recomendacoes_aluno_turma()
        {
            await CriarItensBasicos();
            await CriarConselhoClasseRecomendacao();
            await CriarConselhoClasseFechamentoTurma(RECOMENDACOES_ALUNO_1, RECOMENDACOES_FAMILIA_ALUNO_1, ANOTACOES_PEDAGOGICAS_ALUNO_1);

            var useCase = ServiceProvider.GetService<IObterRecomendacoesPorAlunoTurmaUseCase>();

            var retorno = await useCase.Executar(new Infra.FiltroRecomendacaoConselhoClasseAlunoTurmaDto() { AnoLetivo = ANO_ATUAL, CodigoAluno = CODIGO_ALUNO_1, CodigoTurma = TURMA_CODIGO_1, Modalidade = (int)Modalidade.Fundamental});
            retorno.ShouldNotBeEmpty();
            retorno.Count().ShouldBe(1);
            retorno.Where(e => e.AlunoCodigo == CODIGO_ALUNO_1 && e.TurmaCodigo == TURMA_CODIGO_1 
                            && e.RecomendacoesAluno.Contains(RECOMENDACOES_ALUNO_1) && e.RecomendacoesFamilia.Contains(RECOMENDACOES_FAMILIA_ALUNO_1)).Count().ShouldBe(1);
        }

        [Fact(DisplayName = "Retornar recomendações de conselho de classe do aluno/turma não informado - Geral")]
        public async Task Deve_retornar_recomendacoes_aluno_turma_nao_informado__geral()
        {
            await CriarItensBasicos();
            await CriarConselhoClasseRecomendacao();
            await CriarConselhoClasseFechamentoTurma();

            var useCase = ServiceProvider.GetService<IObterRecomendacoesPorAlunoTurmaUseCase>();

            var retorno = await useCase.Executar(new Infra.FiltroRecomendacaoConselhoClasseAlunoTurmaDto() { AnoLetivo = ANO_ATUAL, CodigoAluno = CODIGO_ALUNO_1, CodigoTurma = TURMA_CODIGO_1, Modalidade = (int)Modalidade.Fundamental });
            retorno.ShouldNotBeEmpty();
            retorno.Count().ShouldBe(1);
            retorno.Where(e => e.AlunoCodigo == CODIGO_ALUNO_1 && e.TurmaCodigo == TURMA_CODIGO_1
                            && e.RecomendacoesAluno.Contains(RECOMENDACOES_ALUNO_PADRAO) && e.RecomendacoesFamilia.Contains(RECOMENDACOES_FAMILIA_PADRAO)).Count().ShouldBe(1);
        }

        [Fact(DisplayName = "Retornar recomendações de conselho de classe do aluno/turma selecionadas")]
        public async Task Deve_retornar_recomendacoes_aluno_turma_informado_selecao()
        {
            await CriarItensBasicos();
            await CriarConselhoClasseRecomendacao();
            await CriarConselhoClasseFechamentoTurma("","","");
            await CriarConselhoClasseRecomendacaoAluno();

            var useCase = ServiceProvider.GetService<IObterRecomendacoesPorAlunoTurmaUseCase>();

            var retorno = await useCase.Executar(new Infra.FiltroRecomendacaoConselhoClasseAlunoTurmaDto() { AnoLetivo = ANO_ATUAL, CodigoAluno = CODIGO_ALUNO_1, CodigoTurma = TURMA_CODIGO_1, Modalidade = (int)Modalidade.Fundamental });
            retorno.ShouldNotBeEmpty();
            retorno.Count().ShouldBe(1);
            retorno.Where(e => e.AlunoCodigo == CODIGO_ALUNO_1 && e.TurmaCodigo == TURMA_CODIGO_1
                            && e.RecomendacoesAluno.Contains(RECOMENDACOES_ALUNO_PADRAO) && e.RecomendacoesFamilia.Contains(RECOMENDACOES_FAMILIA_PADRAO)).Count().ShouldBe(1);
        }

        //conselho_classe_aluno_recomendacao
        private async Task CriarConselhoClasseFechamentoTurma(string recomendacaoAluno = null, string recomendacaoFamilia = null, string anotacoesPadagogicas = null)
        {
            await InserirNaBase(new FechamentoTurma()
            {
                Id = 1,
                PeriodoEscolarId = 1,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });

            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = 1,
                Situacao = SituacaoConselhoClasse.EmAndamento,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });

            await InserirNaBase(new ConselhoClasseAluno()
            {
                ConselhoClasseId = 1,
                RecomendacoesAluno = recomendacaoAluno,
                RecomendacoesFamilia = recomendacaoFamilia,
                AnotacoesPedagogicas = anotacoesPadagogicas,
                AlunoCodigo = CODIGO_ALUNO_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
        }

        private async Task CriarConselhoClasseRecomendacaoAluno()
        {
            await InserirNaBase(new ConselhoClasseAlunoRecomendacao()
            {
                ConselhoClasseAlunoId = 1,
                ConselhoClasseRecomendacaoId = 1
            });
            await InserirNaBase(new ConselhoClasseAlunoRecomendacao()
            {
                ConselhoClasseAlunoId = 1,
                ConselhoClasseRecomendacaoId = 2
            });
        }

        private async Task CriarConselhoClasseRecomendacao()
        {
            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Recomendacao = RECOMENDACOES_ALUNO_PADRAO,
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Recomendacao = RECOMENDACOES_FAMILIA_PADRAO,
                Tipo = ConselhoClasseRecomendacaoTipo.Familia,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = DRE_ID_1,
                CodigoDre = DRE_CODIGO_1
            });

            await InserirNaBase(new Ue
            {
                Id = UE_ID_1,
                CodigoUe = UE_CODIGO_1,
                DreId = DRE_ID_1
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = TURMA_ID_1,
                UeId = UE_ID_1,
                Ano = "1",
                CodigoTurma = TURMA_CODIGO_1,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = ANO_ATUAL
            });
            await InserirNaBase(new TipoCalendario()
            {
                Id = TIPO_CALENDARIO_1,
                AnoLetivo = ANO_ATUAL,
                Nome = "Calendário Teste Ano Atual",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                Periodo = Periodo.Anual,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
            await InserirNaBase(new PeriodoEscolar
            {
                Id = PERIODO_ESCOLAR_CODIGO_1,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                PeriodoInicio = new DateTime(ANO_ATUAL, 01, 03),
                PeriodoFim = new DateTime(ANO_ATUAL, 12, 29),
                Bimestre = 1,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 15, 23, 48, 43),
                CriadoPor = "Sistema",
                AlteradoEm = null,
                AlteradoPor = "",
                CriadoRF = "0",
                AlteradoRF = null,
                Migrado = false
            });
        }
    }
}