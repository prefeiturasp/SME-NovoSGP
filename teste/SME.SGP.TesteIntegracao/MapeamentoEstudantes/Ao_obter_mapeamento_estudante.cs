using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{

    public class Ao_obter_mapeamento_estudante : MapeamentoBase
    {
        public Ao_obter_mapeamento_estudante(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador()
        {
            await CriarDadosBase();

            await InserirNaBase(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var questoes = ObterTodos<Questao>();

            var secaoMapeamento = ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await InserirNaBase(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = 1,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(ALUNO_CODIGO_1, TURMA_ID_1, BIMESTRE_1);

            identificador.ShouldBe(1);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante não encontrado")]
        public async Task Ao_obter_identificador_nao_entrado()
        {
            await CriarDadosBase();

            await InserirNaBase(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var questoes = ObterTodos<Questao>();

            var secaoMapeamento = ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await InserirNaBase(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = 1,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(ALUNO_CODIGO_1, TURMA_ID_1, BIMESTRE_2);

            identificador.ShouldBeNull();
        }

    }
}
