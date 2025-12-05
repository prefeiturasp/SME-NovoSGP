using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Constantes;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.MapeamentoEstudante;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{
    public class Ao_obter_secoes_e_questoes_questionario_mapeamento_estudante : MapeamentoBase
    {

        public Ao_obter_secoes_e_questoes_questionario_mapeamento_estudante(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

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

        [Fact(DisplayName = "Mapeamento Estudantes - Listar as seções")]
        public async Task Ao_listar_secoes_mapeamento_estudantes()
        {
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<IObterSecoesMapeamentoSecaoUseCase>();
            var retorno = await useCase.Executar(null);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(1);
            var secao = retorno.FirstOrDefault();
            secao.NomeComponente.ShouldBe(NOME_COMPONENTE_SECAO_1_MAPEAMENTO_ESTUDANTE);
            secao.Concluido.ShouldBeFalse();
            secao.Auditoria.ShouldBe(null);
        }

        [Fact(DisplayName = "Mapeamento Estudantes - Listar questões por questionário (com respostas default para inclusão)")]
        public async Task Ao_listar_questoes_questionario_com_respostas_default()
        {
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<IObterQuestionarioMapeamentoEstudanteUseCase>();
            var retorno = await useCase.Executar(new FiltroQuestoesQuestionarioMapeamentoEstudanteDto() { QuestionarioId = 1, TurmaId = TURMA_ID_1, CodigoAluno = ALUNO_CODIGO_1, Bimestre = 2 });
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(19);
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR) 
                             && q.TipoQuestao.Equals(TipoQuestao.ComboDinamico)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR)
                             && q.TipoQuestao.Equals(TipoQuestao.Frase)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR)
                             && q.TipoQuestao.Equals(TipoQuestao.Texto)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.NACIONALIDADE)
                             && q.TipoQuestao.Equals(TipoQuestao.Frase)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_REDE_APOIO)
                             && q.TipoQuestao.Equals(TipoQuestao.EditorTexto)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_RECUPERACAO_CONTINUA)
                             && q.TipoQuestao.Equals(TipoQuestao.EditorTexto)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_PAP)
                             && q.TipoQuestao.Equals(TipoQuestao.ComboMultiplaEscolhaDinamico)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_MAIS_EDUCACAO)
                             && q.TipoQuestao.Equals(TipoQuestao.ComboMultiplaEscolhaDinamico)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROJETO_FORTALECIMENTO_APRENDIZAGENS)
                             && q.TipoQuestao.Equals(TipoQuestao.ComboMultiplaEscolhaDinamico)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL)
                             && q.TipoQuestao.Equals(TipoQuestao.Radio)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA)
                             && q.TipoQuestao.Equals(TipoQuestao.Frase)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP)
                             && q.TipoQuestao.Equals(TipoQuestao.AvaliacoesExternasProvaSP)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.OBS_AVALIACAO_PROCESSUAL)
                             && q.TipoQuestao.Equals(TipoQuestao.EditorTexto)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.FREQUENCIA)
                             && q.TipoQuestao.Equals(TipoQuestao.Combo)
                             && q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA)
                             && q.TipoQuestao.Equals(TipoQuestao.Numerico)
                             && q.Obrigatorio).ShouldBeTrue();


            var questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("{\"index\":\"2\",\"value\":\"Promovido\"}");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("EF-5B");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("ANOTAÇÕES PEDAGÓGICAS DO BIMESTRE ANTERIOR");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.NACIONALIDADE));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("Brasil");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Sim").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Sim").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Sim").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_PAP));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("[{\"index\":\"1322\",\"value\":\"Contraturno\"},{\"index\":\"1770\",\"value\":\"Colaborativo\"}]");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_MAIS_EDUCACAO));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("[{\"index\":\"0\",\"value\":\"Não\"}]");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROJETO_FORTALECIMENTO_APRENDIZAGENS));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("[{\"index\":\"1663\",\"value\":\"1663\"},{\"index\":\"1664\",\"value\":\"1664\"}]");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Não").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("Alfabético");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("[{\"areaConhecimento\":\"CIENCIAS DA NATUREZA\",\"proficiencia\":90.5,\"nivel\":\"BÁSICO\"},{\"areaConhecimento\":\"LINGUA PORTUGUES\",\"proficiencia\":179.5,\"nivel\":\"BÁSICO\"}]");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.FREQUENCIA));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Frequente").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("5");

            await CriarTurma(Dominio.Modalidade.Fundamental, ANO_1, false, tipoTurno: 2);
            retorno = await useCase.Executar(new FiltroQuestoesQuestionarioMapeamentoEstudanteDto() { QuestionarioId = 1, TurmaId = TURMA_ID_2, CodigoAluno = ALUNO_CODIGO_1, Bimestre = 2 });
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(19);
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR)
                             && q.TipoQuestao.Equals(TipoQuestao.ComboDinamico)
                             && !q.Obrigatorio).ShouldBeTrue();
            retorno.Any(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR)
                             && q.TipoQuestao.Equals(TipoQuestao.Frase)
                             && !q.Obrigatorio).ShouldBeTrue();
        }

        [Fact(DisplayName = "Mapeamento Estudantes - Listar questões por questionário (com respostas por id mapeamento editado)")]
        public async Task Ao_listar_questoes_questionario_com_respostas_mapeamento()
        {
            await CriarDadosBase();
            await GerarDadosMapeamentosEstudantes_1();
            var useCase = ServiceProvider.GetService<IObterQuestionarioMapeamentoEstudanteUseCase>();
            var retorno = await useCase.Executar(new FiltroQuestoesQuestionarioMapeamentoEstudanteDto() { QuestionarioId = 1, MapeamentoEstudanteId = 1 });
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(19);
            
            var questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("{\"index\":\"1\",\"value\":\"Promovido\"}");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("EF-4B");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("ANOTAÇÕES PEDAGÓGICAS DO BIMESTRE ANTERIOR");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Sim").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.NACIONALIDADE));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("Brasil");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Sim").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Sim").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Sim").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_REDE_APOIO));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("AÇÕES DA REDE DE APOIO");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.ACOES_RECUPERACAO_CONTINUA));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("AÇÕES DE RECUPERAÇÃO CONTÍNUA");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_PAP));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("[{\"index\":\"1663\",\"value\":\"1663 RECUPERACAO PARALELA AUTORAL PORTUGUES\"}, {\"index\":\"1664\",\"value\":\"1664 RECUPERACAO PARALELA AUTORAL MATEMATICA\"}]");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PARTICIPA_MAIS_EDUCACAO));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("[{\"index\":\"1\",\"value\":\"XADREZ\"}, {\"index\":\"2\",\"value\":\"FUTEBOL\"}]");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROJETO_FORTALECIMENTO_APRENDIZAGENS));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("[{\"index\":\"1255\",\"value\":\"1255 Acompanhamento Pedagógico Matemática\"}, {\"index\":\"1204\",\"value\":\"1204 Acompanhamento Pedagógico Português\"}]");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Sim").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("Não alfabético");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("[{\"areaconhecimento\":\"Ciências da Natureza\",\"proficiencia\": 95.5,\"nivel\":\"Abaixo do básico\"}, {\"areaconhecimento\":\"Ciências Humanas\",\"proficiencia\": 179.5,\"nivel\":\"Básico\"}]");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.OBS_AVALIACAO_PROCESSUAL));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("OBS SOBRE A AVALIAÇÃO PROCESSUAL DO ESTUDANTE");

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.FREQUENCIA));
            questao.Resposta.FirstOrDefault().OpcaoRespostaId.ShouldBe(questao.OpcaoResposta.FirstOrDefault(op => op.Nome == "Frequente").Id);

            questao = retorno.FirstOrDefault(q => q.NomeComponente.Equals(NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA));
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("10");
        }

    }
}