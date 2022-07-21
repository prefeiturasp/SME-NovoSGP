using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_lancar_nota_numerica : NotaFechamentoBimestreTesteBase
    {
        public Ao_lancar_nota_numerica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_lancar_nota_para_fundamental()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina);

            await CriarDadosBase(filtroFechamentoNota);

            var fechamentoNota = await LancarNotasAlunos();

            await ExecutarTeste(fechamentoNota);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);
        }

        [Fact]
        public async Task Deve_lancar_nota_para_medio()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                Modalidade.Medio,
                ANO_8,
                TipoFrequenciaAluno.PorDisciplina);

            await CriarDadosBase(filtroFechamentoNota);

            var fechamentoNota = await LancarNotasAlunos();

            await ExecutarTeste(fechamentoNota);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);
        }

        [Fact]
        public async Task Deve_lancar_nota_para_eja()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.EJA,
                false,
                Modalidade.EJA,
                ANO_9,
                TipoFrequenciaAluno.PorDisciplina);

            await CriarDadosBase(filtroFechamentoNota);

            var fechamentoNota = await LancarNotasAlunos();

            await ExecutarTeste(fechamentoNota);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);
        }

        private static async Task<List<FechamentoTurmaDisciplinaDto>> LancarNotasAlunos()
        {
            string[] alunosCodigos = new string[] { CODIGO_ALUNO_1, CODIGO_ALUNO_2, CODIGO_ALUNO_3, CODIGO_ALUNO_4 };
            var fechamentosNotas = new List<FechamentoNotaDto>();

            foreach (var alunoCodigo in alunosCodigos)
            {
                Random randomNota = new();

                var nota = randomNota.Next(0, 10);

                var fechamentoNota = new FechamentoNotaDto()
                {
                    CodigoAluno = alunoCodigo,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Nota = nota,
                    ConceitoId = null,
                    SinteseId = null,
                    Anotacao = $"Anotação fechamento teste de integração do aluno {alunoCodigo}.",
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoRf = SISTEMA_CODIGO_RF,
                    CriadoPor = SISTEMA_NOME,
                };

                fechamentosNotas.Add(fechamentoNota);
            }

            var fechamentoTurma = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = BIMESTRE_1,
                    DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Justificativa = "",
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = fechamentosNotas
                }
            };

            return await Task.FromResult(fechamentoTurma);
        }

        private async Task ExecutarTeste(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma)
        {
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();            
            await comando.Salvar(fechamentoTurma);
        }

        private static async Task<FiltroFechamentoNotaDto> ObterFiltroFechamentoNota(string perfil, ModalidadeTipoCalendario tipoCalendario,
            bool considerarAnoAnterior, Modalidade modalidade, string anoTurma, TipoFrequenciaAluno tipoFrequenciaAluno)
        {
            return await Task.FromResult(new FiltroFechamentoNotaDto
            {
                Perfil = perfil,
                TipoCalendario = tipoCalendario,
                ConsiderarAnoAnterior = considerarAnoAnterior,
                Modalidade = modalidade,
                AnoTurma = anoTurma,
                TipoFrequenciaAluno = tipoFrequenciaAluno
            });
        }
    }
}
