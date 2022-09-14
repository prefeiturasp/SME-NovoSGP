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
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAula.Base;
using SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Teste : PlanoAEETesteBase
    {
        public Teste(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Teste_Inicial()
        {
            var salvarPlanoAEEUseCase = ObterServicoSalvarPlanoAEEUseCase();

            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });

            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Questoes = new List<PlanoAEEQuestaoDto>(),
                Situacao = SituacaoPlanoAEE.Validado,
                TurmaCodigo = TURMA_CODIGO_1,
                ResponsavelRF = USUARIO_PROFESSOR_LOGIN_1111111,
            };
            
            var retorno = await salvarPlanoAEEUseCase.Executar(planoAEEPersistenciaDto);
            retorno.ShouldNotBeNull();
            
        }
    }
}