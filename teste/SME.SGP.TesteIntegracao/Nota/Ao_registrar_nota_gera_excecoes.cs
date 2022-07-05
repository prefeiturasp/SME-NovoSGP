using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class Ao_registrar_nota_gera_excecoes : NotaBase
    {
        public Ao_registrar_nota_gera_excecoes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Nao_lancar_nota_do_professor_com_atribuicao_encerrada()
        {
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerSemPermissaoFake), ServiceLifetime.Scoped));
            _collectionFixture.BuildServiceProvider();

            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);

            var dto = new NotaConceitoListaDto()
            {
                DisciplinaId = filtroNota.ComponenteCurricular,
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_1,
                        Nota = 7,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    }
                }
            };

            var comando = ServiceProvider.GetService<IComandosNotasConceitos>();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Salvar(dto));

            excecao.Message.ShouldBe(MensagensNegocioLancamentoNota.Nao_pode_fazer_alteracoes_nesta_turma_componente_e_data);
        }

  
    }
}
