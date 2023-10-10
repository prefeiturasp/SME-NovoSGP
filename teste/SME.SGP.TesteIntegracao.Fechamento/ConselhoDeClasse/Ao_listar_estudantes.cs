using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_listar_estudantes : ConselhoDeClasseTesteBase
    {
        public Ao_listar_estudantes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_ser_carregado_em_ordem_alfabetica()
        {
            await CriarDadosBase(new FiltroConselhoClasseDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Medio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_8,
                SituacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });
            
            var consulta = ServiceProvider.GetService<IConsultasFechamentoTurmaDisciplina>();
            consulta.ShouldNotBeNull();
            
            var turmas = ObterTodos<Dominio.Turma>();
            var turma = turmas.FirstOrDefault(c => c.CodigoTurma == TURMA_CODIGO_1);

            turma.ShouldNotBeNull();

            var alunos = (await consulta.ObterDadosAlunos(turma.CodigoTurma, turma.AnoLetivo, turma.Semestre)).ToList();
            var alunosOrdenados = alunos.OrderBy(c => c.Nome);

            alunosOrdenados.SequenceEqual(alunos).ShouldBeTrue();
        }
    }
}