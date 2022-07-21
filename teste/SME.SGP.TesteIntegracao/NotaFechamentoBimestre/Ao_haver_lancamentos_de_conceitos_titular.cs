using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_haver_lancamentos_de_conceitos_titular : NotaFechamentoBimestreTesteBase
    {
        private const string ALUNO_CODIGO_1 = "1";
        private const string ALUNO_CODIGO_2 = "2";
        private const string ALUNO_CODIGO_3 = "3";

        public Ao_haver_lancamentos_de_conceitos_titular(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task deve_haver_lancamento_com_mais_de_50_porcento_NS_para_inserir_justificativa()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilCP(), ANO_1));
            await ExecutarComandoConceito();
        }

        [Fact]
        public async Task deve_haver_lancamento_com_mais_de_50_porcento_NS_para_inserir_justificativa_regencia()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaRegenciaDeClasse(ObterPerfilCP(), ANO_1));
            await ExecutarComandoConceito();
        }

        private async Task ExecutarComandoConceito()
        {
            var fechamentoNotaDto = new List<FechamentoNotaDto>()
            {
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int) ConceitoValores.NS,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = "",
                    AlteradoRf = "",
                    Anotacao ="",
                    CodigoAluno = ALUNO_CODIGO_1,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId= (int) ConceitoValores.NS,
                    CriadoPor= "",
                    CriadoRf= "",
                    Id= 1,
                    Nota= null,
                    NotaAnterior= null,
                    SinteseId= (int)SinteseEnum.Frequente
                },
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int) ConceitoValores.NS,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = "",
                    AlteradoRf = "",
                    Anotacao ="",
                    CodigoAluno = ALUNO_CODIGO_2,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId= (int) ConceitoValores.NS,
                    CriadoPor= "",
                    CriadoRf= "",
                    Id= 1,
                    Nota= null,
                    NotaAnterior= null,
                    SinteseId= (int)SinteseEnum.Frequente
                },
                new FechamentoNotaDto()
                {
                    ConceitoIdAnterior = (int) ConceitoValores.P,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = "",
                    AlteradoRf = "",
                    Anotacao ="",
                    CodigoAluno = ALUNO_CODIGO_3,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    ConceitoId= (int) ConceitoValores.P,
                    CriadoPor= "",
                    CriadoRf= "",
                    Id= 1,
                    Nota= null,
                    NotaAnterior= null,
                    SinteseId= (int)SinteseEnum.Frequente
                }
            };

            var fechamentoTurmaDisciplinaDto = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = 1 ,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Id = 1,
                    Justificativa = "" ,
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = fechamentoNotaDto
                }
            };

            await ExecutarTeste(fechamentoTurmaDisciplinaDto);
        }

        private async Task ExecutarTeste(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma)
        {
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();

            await comando.Salvar(fechamentoTurma);
            var notasFechamento = ObterTodos<FechamentoNota>();

            notasFechamento.ShouldNotBeNull();
            notasFechamento.ShouldNotBeEmpty();
            notasFechamento.Count(x => x.ConceitoId == (long)ConceitoValores.NS).ShouldBeGreaterThan((notasFechamento.Count() / 2));
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

        private FiltroFechamentoNotaDto ObterFiltroFechamentoNotaRegenciaDeClasse(string perfil, string anoTurma, bool consideraAnorAnterior = false)
        {
            return new FiltroFechamentoNotaDto()
            {
                Perfil = perfil,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = consideraAnorAnterior,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.EJA,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina
            };
        }
    }
}