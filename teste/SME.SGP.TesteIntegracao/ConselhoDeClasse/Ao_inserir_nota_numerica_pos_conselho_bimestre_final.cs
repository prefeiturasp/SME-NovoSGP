using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_inserir_nota_numerica_pos_conselho_bimestre_final: ConselhoDeClasseTesteBase
    {
        public Ao_inserir_nota_numerica_pos_conselho_bimestre_final(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
        }
        
        [Fact]
        public async Task Deve_lancar_numerica_pos_conselho_bimestre_2()
        {
            var filtroConselhoClasse = ObterFiltroPadraoConselhoClasseDto();
            
            await CriarDados(filtroConselhoClasse);
            
            await ExecutarTeste(filtroConselhoClasse);
        }

        [Fact]
        public async Task Deve_lancar_numerica_pos_conselho_bimestre_final()
        {
            var filtroConselhoClasse = ObterFiltroPadraoConselhoClasseDto();
            filtroConselhoClasse.BimestreConselhoClasse = BIMESTRE_FINAL;
            filtroConselhoClasse.FechamentoTurmaId = FECHAMENTO_TURMA_ID_5;
            filtroConselhoClasse.CriarConselhosTodosBimestres = true;
            
            await CriarDados(filtroConselhoClasse);
            
            await ExecutarTeste(filtroConselhoClasse);
        }
        
        private FiltroConselhoClasseDto ObterFiltroPadraoConselhoClasseDto()
        {
            return new FiltroConselhoClasseDto()
            {
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_8,
                Modalidade = Modalidade.Fundamental,
                ModalidadeTipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ConsiderarAnoAnterior = false,
                SituacaoConselho = SituacaoConselhoClasse.EmAndamento,
                CriarFechamentoBimestreFinal = true,
                ConselhoClasseId = 0,
                AlunoCodigo = ALUNO_CODIGO_1,
                BimestreConselhoClasse = BIMESTRE_2,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_2,
                ConselhoClassePersistirDto = ObterConselhoClasseNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138),
                Perfil = ObterPerfilProfessor()
            };
        }

        private async Task CriarDados(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            var dataAula = filtroConselhoClasseDto.ConsiderarAnoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroNotasDto()
            {
                Perfil = filtroConselhoClasseDto.Perfil,
                Modalidade = filtroConselhoClasseDto.Modalidade,
                TipoCalendario = filtroConselhoClasseDto.ModalidadeTipoCalendario,
                Bimestre = filtroConselhoClasseDto.BimestreConselhoClasse,
                ComponenteCurricular = filtroConselhoClasseDto.ComponenteCurricularId.ToString(),
                TipoNota = filtroConselhoClasseDto.TipoNota,
                AnoTurma = filtroConselhoClasseDto.AnoTurma,
                ConsiderarAnoAnterior = filtroConselhoClasseDto.ConsiderarAnoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = filtroConselhoClasseDto.CriarFechamentoBimestreFinal,
                SituacaoConselhoClasse = filtroConselhoClasseDto.SituacaoConselho,
                CriarConselhosTodosBimestres = filtroConselhoClasseDto.CriarConselhosTodosBimestres,
                
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }

        private ConselhoClasseNotaDto ObterConselhoClasseNotaDto(long componente)
        {
            return new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = componente,
                Nota = NOTA_7,
                Justificativa = JUSTIFICATIVA
            };
        }
    }
}