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
        
        [Fact(DisplayName = "Listão - Alteração de frequência pelo professor titular.")]
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
        
        [Fact(DisplayName = "Listão - Alteração de frequência pelo professor CJ")]
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

        [Fact(DisplayName = "Listão - Alteração de frequência pelo CP")]
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
        
        [Fact(DisplayName = "Listão - Alteração de frequência pelo Diretor")]
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
                {FrequenciaId = registroFrequenciaId,AulaId = aulaId, Alunos = ObterListaFrequenciaSalvarAluno() }).ToList();
            
            
            var useCaseSalvar = InserirFrequenciaListaoUseCase();
            useCaseSalvar.ShouldNotBeNull();
            var retornoSalvar = await useCaseSalvar.Executar(frequenciasSalvar);

            retornoSalvar.ShouldNotBeNull();

            retornoSalvar.AlteradoEm!.Value.Date.ShouldBeEquivalentTo(DateTime.Now.Date);
        }
        private IEnumerable<FrequenciaSalvarAlunoDto> ObterListaFrequenciaSalvarAluno()
        {
            return CODIGOS_ALUNOS.Select(codigoAluno => new FrequenciaSalvarAlunoDto
                { CodigoAluno = codigoAluno, Frequencias = ObterFrequenciaAula(codigoAluno) }).ToList();
        }
        private IEnumerable<FrequenciaAulaDto> ObterFrequenciaAula(string codigoAluno)
        {
            string[] codigosAlunosAusencia = { CODIGO_ALUNO_1, CODIGO_ALUNO_3 };
            string[] codigosAlunosPresenca = { CODIGO_ALUNO_2, CODIGO_ALUNO_4, CODIGO_ALUNO_6 };
            string[] codigosAlunosRemotos = { CODIGO_ALUNO_5 };

            return QUANTIDADES_AULAS.Select(numeroAula => new FrequenciaAulaDto
            {
                NumeroAula = numeroAula,
                TipoFrequencia = codigosAlunosAusencia.Contains(codigoAluno) ? TipoFrequencia.F.ObterNomeCurto() :
                    codigosAlunosPresenca.Contains(codigoAluno) ? TipoFrequencia.C.ObterNomeCurto() :
                    codigosAlunosRemotos.Contains(codigoAluno) ? TipoFrequencia.R.ObterNomeCurto() :
                    TIPOS_FREQUENCIAS[new Random().Next(TIPOS_FREQUENCIAS.Length)].ObterNomeCurto()
            }).ToList();
        }
    }
}