using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PendenciaFechamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaFechamento
{
    public class Ao_gerar_pendencia_diario_classe_frequencia_no_fechamento : PendenciaFechamentoBase
    {
        public Ao_gerar_pendencia_diario_classe_frequencia_no_fechamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Nao_gerar_pendencia_diario_classe_frequencia_com_fechamento_turma_disciplina_aberta()
        {
            var dto = new FiltroPendenciaFechamentoDto()
            {
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138
            };

            await CriarDadosBasicos(dto);
            var useCase = ObterUseCaseGerarPendenciaAulaFrequencia();
            var command = new DreUeDto(1, 1, UE_CODIGO_1);
            await useCase.Executar(new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(command) });

            var pendenciasAula = ObterTodos<Dominio.PendenciaAula>();
            pendenciasAula.ShouldBeEmpty();
        }

    }
}
