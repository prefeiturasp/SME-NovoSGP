using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PendenciaFechamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaFechamento
{
    public class Ao_gerar_pendencia_fechamento_aprovado : PendenciaFechamentoBase
    {
        public Ao_gerar_pendencia_fechamento_aprovado(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_gerar_pendencia_fechamento_gravar_pendencia_fechamento_aula()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138
            };
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddDays(1);

            await CriarDadosBasicos(dto);
            await CriarAula(DateTimeExtension.HorarioBrasilia().AddDays(-1), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_2222222, TURMA_CODIGO_1, UE_CODIGO_1, dto.ComponenteCurricularCodigo, TIPO_CALENDARIO_1);
            await CriarAula(DateTimeExtension.HorarioBrasilia().AddDays(-1), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_2222222, TURMA_CODIGO_1, UE_CODIGO_1, dto.ComponenteCurricularCodigo, TIPO_CALENDARIO_1);

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
            var pendencia = ObterTodos<Pendencia>().Find(p => p.Tipo == TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);
            pendencia.ShouldNotBeNull();
            var pendenciafechamento = ObterTodos<Dominio.PendenciaFechamento>().Find(pf => pf.PendenciaId == pendencia.Id);
            pendenciafechamento.ShouldNotBeNull();
            var pendeciasFechamentoAula = ObterTodos<PendenciaFechamentoAula>().FindAll(pfa => pfa.PendenciaFechamentoId == pendenciafechamento.Id);
            pendeciasFechamentoAula.ShouldNotBeNull();
            pendeciasFechamentoAula.Count().ShouldBe(3);
        }


        [Fact]
        public async Task Ao_gerar_pendencia_fechamento_aprovada_sem_plano_de_aula()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138
            };
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddDays(1);

            await CriarDadosBasicos(dto);
            await CriaPendenciaPorTipo(TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento, SituacaoPendencia.Aprovada);
            await CriarPendenciaFechamento(FECHAMENTO_TURMA_DISCIPLINA_ID_1, PENDENCIA_ID_1);
            await CriarPendenciaFechamentoAula(AULA_ID, PENDENCIA_FECHAMENTO_ID_1);
            await CriarAula(DateTimeExtension.HorarioBrasilia().AddDays(-1), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_2222222, TURMA_CODIGO_1, UE_CODIGO_1, dto.ComponenteCurricularCodigo, TIPO_CALENDARIO_1);
            await CriarAula(DateTimeExtension.HorarioBrasilia().AddDays(-1), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_2222222, TURMA_CODIGO_1, UE_CODIGO_1, dto.ComponenteCurricularCodigo, TIPO_CALENDARIO_1);

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
            var pendencia = ObterTodos<Pendencia>().Find(p => p.Tipo == TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento && p.Situacao == SituacaoPendencia.Pendente);
            pendencia.ShouldNotBeNull();
            var pendenciafechamento = ObterTodos<Dominio.PendenciaFechamento>().Find(pf => pf.PendenciaId == pendencia.Id);
            pendenciafechamento.ShouldNotBeNull();
            var pendeciasFechamentoAula = ObterTodos<PendenciaFechamentoAula>().FindAll(pfa => pfa.PendenciaFechamentoId == pendenciafechamento.Id);
            pendeciasFechamentoAula.ShouldNotBeNull();
            pendeciasFechamentoAula.Count().ShouldBe(2);
        }

        [Fact]
        public async Task Ao_gerar_pendencia_fechamento_aprovada_sem_frequencia()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138
            };
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddDays(1);

            await CriarDadosBasicos(dto);
            await CriaPendenciaPorTipo(TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, SituacaoPendencia.Aprovada);
            await CriarPendenciaFechamento(FECHAMENTO_TURMA_DISCIPLINA_ID_1, PENDENCIA_ID_1);
            await CriarPendenciaFechamentoAula(AULA_ID, PENDENCIA_FECHAMENTO_ID_1);
            await CriarAula(DateTimeExtension.HorarioBrasilia().AddDays(-1), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_2222222, TURMA_CODIGO_1, UE_CODIGO_1, dto.ComponenteCurricularCodigo, TIPO_CALENDARIO_1);
            await CriarAula(DateTimeExtension.HorarioBrasilia().AddDays(-1), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_2222222, TURMA_CODIGO_1, UE_CODIGO_1, dto.ComponenteCurricularCodigo, TIPO_CALENDARIO_1);

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
            var pendencia = ObterTodos<Pendencia>().Find(p => p.Tipo == TipoPendencia.AulasSemFrequenciaNaDataDoFechamento && p.Situacao == SituacaoPendencia.Pendente);
            pendencia.ShouldNotBeNull();
            var pendenciafechamento = ObterTodos<Dominio.PendenciaFechamento>().Find(pf => pf.PendenciaId == pendencia.Id);
            pendenciafechamento.ShouldNotBeNull();
            var pendeciasFechamentoAula = ObterTodos<PendenciaFechamentoAula>().FindAll(pfa => pfa.PendenciaFechamentoId == pendenciafechamento.Id);
            pendeciasFechamentoAula.ShouldNotBeNull();
            pendeciasFechamentoAula.Count().ShouldBe(2);
        }

        [Fact]
        public async Task Ao_gerar_pendencia_fechamento_aprovada_avaliacao_sem_nota()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138
            };
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddDays(1);

            await CriarDadosBasicos(dto);
            await CriaPendenciaPorTipo(TipoPendencia.AvaliacaoSemNotaParaNenhumAluno, SituacaoPendencia.Aprovada);
            await CriarPendenciaFechamento(FECHAMENTO_TURMA_DISCIPLINA_ID_1, PENDENCIA_ID_1);
            await CriarAtividadeAvaliativaFundamental(dataReferencia);
            await CriarPendenciaFechamentoAtividadeAvaliativa(ATIVIDADE_AVALIATIVA_1, PENDENCIA_FECHAMENTO_ID_1);
            await CriarAtividadeAvaliativa(dataReferencia, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), USUARIO_PROFESSOR_CODIGO_RF_2222222, false, ATIVIDADE_AVALIATIVA_2);

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
            var pendencia = ObterTodos<Pendencia>().Find(p => p.Tipo == TipoPendencia.AvaliacaoSemNotaParaNenhumAluno && p.Situacao == SituacaoPendencia.Pendente);
            pendencia.ShouldNotBeNull();
            var pendenciafechamento = ObterTodos<Dominio.PendenciaFechamento>().Find(pf => pf.PendenciaId == pendencia.Id);
            pendenciafechamento.ShouldNotBeNull();
            var pendeciasFechamentoAtividade = ObterTodos<PendenciaFechamentoAtividadeAvaliativa>().FindAll(pfa => pfa.PendenciaFechamentoId == pendenciafechamento.Id);
            pendeciasFechamentoAtividade.ShouldNotBeNull();
            pendeciasFechamentoAtividade.Count().ShouldBe(1);
        }
    }
}
