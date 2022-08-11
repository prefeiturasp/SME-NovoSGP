using System;
using System.Collections.Generic;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using Xunit;
using System.Linq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_validar_exibicao_alunos : NotaFechamentoTesteBase
    {
        public Ao_validar_exibicao_alunos(CollectionFixture collectionFixture) : base(collectionFixture)
        { }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_exibir_tooltip_alunos_novos_durante_15_dias()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            var retorno = await ExecutarTeste(filtroNotaFechamento);

            retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1)).Informacao.Contains(MensagemNegocioAluno.ESTUDANTE_NOVO).ShouldBeTrue();      
            retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2)).Informacao.Contains(MensagemNegocioAluno.ESTUDANTE_NOVO).ShouldBeTrue();
            (retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3)).Informacao == null).ShouldBeTrue();
            (retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4)).Informacao == null).ShouldBeTrue();
        }

        [Fact]
        public async Task Deve_exibir_tooltip_alunos_inativos_ate_data_sua_inativacao()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            var retorno = await ExecutarTeste(filtroNotaFechamento);

            retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_5)).Informacao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();
            retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_6)).Informacao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();
            retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_7)).Informacao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();
            retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_8)).Informacao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();
            retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_8)).Informacao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();
            retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_10)).Informacao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();
            retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_11)).Informacao.Contains(MensagemNegocioAluno.ESTUDANTE_INATIVO).ShouldBeTrue();
        }
        
        [Fact]
        public async Task Nao_deve_exibir_alunos_inativos_antes_do_comeco_do_ano_ou_bimestre()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(),
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            var retorno = await ExecutarTeste(filtroNotaFechamento);

            (retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_12)) == null).ShouldBeTrue();
            (retorno.Alunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_13)) == null).ShouldBeTrue();
        }
        
        private async Task<FechamentoTurmaDisciplinaBimestreDto> ExecutarTeste(FiltroNotaFechamentoDto filtroNotaFechamentoDto)
        {
            filtroNotaFechamentoDto.CriarPeriodoEscolar = false;
            
            filtroNotaFechamentoDto.CriarPeriodoAbertura = false;
            
            await CriarDadosBase(filtroNotaFechamentoDto);
            
            await InserirPeriodoEscolarCustomizado();

            await InserirFechamentoAluno(filtroNotaFechamentoDto);

            var filtroNotaFechamentoAluno = ObterFiltroNotasFechamentoAlunos(TURMA_CODIGO_1,COMPONENTE_CURRICULAR_PORTUGUES_ID_138,BIMESTRE_1, SEMESTRE_0);
            
            return await ExecutarConsultasFechamentoTurmaDisciplinaComValidacaoAluno(filtroNotaFechamentoAluno);
        }

        private async Task InserirPeriodoEscolarCustomizado()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();
            
            await CriarPeriodoEscolar(dataReferencia.AddDays(-45), dataReferencia.AddDays(+30), BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(40), dataReferencia.AddDays(115), BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(125), dataReferencia.AddDays(200), BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(210), dataReferencia.AddDays(285), BIMESTRE_4, TIPO_CALENDARIO_1);
        }

        private async Task InserirFechamentoAluno(FiltroNotaFechamentoDto filtroNotaFechamentoDto)
        {
            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_1,
                Nota = NOTA_8,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_2,
                Nota = NOTA_7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_3,
                Nota = NOTA_6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_4,
                Nota = NOTA_5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_5,
                Nota = NOTA_7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private FiltroNotaFechamentoAlunosDto ObterFiltroNotasFechamentoAlunos(string turmaCodigo,long disciplinaCodigo, int bimestre, int semestre)
        {
            return new FiltroNotaFechamentoAlunosDto()
            {
                TurmaCodigo = turmaCodigo,
                DisciplinaCodigo = disciplinaCodigo,
                Bimestre = bimestre,
                Semestre = semestre
            };
        }
        
        private FiltroNotaFechamentoDto ObterFiltroNotasFechamento(string perfil, TipoNota tipoNota, string anoTurma,Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, string componenteCurricular , bool considerarAnoAnterior = false, bool ehRegencia = false)
        {
            return new FiltroNotaFechamentoDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
                TipoNota = tipoNota,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = considerarAnoAnterior,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                EhRegencia = ehRegencia
            };
        }
    }
}