using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_inserir_nota_pos_conselho_professor_regente_anterior : ConselhoDeClasseTesteBase
    {
        public Ao_inserir_nota_pos_conselho_professor_regente_anterior(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerSemPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerSemPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_inserir_nota_pos_conselho_professor_regente_anterior_com_atribuicao_encerrada()
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Nota);

            await CriarDados(ObterPerfilProfessor(),
                            salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular,
                            ANO_7,
                            Modalidade.Fundamental,
                            ModalidadeTipoCalendario.FundamentalMedio);

            var comando = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Executar(salvarConselhoClasseAlunoNotaDto));

            excecao.Message.ShouldBe(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }

        private async Task CriarDados(
                string perfil,
                long componente,
                string anoTurma,
                Modalidade modalidade,
                ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = componente.ToString(),
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = true,
                DataAula = DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1),
                CriarFechamentoDisciplinaAlunoNota = true
            };

            await CriarDadosBase(filtroNota);
        }
    }
}
