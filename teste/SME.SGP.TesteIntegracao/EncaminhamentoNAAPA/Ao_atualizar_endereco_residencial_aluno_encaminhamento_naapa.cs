using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using StackExchange.Redis;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_atualizar_endereco_residencial_aluno_encaminhamento_naapa : EncaminhamentoNAAPATesteBase
    {
        private const string ENDERECO_RESIDENCIAL_ALUNO_NAAPA_DIFERENTE = "[{\"numero\":\"140\",\"complemento\":\"Casa 02\",\"bairro\":\"Interior\",\"tipoLogradouro\":\"Rua\",\"logradouro\":\"Rua das melancias\"}]";
        private const string ENDERECO_RESIDENCIAL_ALUNO_NAAPA_IGUAL = "[{\"numero\":\"142\",\"complemento\":\"Casa\",\"bairro\":\"Centro\",\"tipoLogradouro\":\"Rua\",\"logradouro\":\"Rua das maçãs\"}]";
        private const string ENDERECO_RESIDENCIAL_ALUNO_EOL = "[{\"numero\":\"142\",\"complemento\":\"Casa\",\"bairro\":\"Centro\",\"tipoLogradouro\":\"Rua\",\"logradouro\":\"Rua das maçãs\"}]";

        public Ao_atualizar_endereco_residencial_aluno_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoEnderecoEolQuery, AlunoEnderecoRespostaDto>), typeof(ObterAlunoEnderecoEolQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Execução worker de alteração de endereço residencial aluno com novo endereço alterado EOL")]
        public async Task Ao_encontrar_endereco_diferente_eol_deve_alterar_resposta_naapa()
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
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            var atualizarEnderecoDoEncaminhamentoNAAPAUseCase = ObterServicoAtualizarEnderecoDoEncaminhamentoNAAPA();
            await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date, ENDERECO_RESIDENCIAL_ALUNO_NAAPA_DIFERENTE);

            var questoesQuestionarioNAAPA = ObterTodos<Dominio.Questao>().Where(questao => questao.NomeComponente == NOME_COMPONENTE_QUESTAO_ENDERECO_RESIDENCIAL);
            questoesQuestionarioNAAPA.ShouldBeUnique("Qdade registros Questão [Endereço residencial] inválidos");

            var questoesEncaminhamentoNAAPA = ObterTodos<Dominio.QuestaoEncaminhamentoNAAPA>().Where(questao => questao.QuestaoId == questoesQuestionarioNAAPA.FirstOrDefault().Id);
            questoesEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Questão Encaminhamento NAAPA [Endereço residencial] inválidos");

            var respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Endereço residencial] inválidos");
            var respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(ENDERECO_RESIDENCIAL_ALUNO_NAAPA_DIFERENTE);

            var encaminhamentoNAAPADto = new AtendimentoNAAPADto { Id = 1, AlunoCodigo = ALUNO_CODIGO_1 };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(encaminhamentoNAAPADto) };
            var retorno = await atualizarEnderecoDoEncaminhamentoNAAPAUseCase.Executar(mensagem);

            retorno.ShouldBe(true);
            respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Endereço residencial] inválidos");
            respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(ENDERECO_RESIDENCIAL_ALUNO_EOL);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Execução worker de alteração de endereço residencial aluno com novo endereço alterado EOL quando não existir resposta de endereço no encaminhamento")]
        public async Task Ao_encontrar_endereco_diferente_eol_deve_incluir_resposta_naapa()
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
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            var atualizarEnderecoDoEncaminhamentoNAAPAUseCase = ObterServicoAtualizarEnderecoDoEncaminhamentoNAAPA();
            await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date, null);

            var questoesQuestionarioNAAPA = ObterTodos<Dominio.Questao>().Where(questao => questao.NomeComponente == NOME_COMPONENTE_QUESTAO_ENDERECO_RESIDENCIAL);
            questoesQuestionarioNAAPA.ShouldBeUnique("Qdade registros Questão [Endereço residencial] inválidos");

            var questoesEncaminhamentoNAAPA = ObterTodos<Dominio.QuestaoEncaminhamentoNAAPA>().Where(questao => questao.QuestaoId == questoesQuestionarioNAAPA.FirstOrDefault().Id);
            questoesEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Questão Encaminhamento NAAPA [Endereço residencial] inválidos");

            var respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeEmpty();

            var encaminhamentoNAAPADto = new AtendimentoNAAPADto { Id = 1, AlunoCodigo = ALUNO_CODIGO_1 };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(encaminhamentoNAAPADto) };
            var retorno = await atualizarEnderecoDoEncaminhamentoNAAPAUseCase.Executar(mensagem);

            retorno.ShouldBe(true);
            respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Endereço residencial] inválidos");
            var respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(ENDERECO_RESIDENCIAL_ALUNO_EOL);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Execução worker de alteração de endereço residencial aluno com novo endereço alterado EOL quando existir resposta de endereço vazio no encaminhamento")]
        public async Task Ao_encontrar_endereco_diferente_eol_deve_alterar_resposta_vazia_naapa()
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
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            var atualizarEnderecoDoEncaminhamentoNAAPAUseCase = ObterServicoAtualizarEnderecoDoEncaminhamentoNAAPA();
            await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date, "");

            var questoesQuestionarioNAAPA = ObterTodos<Dominio.Questao>().Where(questao => questao.NomeComponente == NOME_COMPONENTE_QUESTAO_ENDERECO_RESIDENCIAL);
            questoesQuestionarioNAAPA.ShouldBeUnique("Qdade registros Questão [Endereço residencial] inválidos");

            var questoesEncaminhamentoNAAPA = ObterTodos<Dominio.QuestaoEncaminhamentoNAAPA>().Where(questao => questao.QuestaoId == questoesQuestionarioNAAPA.FirstOrDefault().Id);
            questoesEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Questão Encaminhamento NAAPA [Endereço residencial] inválidos");

            var respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Endereço residencial] inválidos");
            var respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe("");

            var encaminhamentoNAAPADto = new AtendimentoNAAPADto { Id = 1, AlunoCodigo = ALUNO_CODIGO_1 };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(encaminhamentoNAAPADto) };
            var retorno = await atualizarEnderecoDoEncaminhamentoNAAPAUseCase.Executar(mensagem);

            retorno.ShouldBe(true);
            respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Endereço residencial] inválidos");
            respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(ENDERECO_RESIDENCIAL_ALUNO_EOL);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Execução worker de alteração de endereço residencial aluno com endereço não alterado EOL")]
        public async Task Ao_encontrar_endereco_igual_eol_nao_deve_alterar_resposta_naapa()
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
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            var atualizarEnderecoDoEncaminhamentoNAAPAUseCase = ObterServicoAtualizarEnderecoDoEncaminhamentoNAAPA();
            await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date, ENDERECO_RESIDENCIAL_ALUNO_NAAPA_IGUAL);

            var questoesQuestionarioNAAPA = ObterTodos<Dominio.Questao>().Where(questao => questao.NomeComponente == NOME_COMPONENTE_QUESTAO_ENDERECO_RESIDENCIAL);
            questoesQuestionarioNAAPA.ShouldBeUnique("Qdade registros Questão [Endereço residencial] inválidos");

            var questoesEncaminhamentoNAAPA = ObterTodos<Dominio.QuestaoEncaminhamentoNAAPA>().Where(questao => questao.QuestaoId == questoesQuestionarioNAAPA.FirstOrDefault().Id);
            questoesEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Questão Encaminhamento NAAPA [Endereço residencial] inválidos");

            var respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Endereço residencial] inválidos");
            var respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(ENDERECO_RESIDENCIAL_ALUNO_NAAPA_IGUAL);

            var encaminhamentoNAAPADto = new AtendimentoNAAPADto { Id = 1, AlunoCodigo = ALUNO_CODIGO_1 };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(encaminhamentoNAAPADto) };
            var retorno = await atualizarEnderecoDoEncaminhamentoNAAPAUseCase.Executar(mensagem);

            retorno.ShouldBe(false);
            respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Endereço residencial] inválidos");
            respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(ENDERECO_RESIDENCIAL_ALUNO_EOL);
        }

        private async Task GerarDadosEncaminhamentoNAAPA(DateTime dataQueixa, string? enderecoResidencial)
        {
            await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecao();
            await CriarQuestoesEncaminhamentoNAAPA();
            await CriarRespostasEncaminhamentoNAAPA(dataQueixa, enderecoResidencial);
        }

        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa, string? enderecoResidencial)
        {
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

             if (enderecoResidencial.NaoEhNulo())
                await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
                {
                    QuestaoEncaminhamentoId = 3,
                    Texto = enderecoResidencial,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });           
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            //ID 01
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //ID 02
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //ID 03
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_ENDERECO_RESIDENCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
           
        }

        private async Task CriarEncaminhamentoNAAPASecao()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }

}

