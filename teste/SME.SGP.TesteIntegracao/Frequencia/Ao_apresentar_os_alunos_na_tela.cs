using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.TesteIntegracao.Frequencia.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_apresentar_os_alunos_na_tela : FrequenciaTesteBase
    {
        public Ao_apresentar_os_alunos_na_tela(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFakeValidarAlunosFrequencia), ServiceLifetime.Scoped));
        }
        
        [Fact(DisplayName = "Frequência - Deve exibir tooltip alunos novos durante 15 dias")]
        public async Task Deve_exibir_tooltip_alunos_novos_durante_15_dias()
        {
            var retorno = await ExecutarTesteToolTip();

            var retornoAluno = retorno.ListaFrequencia;
            retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1)).Marcador.Descricao.Contains(MensagemNegocioAluno.ESTUDANTE_NOVO).ShouldBeTrue();      
            retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3)).Marcador.Descricao.Contains(MensagemNegocioAluno.ESTUDANTE_NOVO).ShouldBeTrue();
            (retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4)).Marcador == null).ShouldBeTrue();
            (retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2)).Marcador == null).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Deve exibir tooltip alunos inativos ate data sua inativacao")]
        public async Task Deve_exibir_tooltip_alunos_inativos_ate_data_sua_inativacao()
        {
            var retorno = await ExecutarTesteToolTip();
            var retornoAluno = retorno.ListaFrequencia;
            
            retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_5)).Marcador.Descricao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();  
            retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_6)).Marcador.Descricao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();  
            retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_7)).Marcador.Descricao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();  
            retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_8)).Marcador.Descricao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();  
            retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_9)).Marcador.Descricao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();  
            retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_10)).Marcador.Descricao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();  
            retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_11)).Marcador.Descricao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();  
        }

        [Fact(DisplayName = "Frequência - Nao deve exibir alunos inativos antes do comeco do ano ou bimestre")]
        public async Task Nao_deve_exibir_alunos_inativos_antes_do_comeco_do_ano_ou_bimestre()
        {
            var retorno = await ExecutarTesteToolTip();
            
            var retornoAluno = retorno.ListaFrequencia;
            
            (retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_12)) == null).ShouldBeTrue();
            
            (retornoAluno.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_13)) == null).ShouldBeTrue();
        }

        private async Task InserirPeriodoEscolarCustomizado()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();
            
            await CriarPeriodoEscolar(dataReferencia.AddDays(-45), dataReferencia.AddDays(+30), BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(40), dataReferencia.AddDays(115), BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(125), dataReferencia.AddDays(200), BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(210), dataReferencia.AddDays(285), BIMESTRE_4, TIPO_CALENDARIO_1);
        }
        
        private async Task<FrequenciaDto> ExecutarTesteToolTip()
        {
            await CriarDadosBasicosSemPeriodoEscolar(ObterPerfilProfessor(), Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio, DateTimeExtension.HorarioBrasilia().Date,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), NUMERO_AULAS_1);

            await InserirParametroSistema(true);

            await InserirPeriodoEscolarCustomizado();

            var useCase = ServiceProvider.GetService<IObterFrequenciaPorAulaUseCase>();

            var filtroFrequencia = new FiltroFrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            var retorno = await useCase.Executar(filtroFrequencia);
            return retorno;
        }
    }
}