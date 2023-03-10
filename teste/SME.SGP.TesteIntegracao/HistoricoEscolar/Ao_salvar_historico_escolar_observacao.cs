using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Excecoes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.HistoricoEscolar
{
    public class Ao_salvar_historico_escolar_observacao : TesteBase
    {
        public Ao_salvar_historico_escolar_observacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_enviar_fila_gravar_observacao_complementar_historico_escolar()
        {
            var codigoAluno = "456789";
            var salvarObservacaoHistoricoEscolarDto = new SalvarObservacaoHistoricoEscolarDto("Inserir observação nova para aluno");

            var salvarHistoricoEscolarObservacaoUseCase = ServiceProvider.GetService<IEnviarFilaGravarHistoricoEscolarObservacaoUseCase>();

            var retorno = await salvarHistoricoEscolarObservacaoUseCase.Executar(codigoAluno, salvarObservacaoHistoricoEscolarDto);

            retorno.ShouldBe(true);
        }

        [Fact]
        public async Task Excecao_negocio_enviar_fila_gravar_observacao_complementar_historico_escolar()
        {
            string codigoAluno = null;
            var salvarObservacaoHistoricoEscolarDto = new SalvarObservacaoHistoricoEscolarDto("Inserir observação nova para aluno");

            var salvarHistoricoEscolarObservacaoUseCase = ServiceProvider.GetService<IEnviarFilaGravarHistoricoEscolarObservacaoUseCase>();

            var exAlunoCodigoVazio = await Should.ThrowAsync<NegocioException>(() => salvarHistoricoEscolarObservacaoUseCase.Executar(codigoAluno, salvarObservacaoHistoricoEscolarDto));

            exAlunoCodigoVazio.Message.ShouldBe("O Código do Aluno deve ser informado.");
        }

        [Fact]
        public async Task Deve_executar_gravacao_observacao_complementar_historico_escolar()
        {
            var historicoEscolarObservacaoDto = new HistoricoEscolarObservacaoDto("456789", "Inserir observação nova para aluno");
            var mensagemHabbit = CriarMensagemRabbit(historicoEscolarObservacaoDto);

            var salvarHistoricoEscolarObservacaoUseCase = ServiceProvider.GetService<IExecutarGravarHistoricoEscolarObservacaoUseCase>();

            var retorno = await salvarHistoricoEscolarObservacaoUseCase.Executar(mensagemHabbit);

            retorno.ShouldBe(true);
        }

        [Fact]
        public async Task Deve_executar_alteracao_observacao_complementar_historico_escolar()
        {
            var alunoCodigo = "777777";

            var historicoEscolar = new HistoricoEscolarObservacao(alunoCodigo, "observação que será alterada")
            {
                CriadoPor = "Teste",
                CriadoRF = "123456"
            };

            await InserirNaBase(historicoEscolar);

            var historicoEscolarObservacaoDto = new HistoricoEscolarObservacaoDto(alunoCodigo, "Nova observação para o aluno");
            var mensagemHabbit = CriarMensagemRabbit(historicoEscolarObservacaoDto);

            var salvarHistoricoEscolarObservacaoUseCase = ServiceProvider.GetService<IExecutarGravarHistoricoEscolarObservacaoUseCase>();

            var retorno = await salvarHistoricoEscolarObservacaoUseCase.Executar(mensagemHabbit);

            retorno.ShouldBe(true);
        }

        [Fact]
        public async Task Excecao_validacao_executar_gravacao_observacao_complementar_historico_escolar()
        {
            var historicoEscolarObservacaoAlunoCodigoVazio = new HistoricoEscolarObservacaoDto(null, "Nova observação para o aluno");
            var historicoEscolarObservacaoObservacaoVazio = new HistoricoEscolarObservacaoDto("123456", null);
            var historicoEscolarObservacaoObservacaoTamanhoMaximo = new HistoricoEscolarObservacaoDto("123456", RandomString(501));

            var salvarHistoricoEscolarObservacaoUseCase = ServiceProvider.GetService<IExecutarGravarHistoricoEscolarObservacaoUseCase>();

            var exAlunoCodigoVazio = await Should.ThrowAsync<ValidacaoException>(() => salvarHistoricoEscolarObservacaoUseCase.Executar(CriarMensagemRabbit(historicoEscolarObservacaoAlunoCodigoVazio)));
            var exObservacaoVazio = await Should.ThrowAsync<ValidacaoException>(() => salvarHistoricoEscolarObservacaoUseCase.Executar(CriarMensagemRabbit(historicoEscolarObservacaoObservacaoVazio)));
            var exObservacaoTamanhoMaximo = await Should.ThrowAsync<ValidacaoException>(() => salvarHistoricoEscolarObservacaoUseCase.Executar(CriarMensagemRabbit(historicoEscolarObservacaoObservacaoTamanhoMaximo)));

            exAlunoCodigoVazio.Erros.FirstOrDefault().ErrorMessage.ShouldBe("O Código do Aluno deve ser informado.");
            exObservacaoVazio.Erros.FirstOrDefault().ErrorMessage.ShouldBe("Observação deve ser informado.");
            exObservacaoTamanhoMaximo.Erros.FirstOrDefault().ErrorMessage.ShouldBe("Observação não pode conter mais que 500 caracteres.");
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static MensagemRabbit CriarMensagemRabbit(object objeto)
        {
            var json = JsonConvert.SerializeObject(objeto);

            return new MensagemRabbit(json);
        }
    }
}
