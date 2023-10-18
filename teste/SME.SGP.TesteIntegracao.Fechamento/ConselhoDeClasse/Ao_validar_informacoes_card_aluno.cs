using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_validar_informacoes_card_aluno : ConselhoDeClasseTesteBase
    {
        public Ao_validar_informacoes_card_aluno(CollectionFixture collectionFixture): base(collectionFixture)
        {
            
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));

        }

        [Fact]
        public async Task Deve_retornar_lista_com_alunos_e_seus_dados()
        {
            await CriarDadosBase(new FiltroConselhoClasseDto()
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
            
            var obterDadosAlunos = await consulta!.ObterDadosAlunos(turma.CodigoTurma, turma.AnoLetivo, turma.Semestre);
            
            
            obterDadosAlunos.ShouldNotBeNull();
            obterDadosAlunos.ShouldNotBeNull().Any();
            obterDadosAlunos.FirstOrDefault().Nome.ShouldNotBeNull();
            obterDadosAlunos.FirstOrDefault().CodigoEOL.ShouldNotBeNull();
            obterDadosAlunos.FirstOrDefault().NomeResponsavel.ShouldNotBeNull();

        }
    }
}