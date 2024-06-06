using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Constantes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using SME.SGP.Infra.Dtos.ProvaSP;
using SME.SGP.Infra.Dtos.Sondagem;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{
    public class Ao_executar_atualizacao_informacoes_mapeamento_estudante : MapeamentoBase
    {
        public Ao_executar_atualizacao_informacoes_mapeamento_estudante(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandFakeAtualizacaoMapEstudante), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQuery, InformacoesAtualizadasMapeamentoEstudanteAlunoDto>), typeof(ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery, InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto>), typeof(ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoEnderecoEolQuery, AlunoEnderecoRespostaDto>), typeof(ObterAlunoEnderecoEolQueryAlunoMigranteFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterSondagemLPAlunoQuery, SondagemLPAlunoDto>), typeof(ObterSondagemLPAlunoQueryNaoAlfabeticoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterConsultaFrequenciaGeralAlunoQuery, string>), typeof(ObterConsultaFrequenciaGeralAlunoQueryAlunoFrequenteFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAvaliacoesExternasProvaSPAlunoQuery, IEnumerable<AvaliacaoExternaProvaSPDto>>), typeof(ObterAvaliacoesExternasProvaSPAlunoQueryFake), ServiceLifetime.Scoped));
        }

        protected override async Task CriarDadosBase()
        {
            await base.CriarDadosBase();
            CarregarDadosBase();
        }

        
        [Fact(DisplayName = "Mapeamento Estudante - Atualizar informações")]
        public async Task Ao_atualizar_informacoes_mapeamento_estudante()
        {
            await CriarDadosBase();
            await GerarDadosMapeamentosEstudantes_2(BIMESTRE_4);

            var useCase = ServiceProvider.GetService<IAtualizarMapeamentoDosEstudantesUseCase>();
            var retorno = await useCase.Executar(new MensagemRabbit(DateTimeExtension.HorarioBrasilia().Date));
            retorno.ShouldBeTrue();

            var questaoMapeamento = ObterTodos<QuestaoMapeamentoEstudante>();
            var respostaMapeamento = ObterTodos<RespostaMapeamentoEstudante>();
            
            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("{\"index\":\"3\",\"value\":\"Continuidade dos estudos\"}")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("EF-4B")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("ANOTAÇÕES PEDAGÓGICAS DO BIMESTRE ANTERIOR")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.OBS_AVALIACAO_PROCESSUAL)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("OBS SOBRE A AVALIAÇÃO PROCESSUAL DO ESTUDANTE")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE)).Id).FirstOrDefault().Id
                                          && a.RespostaId.Equals(OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE)).Id
                                                                     && q.Nome == "Não").FirstOrDefault().Id)).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA)).Id).FirstOrDefault().Id
                                          && a.RespostaId.Equals(OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA)).Id
                                                                     && q.Nome == "Sim").FirstOrDefault().Id)).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_RECUPERACAO_CONTINUA)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("AÇÕES DE RECUPERAÇÃO CONTÍNUA")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_PAP)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("[{\"index\":\"1663\",\"value\":\"1663\"},{\"index\":\"1322\",\"value\":\"Contraturno\"},{\"index\":\"1770\",\"value\":\"Colaborativo\"}]")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("[{\"areaConhecimento\":\"CIENCIAS DA NATUREZA\",\"proficiencia\":90.5,\"nivel\":\"BÁSICO\"},{\"areaConhecimento\":\"LINGUA PORTUGUES\",\"proficiencia\":179.5,\"nivel\":\"BÁSICO\"}]")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("Alfabético")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("5")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL)).Id).FirstOrDefault().Id
                                          && a.RespostaId.Equals(OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL)).Id
                                                                     && q.Nome == "Não").FirstOrDefault().Id)).ShouldBeTrue();
        }

    }
}

