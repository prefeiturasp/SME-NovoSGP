using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_salvar_consolidado_atendimento_encaminhamento_naapa : EncaminhamentoNAAPATesteBase
    {
        public Ao_salvar_consolidado_atendimento_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodasUesIdsQuery, IEnumerable<long>>), typeof(ObterTodasUesIdsQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandExecutarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Deve Retornar True e gerar os registros consolidados ao Executar Rotina de Consolidação")]
        public async Task Deve_retornar_true_e_gerar_consolidacao_ao_executar_rotina()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Encerrado,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecaoItinerancia(5, USUARIO_PROFESSOR_NOME_2222222, USUARIO_PROFESSOR_CODIGO_RF_2222222);
            await CriarEncaminhamentoNAAPASecaoItinerancia(2, USUARIO_PROFESSOR_NOME_1111111, USUARIO_PROFESSOR_CODIGO_RF_1111111);
            await CriarEncaminhamentoNAAPASecaoItinerancia(2, USUARIO_PROFESSOR_NOME_1111111, USUARIO_PROFESSOR_CODIGO_RF_1111111, true); //2 registros excluidos que devem ser ignorados do processo
            await CriarQuestoesItineranciaEncaminhamentoNAAPA(9);
            await CriarRespostasItineranciaEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date, 9);


            var useCase = ServiceProvider.GetService<IExecutarBuscarConsolidadoAtendimentosProfissionalAtendimentoNAAPAUseCase>();
            var filtro = new FiltroBuscarAtendimentosProfissionalConsolidadoAtendimentoNAAPADto(1, DateTimeExtension.HorarioBrasilia().Month, DateTimeExtension.HorarioBrasilia().Year);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            var retorno = await useCase.Executar(mensagem);
            retorno.ShouldBeTrue();
            var consolidacoes = ObterTodos<ConsolidadoAtendimentoNAAPA>();
            consolidacoes.ShouldNotBeNull("Registros de consolidações deveriam existir");
            consolidacoes.Count.ShouldBe(2, "2 Registros de consolidações deveriam existir (itinerâncias existentes para 2 usuários)");
            var rf1 = consolidacoes.Find(x => x.RfProfissional == USUARIO_PROFESSOR_CODIGO_RF_1111111);
            var rf2 = consolidacoes.Find(x => x.RfProfissional == USUARIO_PROFESSOR_CODIGO_RF_2222222);
            rf2.ShouldNotBeNull("1 Registro de consolidações deveriam existir para o usuário 2222222");
            rf1.ShouldNotBeNull("1 Registro de consolidações deveriam existir para o usuário 1111111");
            rf2.Quantidade.ShouldBe(5, "5 Atendimentos deveriam ser consolidados para o usuário 2222222");
            rf1.Quantidade.ShouldBe(2, "2 Atendimentos deveriam ser consolidados para o usuário 1111111");
            rf2.Modalidade.ShouldBe(Modalidade.Fundamental);
            rf1.Modalidade.ShouldBe(Modalidade.Fundamental);
        }

        private async Task CriarRespostasItineranciaEncaminhamentoNAAPA(DateTime dataAtendimento, int qdadeSecoes = 1)
        {
            for (int i = 1; i <= qdadeSecoes; i++)
            {
                await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
                {
                    QuestaoEncaminhamentoId = i,
                    Texto = (new DateTime(dataAtendimento.Year, dataAtendimento.Month, i)).ToString("yyyy-MM-dd"),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }

        private async Task CriarQuestoesItineranciaEncaminhamentoNAAPA(int qdadeSecoes = 1)
        {
            for (int i = 1; i <= qdadeSecoes; i++)
            {
                await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
                {
                    EncaminhamentoNAAPASecaoId = i,
                    QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }

        private async Task CriarEncaminhamentoNAAPASecaoItinerancia(int qdadeSecoes = 1, string criadoPor = SISTEMA_NOME, string criadoRf = SISTEMA_CODIGO_RF, bool excluido = false)
        {
            for (int i = 1; i <= qdadeSecoes; i++)
            {
                await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
                {
                    EncaminhamentoNAAPAId = 1,
                    SecaoEncaminhamentoNAAPAId = ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = criadoRf,
                    Excluido = excluido
                });
            }
        }

        private async Task CriarEncaminhamentoNAAPA(SituacaoNAAPA situacao = SituacaoNAAPA.Encerrado, long turmaId = TURMA_ID_1, string alunoCodigo = ALUNO_CODIGO_1)
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = turmaId,
                AlunoCodigo = alunoCodigo,
                Situacao = situacao,
                AlunoNome = $"Nome do aluno {ALUNO_CODIGO_1}",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}