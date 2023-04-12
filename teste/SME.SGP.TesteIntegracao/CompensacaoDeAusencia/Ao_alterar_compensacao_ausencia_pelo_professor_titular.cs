using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_alterar_compensacao_ausencia_pelo_professor_titular : Ao_lancar_compensacao_de_ausencia_base
    {
        public Ao_alterar_compensacao_ausencia_pelo_professor_titular(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRepositorioCache), typeof(RepositorioCacheFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery, IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>>), typeof(ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));            
        }

        [Fact]
        public async Task Deve_adicionar_aluno_na_compensacao_sem_aulas_selecionadas()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecuteInserirCompensasaoAusenciaSemAulasSelecionadas(dtoDadoBase);
        }

        [Fact]
        public async Task Deve_adicionar_aluno_na_compensacao_com_aulas_selecionadas()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecuteInserirCompensasaoAusenciaComAulasSelecionadas(dtoDadoBase);
        }

        [Fact]
        public async Task Deve_alterar_quantidade_ausencias_compensadas_sem_aulas_selecionadas()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecuteAlterarCompensacaoAusenciaSemAulasSelecionadas(dtoDadoBase);
        }

        [Fact]
        public async Task Deve_alterar_quantidade_ausencias_compensadas_com_aulas_selecionadas()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecuteAlterarCompensacaoAusenciaComAulasSelecionadas(dtoDadoBase);
        }

        [Fact]
        public async Task Deve_remover_aluno_na_compensacao()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await ExecutarTesteRemoverAlunoCompensacao(dtoDadoBase);
        }
    }
}