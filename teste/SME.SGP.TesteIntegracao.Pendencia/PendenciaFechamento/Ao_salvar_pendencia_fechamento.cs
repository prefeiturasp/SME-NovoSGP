using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.PendenciaFechamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaFechamento
{
    public class Ao_salvar_pendencia_fechamento : PendenciaFechamentoBase
    {
        public Ao_salvar_pendencia_fechamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_gravar_pendencia_fechamento_aula()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138
            };
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddDays(1);

            await CriarDadosBasicos(dto);
            await CriaPendenciaPorTipo(TipoPendencia.AulasSemFrequenciaNaDataDoFechamento);
            await CriarPendenciaFechamento(FECHAMENTO_TURMA_DISCIPLINA_ID_1, PENDENCIA_ID_1);

            var mediator = ServiceProvider.GetService<IMediator>();

            await mediator.Send(new SalvarPendenciaFechamentoAulaCommand(AULA_ID, PENDENCIA_FECHAMENTO_ID_1));

            var pendeciasFechamentoAula = ObterTodos<PendenciaFechamentoAula>();
            pendeciasFechamentoAula.ShouldNotBeNull();
            pendeciasFechamentoAula.Exists(pfa => pfa.AulaId == AULA_ID && pfa.PendenciaFechamentoId == PENDENCIA_FECHAMENTO_ID_1).ShouldBeTrue();
        }

        [Fact]
        public async Task Ao_gravar_pendencia_fechamento_atividade_avaliativa()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138
            };
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddDays(1);

            await CriarDadosBasicos(dto);
            await CriaPendenciaPorTipo(TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);
            await CriarPendenciaFechamento(FECHAMENTO_TURMA_DISCIPLINA_ID_1, PENDENCIA_ID_1);
            await CriarAtividadeAvaliativaFundamental(dataReferencia);

            var mediator = ServiceProvider.GetService<IMediator>();

            await mediator.Send(new SalvarPendenciaFechamentoAtividadeAvaliativaCommand(ATIVIDADE_AVALIATIVA_1, PENDENCIA_FECHAMENTO_ID_1));

            var pendeciasFechamentoAula = ObterTodos<PendenciaFechamentoAtividadeAvaliativa>();
            pendeciasFechamentoAula.ShouldNotBeNull();
            pendeciasFechamentoAula.Exists(pfa => pfa.AtividadeAvaliativaId == ATIVIDADE_AVALIATIVA_1 && pfa.PendenciaFechamentoId == PENDENCIA_FECHAMENTO_ID_1).ShouldBeTrue();
        }
    }
}
