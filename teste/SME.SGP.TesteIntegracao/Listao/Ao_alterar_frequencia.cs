using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class Ao_alterar_frequencia : ListaoTesteBase
    {
        public Ao_alterar_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>),
                typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));            
        }
        
        [Fact(DisplayName = "Frequência Listão - Alteração de frequência pelo professor titular")]
        public async Task Alteracao_de_frequencia_pelo_professor_titular()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 1,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };
            await ExecutarTeste(filtroListao);
        } 
        
        [Fact(DisplayName = "Frequência Listão - Alteração de frequência pelo professor CJ")]
        public async Task Alteracao_de_frequencia_pelo_professor_cj()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 1,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCJ(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };
            await ExecutarTeste(filtroListao);
        }

        [Fact(DisplayName = "Frequência Listão - Alteração de frequência pelo CP")]
        public async Task Alteracao_de_frequencia_pelo_cp()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 1,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };
            await ExecutarTeste(filtroListao);
        }
        
        [Fact(DisplayName = "Frequência Listão - Alteração de frequência pelo Diretor")]
        public async Task Alteracao_de_frequencia_pelo_diretor()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 1,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilDiretor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };
            await ExecutarTeste(filtroListao);
        }

        private async Task ExecutarTeste(FiltroListao filtroListao)
        {
            await CriarDadosBasicos(filtroListao);
            await CriarRegistroFrenquenciaTodasAulas(filtroListao.Bimestre,COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            var listaAulaId = ObterTodos<Dominio.Aula>().Select(c => c.Id).Distinct().ToList();
            listaAulaId.ShouldNotBeNull();

            var registroFrequenciaId = (ObterTodos<RegistroFrequencia>().FirstOrDefault()?.Id).GetValueOrDefault();
            registroFrequenciaId.ShouldBeGreaterThan(0);
            
            var frequenciasSalvar = listaAulaId.Select(aulaId => new FrequenciaSalvarAulaAlunosDto
                {FrequenciaId = registroFrequenciaId,AulaId = aulaId, Alunos = ObterListaFrequenciaSalvarAlunoComAusencia() }).ToList();
            
            var useCaseSalvar = InserirFrequenciaListaoUseCase();
            useCaseSalvar.ShouldNotBeNull();
            var retornoSalvar = await useCaseSalvar.Executar(frequenciasSalvar);

            retornoSalvar.ShouldNotBeNull();
            retornoSalvar.Auditoria.ShouldNotBeNull();
            retornoSalvar.Auditoria.AlteradoEm!.Value.Date.ShouldBeEquivalentTo(DateTimeExtension.HorarioBrasilia().Date);
        }
    }
}