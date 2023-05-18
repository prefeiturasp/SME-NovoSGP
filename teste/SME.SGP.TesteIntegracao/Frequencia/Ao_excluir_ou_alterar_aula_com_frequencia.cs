using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_excluir_ou_alterar_aula_com_frequencia : FrequenciaTesteBase
    {
        public Ao_excluir_ou_alterar_aula_com_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<RecalcularFrequenciaPorTurmaCommand, bool>), typeof(RecalcularFrequenciaPorTurmaCommandHandlerFake), ServiceLifetime.Scoped));
            
        }

        [Fact(DisplayName = "Frequência - Ao excluir aula com frequencia e calculo")]
        public async Task Ao_excluir_aula_com_frequencia_e_calculo()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, DATA_02_05_INICIO_BIMESTRE_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), criarPeriodo:false);

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();

            var dto = new ExcluirAulaDto()
            {
                AulaId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica
            };

            await useCase.Executar(dto);

            var aulas = ObterTodos<Dominio.Aula>();
            aulas = aulas.Where(t => !t.Excluido).ToList();

            aulas.Any().ShouldBeFalse();
            aulas.Count().ShouldBe(0);
        }

        [Fact(DisplayName = "Frequência - Ao diminuir quantidade de aula a frequencia deve ser excluida")]
        public async Task Ao_diminuir_quantidade_de_aula_a_frequencia_deve_ser_excluida()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), true, TIPO_CALENDARIO_1, false, QUANTIDADE_AULA_2);
            await CriarRegistrosConsolidacaoFrequenciaAlunoMensal();
            await CrieRegistroDeFrenquencia();

            var dto = new AulaAlterarFrequenciaRequestDto(AULA_ID_1, QUANTIDADE_AULA_3);
            var mensagem = new MensagemRabbit()
            {
                Mensagem = JsonConvert.SerializeObject(dto)
            };
            var useCase = ServiceProvider.GetService<IAlterarAulaFrequenciaTratarUseCase>();

            await useCase.Executar(mensagem);

            var listaDeRegistroFrequencia = ObterTodos<RegistroFrequenciaAluno>();
            listaDeRegistroFrequencia.ShouldNotBeEmpty();
            listaDeRegistroFrequencia.Exists(frequencia => frequencia.NumeroAula == QUANTIDADE_AULA_3 && !frequencia.Excluido).ShouldBe(false);
            listaDeRegistroFrequencia.Exists(frequencia => frequencia.NumeroAula == QUANTIDADE_AULA_3 && frequencia.Excluido).ShouldBe(true);
        }

        [Fact(DisplayName = "Frequência - Ao aumentar quantidade de aula a frequencia anterior deve ser replicada")]
        public async Task Ao_aumentar_quantidade_de_aula_a_frequencia_anterior_deve_ser_replicada()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, DATA_02_05_INICIO_BIMESTRE_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), true, TIPO_CALENDARIO_1, false, QUANTIDADE_AULA_4);
            await CriarRegistrosConsolidacaoFrequenciaAlunoMensal();
            await CrieRegistroDeFrenquencia();

            var dto = new AulaAlterarFrequenciaRequestDto(AULA_ID_1, QUANTIDADE_AULA_3);
            var mensagem = new MensagemRabbit()
            {
                Mensagem = JsonConvert.SerializeObject(dto)
            };
            var useCase = ServiceProvider.GetService<IAlterarAulaFrequenciaTratarUseCase>();

            await useCase.Executar(mensagem);

            var listaDeRegistroFrequencia = ObterTodos<RegistroFrequenciaAluno>();
            listaDeRegistroFrequencia.ShouldNotBeEmpty();
            var registroAula4 = listaDeRegistroFrequencia.Find(frequencia => frequencia.NumeroAula == QUANTIDADE_AULA_4);
            registroAula4.ShouldNotBeNull();
            registroAula4.Valor.ShouldBe((int)TipoFrequencia.F);
        }

        [Fact(DisplayName = "Frequência - Ao diminuir quantidade de aula recorrente a frequencia deve ser excluida")]
        public async Task Ao_diminuir_quantidade_de_aula_recorrente_a_frequencia_deve_ser_excluida()
        {
            await CriarDadosBasicosAulaRecorrencia(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, 
                                                    DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, DATA_25_07_INICIO_BIMESTRE_3, 
                                                    COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), true, TIPO_CALENDARIO_1, false, QUANTIDADE_AULA_2, QUANTIDADE_AULA_RECORRENTE_2);
            await CriarRegistrosConsolidacaoFrequenciaAlunoMensal();
            var alunos = new string[] { CODIGO_ALUNO_1, CODIGO_ALUNO_2, CODIGO_ALUNO_3 };
            await CrieRegistroDeFrenquenciaTodasAulas(alunos, QUANTIDADE_AULA_3);

            var listaDeRegistroFrequencia = ObterTodos<RegistroFrequenciaAluno>();
            listaDeRegistroFrequencia.Where(frequencia => frequencia.NumeroAula == QUANTIDADE_AULA_3).Count().ShouldBe(alunos.Count() * QUANTIDADE_AULA_NORMAL_MAIS_RECORRENTES_3);

            var useCase = ServiceProvider.GetService<IAlterarAulaFrequenciaTratarUseCase>();

            var aulas = ObterTodos<Dominio.Aula>();
            foreach(var aula in aulas)
            {
                var dto = new AulaAlterarFrequenciaRequestDto(aula.Id, QUANTIDADE_AULA_3);
                var mensagem = new MensagemRabbit()
                {
                    Mensagem = JsonConvert.SerializeObject(dto)
                };
                await useCase.Executar(mensagem);
            }


            listaDeRegistroFrequencia = ObterTodos<RegistroFrequenciaAluno>();
            listaDeRegistroFrequencia.ShouldNotBeEmpty();
            listaDeRegistroFrequencia.Where(frequencia => frequencia.NumeroAula == QUANTIDADE_AULA_3 && !frequencia.Excluido).Count().ShouldBe(0);
            listaDeRegistroFrequencia.Where(frequencia => frequencia.NumeroAula == QUANTIDADE_AULA_3 && frequencia.Excluido).Count().ShouldBe(alunos.Count() * QUANTIDADE_AULA_NORMAL_MAIS_RECORRENTES_3);
        }
    }
}
