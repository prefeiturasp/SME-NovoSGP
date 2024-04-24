using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Constantes;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{
    public class Ao_cadastrar_editar_mapeamento_estudante : MapeamentoBase
    {
        public Ao_cadastrar_editar_mapeamento_estudante(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorCodigoEolQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
        }

        protected override async Task CriarDadosBase()
        {
            await base.CriarDadosBase();
            CarregarDadosBase();
        }

        [Fact(DisplayName = "Mapeamento Estudante - Cadastrar")]
        public async Task Ao_cadastrar_mapeamento_estudante()
        {
            await CriarDadosBase();

            var useCase = ServiceProvider.GetService<IRegistrarMapeamentoEstudanteUseCase>();
            var dtoUseCase = ObterMapeamentoEstudanteDto();

            var retorno = await useCase.Executar(dtoUseCase);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeFalse();
            (retorno.Auditoria.CriadoEm.Year == DateTimeExtension.HorarioBrasilia().Year).ShouldBeTrue();
            
            var mapeamento = ObterTodos<Dominio.MapeamentoEstudante>();
            mapeamento.Count().ShouldBe(1);
            mapeamento.FirstOrDefault().TurmaId.ShouldBe(TURMA_ID_1);
            mapeamento.FirstOrDefault().AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);

            var mapeamentoSecao = ObterTodos<MapeamentoEstudanteSecao>();
            mapeamentoSecao.ShouldNotBeNull();
            mapeamentoSecao.FirstOrDefault()?.SecaoMapeamentoEstudanteId.ShouldBe(SECAO_MAPEAMENTO_ESTUDANTE_ID_1);
            mapeamentoSecao.FirstOrDefault()?.Concluido.ShouldBeTrue();
            
            var questaoMapeamento = ObterTodos<QuestaoMapeamentoEstudante>();
            questaoMapeamento.ShouldNotBeNull();
            questaoMapeamento.Count.ShouldBe(19);
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.MIGRANTE)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_REDE_APOIO)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_RECUPERACAO_CONTINUA)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_PAP)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_MAIS_EDUCACAO)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROJETO_FORTALECIMENTO_APRENDIZAGENS)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.OBS_AVALIACAO_PROCESSUAL)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.FREQUENCIA)).Id).ShouldBeTrue();
            questaoMapeamento.Any(a => a.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA)).Id).ShouldBeTrue();

            var respostaMapeamento = ObterTodos<RespostaMapeamentoEstudante>();
            respostaMapeamento.ShouldNotBeNull();
            respostaMapeamento.Count().ShouldBe(19);
            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("{\"index\":\"1\",\"value\":\"Promovido\"}")).ShouldBeTrue();
            
            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("EF-4B")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("ANOTAÇÕES PEDAGÓGICAS DO BIMESTRE ANTERIOR")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA)).Id).FirstOrDefault().Id
                                          && a.RespostaId.Equals(OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA)).Id
                                                                     && q.Nome == "Sim").FirstOrDefault().Id)).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_RECUPERACAO_CONTINUA)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("AÇÕES DE RECUPERAÇÃO CONTÍNUA")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_PAP)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("[{\"index\":\"1663\",\"value\":\"1663 RECUPERACAO PARALELA AUTORAL PORTUGUES\"}, {\"index\":\"1664\",\"value\":\"1664 RECUPERACAO PARALELA AUTORAL MATEMATICA\"}]")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("[{\"areaconhecimento\":\"Ciências da Natureza\",\"proficiencia\": 95.5,\"nivel\":\"Abaixo do básico\"}, {\"areaconhecimento\":\"Ciências Humanas\",\"proficiencia\": 179.5,\"nivel\":\"Básico\"}]")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("Não alfabético")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("10")).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Mapeamento Estudante - Consistir questões obrigatórias ao cadastrar")]
        public async Task Ao_cadastrar_mapeamento_estudante_consistir_questoes_obrigatorias()
        {
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<IRegistrarMapeamentoEstudanteUseCase>();
            var dtoUseCase = ObterMapeamentoEstudanteDto(true);

            var excecao = await Assert.ThrowsAsync<NegocioException>(async () => await useCase.Executar(dtoUseCase));
            excecao.Message.ShouldBe("Existem questões obrigatórias não preenchidas no Mapeamento de Estudante: Seção: Mapeamento Estudante Seção 1 Questões: [2, 17]");
        }
        
        [Fact(DisplayName = "Mapeamento Estudante - Editar")]
        public async Task Ao_editar_mapeamento_estudante()
        {
            await CriarDadosBase();
            await GerarDadosMapeamentosEstudantes_1();

            var questaoregistroAcao = ObterTodos<QuestaoMapeamentoEstudante>();
            questaoregistroAcao.ShouldNotBeNull();
            questaoregistroAcao.Count().ShouldBe(19);

            var respostaregistroAcao = ObterTodos<RespostaMapeamentoEstudante>();
            respostaregistroAcao.ShouldNotBeNull();
            respostaregistroAcao.Count().ShouldBe(19);

            var useCase = ServiceProvider.GetService<IRegistrarMapeamentoEstudanteUseCase>();
            var dtoUseCase = ObterMapeamentoEstudanteDto();
            PreencherIdsEdicao(dtoUseCase);
            var retorno = await useCase.Executar(dtoUseCase);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();

            var mapeamento = ObterTodos<Dominio.MapeamentoEstudante>();
            mapeamento.Count().ShouldBe(1);
            mapeamento.FirstOrDefault().TurmaId.ShouldBe(TURMA_ID_1);
            mapeamento.FirstOrDefault().AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);

            var mapeamentoSecao = ObterTodos<MapeamentoEstudanteSecao>();
            mapeamentoSecao.ShouldNotBeNull();
            mapeamentoSecao.FirstOrDefault()?.SecaoMapeamentoEstudanteId.ShouldBe(SECAO_MAPEAMENTO_ESTUDANTE_ID_1);
            mapeamentoSecao.FirstOrDefault()?.Concluido.ShouldBeTrue();
            
            var questaoMapeamento = ObterTodos<QuestaoMapeamentoEstudante>();
            questaoMapeamento.ShouldNotBeNull();
            questaoMapeamento.Count.ShouldBe(19);

            var respostaMapeamento = ObterTodos<RespostaMapeamentoEstudante>();
            respostaMapeamento.ShouldNotBeNull();
            respostaMapeamento.Count().ShouldBe(19);
            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("{\"index\":\"2\",\"value\":\"Retido por frequência\"}")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("EF-5B")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("ANOTAÇÕES PEDAGÓGICAS DO BIMESTRE ANTERIOR")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.OBS_AVALIACAO_PROCESSUAL)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("OBS SOBRE A AVALIAÇÃO PROCESSUAL DO ESTUDANTE ALTERADO")).ShouldBeTrue();

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
                                          && a.Texto.Equals("[{\"index\":\"1663\",\"value\":\"1663 RECUPERACAO PARALELA AUTORAL PORTUGUES\"}, {\"index\":\"1664\",\"value\":\"1664 RECUPERACAO PARALELA AUTORAL MATEMATICA\"}]")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("[{\"areaconhecimento\":\"Ciências da Natureza\",\"proficiencia\": 80.5,\"nivel\":\"Abaixo do básico\"}]")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("Não alfabético")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA)).Id).FirstOrDefault().Id
                                          && a.Texto.Equals("5")).ShouldBeTrue();

            respostaMapeamento.Any(a => a.QuestaoMapeamentoEstudanteId == questaoMapeamento.Where(q => q.QuestaoId ==
                                                                                                       Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL)).Id).FirstOrDefault().Id
                                          && a.RespostaId.Equals(OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL)).Id
                                                                     && q.Nome == "Não").FirstOrDefault().Id)).ShouldBeTrue();

        }

        private void PreencherIdsEdicao(MapeamentoEstudanteDto dtoUseCase)
        {
            dtoUseCase.Id = 1;
            var secao = dtoUseCase.Secoes.FirstOrDefault();
            var respostasQuestao = ObterTodos<RespostaMapeamentoEstudante>();
            foreach (var questao in secao.Questoes)
            {
                var nomeComponenteQuestao = Questoes.FirstOrDefault(q => q.Id == questao.QuestaoId).NomeComponente;
                questao.RespostaMapeamentoEstudanteId = respostasQuestao.FirstOrDefault(resp => resp.QuestaoMapeamentoEstudanteId ==
                                                                                                IdsQuestoesPorNomeComponente.FirstOrDefault(pair => 
                                                                                                                        pair.Key.Equals(nomeComponenteQuestao)).Value
                ).Id;
                switch (nomeComponenteQuestao)
                {
                    case NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR:
                        questao.Resposta = "{\"index\":\"2\",\"value\":\"Retido por frequência\"}";
                        break;
                    case NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR:
                        questao.Resposta = "EF-5B";
                        break;
                    case NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE:
                        questao.Resposta = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE)).Id
                                                                     && q.Nome == "Não").FirstOrDefault().Id.ToString();
                        break;
                    case NomesComponentesMapeamentoEstudante.OBS_AVALIACAO_PROCESSUAL:
                        questao.Resposta = "OBS SOBRE A AVALIAÇÃO PROCESSUAL DO ESTUDANTE ALTERADO";
                        break;
                    case NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP:
                        questao.Resposta = "[{\"areaconhecimento\":\"Ciências da Natureza\",\"proficiencia\": 80.5,\"nivel\":\"Abaixo do básico\"}]";
                        break;
                    case NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL:
                        questao.Resposta = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL)).Id
                                                                     && q.Nome == "Não").FirstOrDefault().Id.ToString();
                        break;
                    case NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA:
                        questao.Resposta = "5";
                        break;
                }
            } 
        }


        private MapeamentoEstudanteDto ObterMapeamentoEstudanteDto(bool ignorarRespostasObrigatorias = false)
        {
            return new MapeamentoEstudanteDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do mapeamento",
                Bimestre = 3,
                Secoes = new List<MapeamentoEstudanteSecaoDto>()
                {
                    new ()
                    {
                        SecaoId = SECAO_MAPEAMENTO_ESTUDANTE_ID_1,
                        Questoes = new List<MapeamentoEstudanteSecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR)).Id,
                                Resposta = "{\"index\":\"1\",\"value\":\"Promovido\"}",
                                TipoQuestao = TipoQuestao.ComboDinamico
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR)).Id,
                                Resposta = ignorarRespostasObrigatorias ? string.Empty : "EF-4B",
                                TipoQuestao = TipoQuestao.Frase
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR)).Id,
                                Resposta = "ANOTAÇÕES PEDAGÓGICAS DO BIMESTRE ANTERIOR",
                                TipoQuestao = TipoQuestao.Texto
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE)).Id,
                                Resposta = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE)).Id
                                                                     && q.Nome == "Sim").FirstOrDefault().Id.ToString(),
                                TipoQuestao = TipoQuestao.Radio
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.MIGRANTE)).Id,
                                Resposta = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.MIGRANTE)).Id
                                                                     && q.Nome == "Sim").FirstOrDefault().Id.ToString(),
                                TipoQuestao = TipoQuestao.Radio
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI)).Id,
                                Resposta = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI)).Id
                                                                     && q.Nome == "Sim").FirstOrDefault().Id.ToString(),
                                TipoQuestao = TipoQuestao.Radio
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE)).Id,
                                Resposta = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE)).Id
                                                                     && q.Nome == "Sim").FirstOrDefault().Id.ToString(),
                                TipoQuestao = TipoQuestao.Radio
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA)).Id,
                                Resposta = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA)).Id
                                                                     && q.Nome == "Sim").FirstOrDefault().Id.ToString(),
                                TipoQuestao = TipoQuestao.Radio
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_REDE_APOIO)).Id,
                                Resposta = "AÇÕES DA REDE DE APOIO",
                                TipoQuestao = TipoQuestao.EditorTexto
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_RECUPERACAO_CONTINUA)).Id,
                                Resposta = "AÇÕES DE RECUPERAÇÃO CONTÍNUA",
                                TipoQuestao = TipoQuestao.EditorTexto
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_PAP)).Id,
                                Resposta = "[{\"index\":\"1663\",\"value\":\"1663 RECUPERACAO PARALELA AUTORAL PORTUGUES\"}, {\"index\":\"1664\",\"value\":\"1664 RECUPERACAO PARALELA AUTORAL MATEMATICA\"}]",
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolhaDinamico
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_MAIS_EDUCACAO)).Id,
                                Resposta = "[{\"index\":\"1\",\"value\":\"XADREZ\"}, {\"index\":\"2\",\"value\":\"FUTEBOL\"}]",
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolhaDinamico
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROJETO_FORTALECIMENTO_APRENDIZAGENS)).Id,
                                Resposta = "[{\"index\":\"1255\",\"value\":\"1255 Acompanhamento Pedagógico Matemática\"}, {\"index\":\"1204\",\"value\":\"1204 Acompanhamento Pedagógico Português\"}]",
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolhaDinamico
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL)).Id,
                                Resposta = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL)).Id
                                                                     && q.Nome == "Sim").FirstOrDefault().Id.ToString(),
                                TipoQuestao = TipoQuestao.Radio
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA)).Id,
                                Resposta = "Não alfabético",
                                TipoQuestao = TipoQuestao.Frase
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP)).Id,
                                Resposta = "[{\"areaconhecimento\":\"Ciências da Natureza\",\"proficiencia\": 95.5,\"nivel\":\"Abaixo do básico\"}, {\"areaconhecimento\":\"Ciências Humanas\",\"proficiencia\": 179.5,\"nivel\":\"Básico\"}]",
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolhaDinamico
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.OBS_AVALIACAO_PROCESSUAL)).Id,
                                Resposta = ignorarRespostasObrigatorias ? string.Empty : "OBS SOBRE A AVALIAÇÃO PROCESSUAL DO ESTUDANTE",
                                TipoQuestao = TipoQuestao.EditorTexto
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.FREQUENCIA)).Id,
                                Resposta = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.FREQUENCIA)).Id
                                                                     && q.Nome == "Frequente").FirstOrDefault().Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA)).Id,
                                Resposta = "10",
                                TipoQuestao = TipoQuestao.Numerico
                            }
                        }
                    }
                }
            };
        }
    }
}

