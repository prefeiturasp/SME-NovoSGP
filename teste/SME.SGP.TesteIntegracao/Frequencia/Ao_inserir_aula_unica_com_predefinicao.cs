using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_inserir_aula_unica_com_predefinicao : FrequenciaBase
    {
        private DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private DateTime DATA_08_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public Ao_inserir_aula_unica_com_predefinicao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>), typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }


        [Fact]
        public async Task Deve_permitir_inserir_aula_unica_com_predefinicao_padrao()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false);

            var frequencia = new FrequenciaDto()
                {
                    AulaId = AULA_ID_1,
                    ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                    {
                       new RegistroFrequenciaAlunoDto() {
                           Aulas = new List<FrequenciaAulaDto>() { new FrequenciaAulaDto() { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU}},
                           CodigoAluno = CODIGO_ALUNO_99999,
                           TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                       },
                    }
                };

            await InserirFrequenciaUseCaseComValidacaoBasica(frequencia);
        }        

        
    }
}
