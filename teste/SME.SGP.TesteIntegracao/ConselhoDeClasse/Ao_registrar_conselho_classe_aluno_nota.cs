using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.Base;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_registrar_conselho_classe_aluno_nota : ConselhoClasseTesteBase
    {
        
        public Ao_registrar_conselho_classe_aluno_nota(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerComRegistroFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_inserir_conselho_classe_aluno_nota_conceito()
        {
            var dtoNota = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                Conceito = 1,
                Justificativa = JUSTIFICATIVA
            };

            await ExecuteTeste(dtoNota, TipoNota.Conceito);
        }

        [Fact]
        public async Task Ao_inserir_conselho_classe_aluno_nota_numerica()
        {
            var dtoNota = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                Nota = 6,
                Justificativa = JUSTIFICATIVA
            };

            await ExecuteTeste(dtoNota, TipoNota.Nota);
        }

        [Fact]
        public async Task Ao_alterar_conselho_classe_aluno_nota_conceito()
        {
            var dtoNota = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                Conceito = 1,
                Justificativa = JUSTIFICATIVA
            };

            await ExecuteTeste(dtoNota, TipoNota.Conceito, true);
        }

        [Fact]
        public async Task Ao_alterar_conselho_classe_aluno_nota_numerica()
        {
            var dtoNota = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                Nota = 6,
                Justificativa = JUSTIFICATIVA
            };

            await ExecuteTeste(dtoNota, TipoNota.Nota, true);
        }

        private async Task ExecuteTeste(ConselhoClasseNotaDto dtoNota, TipoNota tipoNota, bool ehAlterar = false)
        {
            await CriaBase(tipoNota, ehAlterar);

            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            var dto = new SalvarConselhoClasseAlunoNotaDto()
            {
                Bimestre = BIMESTRE_1,
                CodigoAluno = ALUNO_CODIGO_1,
                CodigoTurma = TURMA_CODIGO_1,
                FechamentoTurmaId = 1,
                ConselhoClasseNotaDto = dtoNota
            };

            if (ehAlterar)
                dto.ConselhoClasseId = 1;

            var dtoRetorno = await useCase.Executar(dto);
            dtoRetorno.ShouldNotBeNull();

            var listaConselho = ObterTodos<ConselhoClasse>();
            listaConselho.ShouldNotBeNull();
            var conselho = listaConselho.FirstOrDefault();
            conselho.ShouldNotBeNull();
            conselho.FechamentoTurmaId.ShouldBe(1);

            var listaConselhoAluno = ObterTodos<ConselhoClasseAluno>();
            listaConselhoAluno.ShouldNotBeNull();
            var conselhoAluno = listaConselhoAluno.FirstOrDefault(aluno => aluno.AlunoCodigo == ALUNO_CODIGO_1);
            conselhoAluno.ShouldNotBeNull();

            var listaConselhoNota = ObterTodos<ConselhoClasseNota>();
            listaConselhoNota.ShouldNotBeNull();
            var conselhoNota = listaConselhoNota.FirstOrDefault(nota => nota.ConselhoClasseAlunoId == conselhoAluno.Id && 
                                                                        nota.ComponenteCurricularCodigo == dtoNota.CodigoComponenteCurricular);
            conselhoNota.ShouldNotBeNull();
            conselhoNota.Justificativa.ShouldBe(JUSTIFICATIVA);
            if (tipoNota == TipoNota.Conceito)
                conselhoNota.ConceitoId.ShouldBe(1);
            else
                conselhoNota.Nota.ShouldBe(6);
        }

        private async Task CriaBase(TipoNota tipoNota, bool ehAlterar)
        {
            var filtroConselhoClasse = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                AnoTurma = ANO_7,
                TipoNota = tipoNota,
                Bimestre = BIMESTRE_1,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                InserirConselhoClassePadrao = ehAlterar,
                InserirFechamentoAlunoPadrao = true
            };

            await CriarDadosBase(filtroConselhoClasse);
        }
    }
}
