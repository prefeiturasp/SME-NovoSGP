using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PendenciaFechamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaFechamento
{
    public class Ao_gerar_pendencia_fechamento_frequencia : PendenciaFechamentoBase
    {
        public Ao_gerar_pendencia_fechamento_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_gerar_pendencia_fechamento_sem_frequencia()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString()
            };
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddDays(1);

            await CriarDadosBasicos(dto);
            await CriaPendenciaPorTipo(TipoPendencia.Frequencia);
            await CriarPendenciaAula(AULA_ID, PENDENCIA_ID_1);

            var useCase = ObterUseCaseGerarPendencia();
            var command = new GerarPendenciasFechamentoCommand(
                                        long.Parse(dto.ComponenteCurricularCodigo),
                                        TURMA_CODIGO_1,
                                        TURMA_NOME_1,
                                        dataReferencia.AddDays(-20),
                                        dataReferencia,
                                        BIMESTRE_2,
                                        USUARIO_ID_1,
                                        USUARIO_PROFESSOR_CODIGO_RF_2222222,
                                        FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                                        "Pendência fechamento",
                                        USUARIO_PROFESSOR_CODIGO_RF_2222222,
                                        TURMA_ID_1,
                                        string.Empty);

            await useCase.Executar(new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(command) });
            var pendeciasSemPlano = ObterTodos<Pendencia>().FindAll(p => p.Tipo == TipoPendencia.AulasSemFrequenciaNaDataDoFechamento);
            pendeciasSemPlano.ShouldNotBeNull();
            var pendeciasFechamento = ObterTodos<Dominio.PendenciaFechamento>().Find(pf => pf.PendenciaId == pendeciasSemPlano.FirstOrDefault().Id);
            pendeciasFechamento.ShouldNotBeNull();
            var pendenciasAula = ObterTodos<Dominio.PendenciaAula>().Select(pendenciaAula => pendenciaAula.PendenciaId);
            var pendencias = ObterTodos<Dominio.Pendencia>().Where(pendencia => pendenciasAula.Contains(pendencia.Id));
            pendenciasAula.Count().ShouldBe(1);
            pendencias.Count(pendencia => !pendencia.Excluido && pendencia.Tipo == TipoPendencia.Frequencia).ShouldBe(1);
            pendencias.Any(pendencia => pendencia.Excluido).ShouldBeFalse();
        }

        [Fact]
        public async Task Ao_gerar_pendencia_fechamento_com_frequencia()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString()
            };
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddDays(1);

            await CriarDadosBasicos(dto);
            await CriaPendenciaPorTipo(TipoPendencia.Frequencia);
            await CriarPendenciaAula(AULA_ID, PENDENCIA_ID_1);
            await CriaFrequencia();

            var useCase = ObterUseCaseGerarPendencia();
            var command = new GerarPendenciasFechamentoCommand(
                                        long.Parse(dto.ComponenteCurricularCodigo),
                                        TURMA_CODIGO_1,
                                        TURMA_NOME_1,
                                        dataReferencia.AddDays(-20),
                                        dataReferencia,
                                        BIMESTRE_2,
                                        USUARIO_ID_1,
                                        USUARIO_PROFESSOR_CODIGO_RF_2222222,
                                        FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                                        "Pendência fechamento",
                                        USUARIO_PROFESSOR_CODIGO_RF_2222222,
                                        TURMA_ID_1,
                                        string.Empty);

            await useCase.Executar(new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(command) });
            var pendeciasSemPlano = ObterTodos<Pendencia>();
            pendeciasSemPlano.ShouldNotBeNull();
            pendeciasSemPlano.Exists(p => p.Tipo == TipoPendencia.AulasSemFrequenciaNaDataDoFechamento).ShouldBeFalse();
            var pendenciasAula = ObterTodos<Dominio.PendenciaAula>().Select(pendenciaAula => pendenciaAula.PendenciaId);
            var pendencias = ObterTodos<Dominio.Pendencia>().Where(pendencia => pendenciasAula.Contains(pendencia.Id));
            pendenciasAula.Count().ShouldBe(1);
            pendencias.Count(pendencia => pendencia.Excluido && pendencia.Tipo == TipoPendencia.Frequencia).ShouldBe(1);
            pendencias.Any(pendencia => pendencia.Excluido).ShouldBeTrue();
        }

        private async Task CriaFrequencia()
        {
            await InserirNaBase(new Dominio.RegistroFrequencia()
            {
                AulaId = AULA_ID,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 03, 01),
            });
        }
    }
}
