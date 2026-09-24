using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_comparar_json_fechamento_bimestre : NotaFechamentoTesteBase
    {
        private readonly ITestOutputHelper output;

        public Ao_comparar_json_fechamento_bimestre(CollectionFixture collectionFixture, ITestOutputHelper output)
            : base(collectionFixture)
        {
            this.output = output;
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(
                typeof(MediatR.IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos),
                ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_preservar_resposta_json_do_fechamento_bimestral()
        {
            var filtro = ObterFiltroNotas(
                ObterPerfilProfessor(), ANO_3, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota.Nota, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);
            filtro.CriarPeriodoEscolar = false;
            filtro.CriarPeriodoAbertura = false;
            await CriarDadosBase(filtro);
            var referencia = DateTimeExtension.HorarioBrasilia();
            await CriarPeriodoEscolar(referencia.AddDays(-45), referencia.AddDays(30), BIMESTRE_1, TIPO_CALENDARIO_1);

            var turmaId = ObterTodos<SME.SGP.Dominio.Turma>().First().Id;
            var periodoId = ObterTodos<PeriodoEscolar>().First().Id;
            var criadoEm = new DateTime(2025, 1, 1, 12, 0, 0);
            var fechamentoTurmaId = await InserirNaBaseAsync(new FechamentoTurma
            {
                TurmaId = turmaId, PeriodoEscolarId = periodoId, CriadoEm = criadoEm,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            var fechamentoId = await InserirNaBaseAsync(new FechamentoTurmaDisciplina
            {
                FechamentoTurmaId = fechamentoTurmaId, DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Situacao = SituacaoFechamento.ProcessadoComSucesso, CriadoEm = criadoEm,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            var alunoId = await InserirNaBaseAsync(new FechamentoAluno
            {
                FechamentoTurmaDisciplinaId = fechamentoId, AlunoCodigo = CODIGO_ALUNO_1,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBaseAsync(new FechamentoNota
            {
                FechamentoAlunoId = alunoId, DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = 7, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var consultaFiltro = new FiltroNotaFechamentoAlunosDto
            {
                TurmaCodigo = TURMA_CODIGO_1,
                DisciplinaCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Bimestre = BIMESTRE_1,
                Semestre = SEMESTRE_1
            };
            var retorno = await ExecutarConsultasFechamentoTurmaDisciplinaComValidacaoAluno(consultaFiltro);

            Assert.Equal(fechamentoId, retorno.FechamentoId);
            Assert.Contains(retorno.Alunos, aluno => aluno.CodigoAluno == CODIGO_ALUNO_1);
            var json = JsonSerializer.Serialize(retorno);
            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(json)));
            output.WriteLine($"US155460_JSON_SHA256={hash};BYTES={Encoding.UTF8.GetByteCount(json)}");
        }
    }
}
