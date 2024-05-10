using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_transferir_pendencia_para_novo_responsavel : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;

        public Ao_transferir_pendencia_para_novo_responsavel(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact(DisplayName = "Plano AEE - Deve transferir pendência para uma novo responsável")]
        public async Task Deve_transferir_pendencia_para_novo_responsavel()
        {
            await CriaBase();

            var useCase = ServiceProvider.GetService<ITransferirPendenciaParaNovoResponsavelUseCase>();

            var command = new TransferirPendenciaParaNovoResponsavelCommand(1, 2);
            var jsonMensagem = JsonSerializer.Serialize(command);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var lista = ObterTodos<Dominio.PendenciaUsuario>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().UsuarioId.ShouldBe(2);
        }

        private async Task CriaBase()
        {
            await _builder.CriaItensComunsEja();

            await InserirNaBase(new Pendencia()
            {
                Id = 1,
                Tipo = TipoPendencia.AEE,
                Descricao = "Pendência AEE",
                Titulo = "Pendência plano AEE",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                Id = 1,
                AlunoCodigo = "11223344",
                ResponsavelId = 1,
                TurmaId = 1,
                AlunoNome = "Maria Aluno teste",
                Questoes = new List<PlanoAEEQuestao>(),
                Situacao = SituacaoPlanoAEE.ParecerCP,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new Usuario
            {
                Id = 2,
                Login = "1234567",
                CodigoRf = "1234567",
                Nome = "Maria dos testes",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });

            await InserirNaBase(new PendenciaPlanoAEE
            {
                PendenciaId = 1,
                PlanoAEEId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new Dominio.PendenciaUsuario
            {
                PendenciaId = 1,
                UsuarioId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });
        }
    }
}
