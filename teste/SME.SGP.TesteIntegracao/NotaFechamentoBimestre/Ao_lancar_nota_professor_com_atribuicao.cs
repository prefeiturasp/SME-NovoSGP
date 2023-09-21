using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_lancar_nota_professor_com_atribuicao: NotaFechamentoBimestreTesteBase
    {
        public Ao_lancar_nota_professor_com_atribuicao(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>),
                typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));            
        }

        [Fact]
        public async Task Deve_permitir_lancamento_nota_para_professor_com_atribuicao()
        {
            var filtroFechamentoNota = ObterFiltroFechamentoNotaDtoFundamental(ObterPerfilProfessor(), ANO_7);
            
            filtroFechamentoNota.CriarPeriodoEscolar = false;
            
            filtroFechamentoNota.CriarPeriodoEscolarCustomizado = true;
            
            filtroFechamentoNota.PeriodoEscolarValido = true;
            
            filtroFechamentoNota.CriarPeriodoAbertura = false;
            
            await CriarDadosBase(filtroFechamentoNota);

            await ExecutarComandoConceito();
        }

        private async Task ExecutarComandoConceito()
        {
            var fechamentoTurmaDisciplinaDto = ObterFechamentoTurmaDisciplinaDto();

            await ExecutarTeste(fechamentoTurmaDisciplinaDto);
        }
        
        private async Task ExecutarComandoConceitoComExcecao()
        {
            var fechamentoTurmaDisciplinaDto = ObterFechamentoTurmaDisciplinaDto();

            await ExecutarTesteComExcecao(fechamentoTurmaDisciplinaDto);
        }

        private List<FechamentoTurmaDisciplinaDto> ObterFechamentoTurmaDisciplinaDto()
        {
            var fechamentoNotaDto = new List<FechamentoNotaDto>()
            {
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int)ConceitoValores.NS,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CodigoAluno = ALUNO_CODIGO_1,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId = (int)ConceitoValores.NS,
                    Nota = null,
                    NotaAnterior = null,
                    SinteseId = (int)SinteseEnum.Frequente
                },
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int)ConceitoValores.NS,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CodigoAluno = ALUNO_CODIGO_2,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId = (int)ConceitoValores.NS,
                    Nota = null,
                    NotaAnterior = null,
                    SinteseId = (int)SinteseEnum.Frequente
                },
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int)ConceitoValores.P,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CodigoAluno = ALUNO_CODIGO_3,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId = (int)ConceitoValores.P,
                    Nota = null,
                    NotaAnterior = null,
                    SinteseId = (int)SinteseEnum.Frequente
                }
            };

            var fechamentoTurmaDisciplinaDto = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = BIMESTRE_4,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Justificativa = "Teste",
                    TurmaId = TURMA_CODIGO_1,
                    NotaConceitoAlunos = fechamentoNotaDto
                }
            };
            return fechamentoTurmaDisciplinaDto;
        }

        private FiltroFechamentoNotaDto ObterFiltroFechamentoNotaDtoFundamental(string perfil, string anoTurma, bool consideraAnorAnterior = false)
        {
            return new FiltroFechamentoNotaDto()
            {
                Perfil = perfil,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = consideraAnorAnterior,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
            };
        }
    }
} 