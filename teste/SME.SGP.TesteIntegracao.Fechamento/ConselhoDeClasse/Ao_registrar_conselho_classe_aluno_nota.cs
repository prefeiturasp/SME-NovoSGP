using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_registrar_conselho_classe_aluno_nota : ConselhoDeClasseTesteBase
    {
        
        public Ao_registrar_conselho_classe_aluno_nota(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerComRegistroFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_inserir_conselho_classe_aluno_nota_conceito()
        {
            var dtoNota = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                Conceito = 1,
                Justificativa = JUSTIFICATIVA
            };

            await ExecuteTeste(dtoNota, TipoNota.Conceito);
        }

        [Fact]
        public async Task Ao_inserir_conselho_classe_aluno_nota_numerica()
        {
            var dtoNota = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                Nota = 6,
                Justificativa = JUSTIFICATIVA
            };

            await ExecuteTeste(dtoNota, TipoNota.Nota);
        }

        [Fact]
        public async Task Ao_alterar_conselho_classe_aluno_nota_conceito()
        {
            var dtoNota = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                Conceito = 1,
                Justificativa = JUSTIFICATIVA
            };

            await ExecuteTeste(dtoNota, TipoNota.Conceito, true);
        }

        [Fact]
        public async Task Ao_alterar_conselho_classe_aluno_nota_numerica()
        {
            var dtoNota = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                Nota = 6,
                Justificativa = JUSTIFICATIVA
            };

            await ExecuteTeste(dtoNota, TipoNota.Nota, true);
        }

        private async Task ExecuteTeste(ConselhoClasseNotaDto dtoNota, TipoNota tipoNota, bool ehAlterar = false)
        {
            await CriarBase(tipoNota, ehAlterar);

            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            var dto = new SalvarConselhoClasseAlunoNotaDto()
            {
                Bimestre = BIMESTRE_1,
                CodigoAluno = ALUNO_CODIGO_1,
                CodigoTurma = TURMA_CODIGO_1,
                FechamentoTurmaId = 1,
                ConselhoClasseNotaDto = dtoNota
            };

            if (ehAlterar)
                dto.ConselhoClasseId = 1;

            var dtoRetorno = await useCase.Executar(dto);
            dtoRetorno.ShouldNotBeNull();

            var listaConselho = ObterTodos<ConselhoClasse>();
            listaConselho.ShouldNotBeNull();
            listaConselho.Any(a=> a.FechamentoTurmaId == 1).ShouldBeTrue();
            var conselhoClasse = listaConselho.FirstOrDefault(f => f.FechamentoTurmaId == 1);
            conselhoClasse.ShouldNotBeNull();

            var listaConselhoAluno = ObterTodos<ConselhoClasseAluno>();
            listaConselhoAluno.ShouldNotBeNull();
            var conselhoAluno = listaConselhoAluno.FirstOrDefault(aluno => aluno.AlunoCodigo == ALUNO_CODIGO_1 && aluno.ConselhoClasseId == conselhoClasse.Id);
            conselhoAluno.ShouldNotBeNull();

            var listaConselhoNota = ObterTodos<ConselhoClasseNota>();
            listaConselhoNota.ShouldNotBeNull();
            var conselhoNota = listaConselhoNota.FirstOrDefault(nota => nota.ConselhoClasseAlunoId == conselhoAluno.Id && 
                                                                        nota.ComponenteCurricularCodigo == dtoNota.CodigoComponenteCurricular);
            conselhoNota.ShouldNotBeNull();
            conselhoNota.Justificativa.ShouldBe(JUSTIFICATIVA);
            if (tipoNota == TipoNota.Conceito)
                conselhoNota.ConceitoId.ShouldBe(1);
            else
                conselhoNota.Nota.ShouldBe(6);
        }

        private async Task CriarBase(TipoNota tipoNota, bool ehAlterar)
        {
            var filtro = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                AnoTurma = tipoNota == TipoNota.Conceito ? ANO_1 : ANO_7,
                CriarFechamentoDisciplinaAlunoNota = true
            };
            
            await CriarDadosBase(filtro);

            if (ehAlterar)
                await InserirConselhoClassePadrao(filtro);
        }
        
        private new async Task InserirConselhoClassePadrao(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            var fechamentoTurmas = ObterTodos<FechamentoTurma>();

            long conselhoClasseId = 1;
	
            long conselhoClasseAlunoId = 1;

            foreach (var fechamentoTurma in fechamentoTurmas)
            {
                await InserirNaBase(new ConselhoClasse()
                {
                    FechamentoTurmaId = fechamentoTurma.Id,
                    Situacao = filtroConselhoClasseDto.SituacaoConselhoClasse,
                    CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
                });
		
                foreach (var alunoCodigo in ObterAlunos())
                {
                    await InserirNaBase(new ConselhoClasseAluno()
                    {
                        ConselhoClasseId = conselhoClasseId,
                        AlunoCodigo = alunoCodigo,
                        CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
                    });

                    foreach (var componenteCurricular in ObterComponentesCurriculares())
                    {
                        await InserirNaBase(new ConselhoClasseNota()
                        {
                            ComponenteCurricularCodigo = componenteCurricular,
                            ConselhoClasseAlunoId = conselhoClasseAlunoId,
                            Justificativa = JUSTIFICATIVA,
                            Nota = filtroConselhoClasseDto.TipoNota == TipoNota.Nota ? new Random().Next(0, 10) : null,
                            ConceitoId = filtroConselhoClasseDto.TipoNota == TipoNota.Conceito ? new Random().Next(1, 3) : null,
                            CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
                        });    
                    }
                    conselhoClasseAlunoId++;
                }

                conselhoClasseId++;
            }
        }
        private static new IEnumerable<long> ObterComponentesCurriculares()
        {
            return new List<long>()
            {
                long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                long.Parse(COMPONENTE_HISTORIA_ID_7),
                long.Parse(COMPONENTE_GEOGRAFIA_ID_8),
                long.Parse(COMPONENTE_CIENCIAS_ID_89),
                long.Parse(COMPONENTE_EDUCACAO_FISICA_ID_6),
                COMPONENTE_CURRICULAR_ARTES_ID_139,
                long.Parse(COMPONENTE_MATEMATICA_ID_2)
            };
        }

        private IEnumerable<string> ObterAlunos()
        {
            return new List<string>()
            {
                ALUNO_CODIGO_1, ALUNO_CODIGO_2, ALUNO_CODIGO_3, ALUNO_CODIGO_4, ALUNO_CODIGO_5
            };
        }
    }
}
