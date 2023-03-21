using System.Collections.Generic;
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
    public class Ao_lancar_frequencia_professor : ListaoTesteBase
    {
        public Ao_lancar_frequencia_professor(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>),
                typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Frequência Listão - Lançamento de frequência por professor com ausência, remoto e presença para os estudantes")]
        public async Task Deve_lancar_frequencia_com_ausencia_remoto_e_presenca_para_os_alunos()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
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

        [Fact(DisplayName = "Frequência Listão - Lançamento de frequência por professor regente de classe EJA com ausência, remoto e presença para os estudantes")]
        public async Task Deve_lancar_frequencia_professor_regente_classe_eja_com_ausencia_remoto_e_presenca_para_os_alunos()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.EJA,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_3,
                TipoCalendario = ModalidadeTipoCalendario.EJA,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114
            };

            await ExecutarTeste(filtroListao);
        }

        private async Task ExecutarTeste(FiltroListao filtroListao)
        {
            await CriarDadosBasicos(filtroListao);

            var listaAulaId = ObterTodos<Dominio.Aula>().Select(c => c.Id).Distinct().ToList();
            listaAulaId.ShouldNotBeNull();

            var frequenciasSalvar = listaAulaId.Select(aulaId => new FrequenciaSalvarAulaAlunosDto
                { AulaId = aulaId, Alunos = ObterListaFrequenciaSalvarAlunoComAusencia() }).ToList();

            //-> Salvar a frequencia
            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            var retorno = await useCaseSalvar.Executar(frequenciasSalvar);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.AulasIDsComErros.Any().ShouldBeFalse();
            
            //-> Obter os períodos de filtro
            var useCasePeriodos = ServiceProvider.GetService<IObterPeriodosPorComponenteUseCase>();
            useCasePeriodos.ShouldNotBeNull();
            var listaPeriodo = (await useCasePeriodos.Executar(TURMA_CODIGO_1, filtroListao.ComponenteCurricularId, false,
                filtroListao.Bimestre, true)).ToList();            

            //-> Obter retorno dos dados salvos e validar por período
            var useCaseObterFrequencia = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCaseObterFrequencia.ShouldNotBeNull();

            var listaTipoFrequenciaLancada = new List<string>();

            foreach (var filtroFrequenciaPorPeriodoDto in listaPeriodo.Select(periodo =>
                         new FiltroFrequenciaPorPeriodoDto
                         {
                             TurmaId = TURMA_CODIGO_1,
                             DisciplinaId = filtroListao.ComponenteCurricularId.ToString(),
                             ComponenteCurricularId = filtroListao.ComponenteCurricularId.ToString(),
                             DataInicio = periodo.DataInicio,
                             DataFim = periodo.DataFim
                         }))
            {
                var frequenciasPorPeriodo = await useCaseObterFrequencia.Executar(filtroFrequenciaPorPeriodoDto);
                frequenciasPorPeriodo.ShouldNotBeNull();

                var listaAluno = frequenciasPorPeriodo.Alunos;
                listaAluno.ShouldNotBeNull();

                var listaAula = listaAluno.SelectMany(c => c.Aulas).ToList();
                listaAula.ShouldNotBeNull();

                var listaDetalheFrequencia = listaAula.SelectMany(c => c.DetalheFrequencia).ToList();
                listaDetalheFrequencia.ShouldNotBeNull();

                foreach (var detalheFrequencia in listaDetalheFrequencia.Where(detalheFrequencia =>
                    !listaTipoFrequenciaLancada.Contains(detalheFrequencia.TipoFrequencia)))
                {
                    listaTipoFrequenciaLancada.Add(detalheFrequencia.TipoFrequencia);
                }
            }
            
            listaTipoFrequenciaLancada.Distinct().Intersect(TIPOS_FREQUENCIAS_SIGLA).Count()
                .ShouldBe(TIPOS_FREQUENCIAS_SIGLA.Length);
        }
    }
}