using System;
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
    public class Ao_lancar_frequencia_cj : ListaoTesteBase
    {
        public Ao_lancar_frequencia_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>),
                typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));            
        }

        [Fact]
        public async Task Deve_lancar_frequencia_professor_cj_ensino_fundamental()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
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

        [Fact]
        public async Task Deve_lancar_frequencia_professor_cj_infantil()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.EducacaoInfantil,
                Perfil = ObterPerfilCJ(),
                AnoTurma = ANO_3,
                TipoCalendario = ModalidadeTipoCalendario.Infantil,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            await ExecutarTeste(filtroListao);            
        }
        
        private async Task ExecutarTeste(FiltroListao filtroListao)
        {
            await CriarDadosBasicos(filtroListao);

            var listaAulaId = ObterTodos<Dominio.Aula>().Select(c => c.Id).Distinct().ToList();
            listaAulaId.ShouldNotBeNull();

            var frequenciasSalvar = listaAulaId.Select(aulaId => new FrequenciaSalvarAulaAlunosDto
                { AulaId = aulaId, Alunos = ObterListaFrequenciaSalvarAluno() }).ToList();

            //-> Salvar a frequencia
            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            await useCaseSalvar.Executar(frequenciasSalvar);
            
            //-> Obter os períodos de filtro
            var useCasePeriodos = ServiceProvider.GetService<IObterPeriodosPorComponenteUseCase>();
            useCasePeriodos.ShouldNotBeNull();
            var listaPeriodo = (await useCasePeriodos.Executar(TURMA_CODIGO_1, filtroListao.ComponenteCurricularId, false,
                filtroListao.Bimestre)).ToList();            

            //-> Obter retorno dos dados salvos e validar por período
            var useCaseObterFrequencia = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCaseObterFrequencia.ShouldNotBeNull();

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
                
                listaDetalheFrequencia.Any(c => TIPOS_FREQUENCIAS_SIGLA.Contains(c.TipoFrequencia)).ShouldBeTrue();
            }
        }        
        
        private IEnumerable<FrequenciaSalvarAlunoDto> ObterListaFrequenciaSalvarAluno()
        {
            return CODIGOS_ALUNOS.Select(codigoAluno => new FrequenciaSalvarAlunoDto
                { CodigoAluno = codigoAluno, Frequencias = ObterFrequenciaAula(codigoAluno) }).ToList();
        }        
        
        private IEnumerable<FrequenciaAulaDto> ObterFrequenciaAula(string codigoAluno)
        {
            return QUANTIDADES_AULAS.Select(numeroAula => new FrequenciaAulaDto
            {
                NumeroAula = numeroAula,
                TipoFrequencia = TIPOS_FREQUENCIAS[new Random().Next(TIPOS_FREQUENCIAS.Length)].ObterNomeCurto()
            }).ToList();
        }        
    }
}