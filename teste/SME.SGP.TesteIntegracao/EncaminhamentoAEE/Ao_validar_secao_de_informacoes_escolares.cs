using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.EncaminhamentoAee.ServicosFake;
using SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_validar_secao_de_informacoes_escolares : EncaminhamentoAEETesteBase
    {
        public Ao_validar_secao_de_informacoes_escolares(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTurmasAlunoPorFiltroQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterNecessidadesEspeciaisAlunoEolQuery, InformacoesEscolaresAlunoDto>), typeof(ObterNecessidadesEspeciaisAlunoEolQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEAnoQuery, AlunoReduzidoDto>), typeof(ObterAlunoPorCodigoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<EhGestorDaEscolaQuery, bool>), typeof(EhGestorDaEscolaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Valide_informacoes_do_object_card()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                AlunoNome = "Nome do aluno 1",
                ResponsavelId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseObterEncaminhamentoPorId();
            var informacoes = await useCase.Executar(1);
            informacoes.ShouldNotBeNull();
            informacoes.Aluno.Nome.ShouldBe("NOME ALUNO 1");
            informacoes.Aluno.DataNascimento.Date.ShouldBe(new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-10).Year, 1, 1).Date);
            informacoes.Aluno.CodigoAluno.ShouldBe("1");
            informacoes.Aluno.Situacao.ShouldBe("RECLASSIFICADO SAIDA");
            informacoes.Aluno.DataSituacao.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(-10).Date);
            informacoes.Aluno.CodigoTurma.ShouldBe("1");
            informacoes.responsavelEncaminhamentoAEE.Id.ShouldBe(2);
        }

        [Fact]
        public async Task Valide_informacoes_da_secao_informacoes_escolares()
        {
            var filtroAee = new FiltroAEEDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtroAee);

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                CodigoAluno = CODIGO_ALUNO_1,
                TurmaId = TURMA_CODIGO_1,
                TotalAulas = 30,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                Bimestre = BIMESTRE_1,
                TotalPresencas = 28,
                TotalAusencias = 1,
                TotalCompensacoes = 1,
                Tipo = TipoFrequenciaAluno.Geral,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseInformacoesEscolares();
            var informacoes = await useCase.Executar(ALUNO_CODIGO_1, TURMA_CODIGO_1);
            
            informacoes.ShouldNotBeNull();
            informacoes.CodigoAluno.ShouldBe(CODIGO_ALUNO_1);
            informacoes.DescricaoNecessidadeEspecial.ShouldBe("Cego");
            informacoes.FrequenciaGlobal.ShouldBe("100,00");
            informacoes.FrequenciaAlunoPorBimestres.ShouldNotBeNull();
            var frenquenciaBimestre = informacoes.FrequenciaAlunoPorBimestres.FirstOrDefault();
            frenquenciaBimestre.Frequencia.ShouldBe(100);
            frenquenciaBimestre.QuantidadeCompensacoes.ShouldBe(1);
            frenquenciaBimestre.QuantidadeCompensacoes.ShouldBe(1);
        }
    }
}
