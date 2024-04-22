using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
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
        public async Task Ao_obter_identificador_nao_encontrado()
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

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificadores dos mapeamentos estudantes do bimestre atual")]
        public async Task Ao_obter_identificadores_mapeamento_do_bimestre_atual()
        {
            var identificadoresAtual = new List<long>();
            await CriarDadosBase();

            identificadoresAtual.Add(await InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));

            identificadoresAtual.Add(await InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_2,
                AlunoNome = "Nome do aluno 2",
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));

            await InserirNaBase(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_3,
                AlunoNome = "Nome do aluno 3",
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var mediator = ServiceProvider.GetService<IMediator>();

            var identificadores = await mediator.Send(new ObterIdentificadoresDosMapeamentosDoBimestreAtualQuery());

            identificadores.ShouldNotBeNull();
            identificadores.Count().ShouldBe(2);
            identificadoresAtual.Exists(id => identificadores.Contains(id)).ShouldBeTrue();
        }
    }
}
