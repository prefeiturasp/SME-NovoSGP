using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using Xunit;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_registrar_nota_pelo_cp_ou_diretor : NotaFechamentoBimestreTesteBase
    {
        private const string ALUNO_CODIGO_1 = "1";

        public Ao_registrar_nota_pelo_cp_ou_diretor(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
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
        public async Task Deve_registrar_nota_numerica_como_cp()
        {

            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilCP(), ANO_5));
            await ExecutarComandoNota();
        }

        [Fact]
        public async Task Deve_registrar_nota_conceito_como_cp()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilCP(), ANO_1));
            await ExecutarComandoConceito();
        }

        [Fact]
        public async Task Deve_registrar_nota_numerica_como_diretor()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilDiretor(), ANO_1));
            await ExecutarComandoNota();
        }
        [Fact]
        public async Task Deve_registrar_nota_conceito_como_diretor()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilDiretor(), ANO_1));
            await ExecutarComandoConceito();
        }

        private async Task ExecutarTeste(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma)
        {
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();

            await comando.Salvar(fechamentoTurma);
            var notasFechamento = ObterTodos<FechamentoNota>();

            notasFechamento.ShouldNotBeNull();
            notasFechamento.ShouldNotBeEmpty();
            notasFechamento.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        private async Task ExecutarComandoNota()
        {
            var fechamentoNotaDto = new List<FechamentoNotaDto>()
            {
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = null,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = "",
                    AlteradoRf = "",
                    Anotacao ="",
                    CodigoAluno = ALUNO_CODIGO_1,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId= null,
                    CriadoPor= "",
                    CriadoRf= "",
                    Nota= 7,
                    NotaAnterior= 6,
                    SinteseId= (int)SinteseEnum.Frequente
                }
            };

            var fechamentoTurmaDisciplinaDto = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = 3,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Justificativa = "" ,
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = fechamentoNotaDto
                }
            };

            await ExecutarTeste(fechamentoTurmaDisciplinaDto);
        }

        private async Task ExecutarComandoConceito()
        {
            var fechamentoNotaDto = new List<FechamentoNotaDto>()
            {
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = 1,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = "",
                    AlteradoRf = "",
                    Anotacao ="",
                    CodigoAluno = ALUNO_CODIGO_1,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId= (long) ConceitoValores.P,
                    CriadoPor= "",
                    CriadoRf= "",
                    Nota= null,
                    NotaAnterior= null,
                    SinteseId= (int)SinteseEnum.Frequente
                }
            };

            var fechamentoTurmaDisciplinaDto = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = 3,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Justificativa = "" ,
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = fechamentoNotaDto
                }
            };

            await ExecutarTeste(fechamentoTurmaDisciplinaDto);
        }

        private FiltroFechamentoNotaDto ObterFiltroFechamentoNotaDto(string perfil, string anoTurma, bool consideraAnorAnterior = false)
        {
            return new FiltroFechamentoNotaDto()
            {
                Perfil = perfil,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = consideraAnorAnterior,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina
            };
        }
    }
}