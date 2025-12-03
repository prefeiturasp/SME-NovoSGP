using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class Ao_listar_periodos_e_predefinicao : ListaoTesteBase
    {
        public Ao_listar_periodos_e_predefinicao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        private IObterPeriodosPorComponenteUseCase ObterPeriodosPorComponenteUseCase()
        {
            return ServiceProvider.GetService<IObterPeriodosPorComponenteUseCase>();
        }

        [Fact(DisplayName = "Validar se os períodos estão sendo listados corretamente - semanal")]
        public async Task Deve_listar_periodos_semanal_regencia_classe()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };
            
            await CriarDadosBasicos(filtroListao);
            await CriarRegistroFrenquencia(filtroListao.Bimestre, filtroListao.ComponenteCurricularId);
            
            var useCase = ObterPeriodosPorComponenteUseCase();

            var listaPeriodo = (await useCase.Executar(TURMA_CODIGO_1, filtroListao.ComponenteCurricularId, true,
                filtroListao.Bimestre, true)).ToList();
            
            listaPeriodo.ShouldNotBeNull();

            var periodoEscolar = ObterTodos<PeriodoEscolar>().FirstOrDefault(c => c.Bimestre == filtroListao.Bimestre);
            periodoEscolar.ShouldNotBeNull();

            var qtdeSemanas = DateAndTime.DateDiff(DateInterval.WeekOfYear, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim) + 1;
            qtdeSemanas.ShouldBe(listaPeriodo.Count);            
        }

        [Fact(DisplayName = "Validar se os períodos estão sendo listados corretamente - 5 dias com aula")]
        public async Task Deve_listar_periodos_5_dias_com_aula()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };
            
            await CriarDadosBasicos(filtroListao);
            await CriarRegistroFrenquencia(filtroListao.Bimestre, filtroListao.ComponenteCurricularId);
            
            var useCase = ObterPeriodosPorComponenteUseCase();

            var listaPeriodo = (await useCase.Executar(TURMA_CODIGO_1, filtroListao.ComponenteCurricularId, false,
                filtroListao.Bimestre)).ToList();

            listaPeriodo.ShouldNotBeNull();
            
            var mediator = ServiceProvider.GetService<IMediator>();
            mediator.ShouldNotBeNull();
            
            var periodosEscolares = (await mediator.Send(new ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery(TURMA_CODIGO_1, new long[] { filtroListao.ComponenteCurricularId },
                filtroListao.Bimestre, false))).Where(c => c.DataAula <= DateTimeExtension.HorarioBrasilia()).ToList();

            const int qtdeLimiteDeAulas = 5;
            var contador = 1;

            foreach (var periodo in listaPeriodo)
            {
                var posicaoInicial = (contador - 1) * qtdeLimiteDeAulas;

                var itens = periodosEscolares.Skip(posicaoInicial)
                    .Take(qtdeLimiteDeAulas).ToList();
                
                var dataInicioValidar = itens.FirstOrDefault()?.DataAula.Date;
                var dataFimValidar = itens.LastOrDefault()?.DataAula.Date;
                
                (dataInicioValidar == periodo.DataInicio).ShouldBeTrue();
                (dataFimValidar == periodo.DataFim).ShouldBeTrue();

                contador++;
            }
            
            Math.Ceiling((decimal)periodosEscolares.Count / qtdeLimiteDeAulas).ShouldBe(listaPeriodo.Count);
        }

        [Fact(DisplayName = "Verificar se a frequência predefinida é sugerida no listão")]
        public async Task Deve_sugerir_frequencia_pre_definida()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };
            
            await CriarDadosBasicos(filtroListao);
                       
            var useCasePeriodo = ObterPeriodosPorComponenteUseCase();
            var listaPeriodo = (await useCasePeriodo.Executar(TURMA_CODIGO_1, filtroListao.ComponenteCurricularId, false,
                filtroListao.Bimestre, true)).ToList();
            
            listaPeriodo.ShouldNotBeNull();
            
            var periodoSelecionado = listaPeriodo.FirstOrDefault();
            periodoSelecionado.ShouldNotBeNull();
        
            var useCase = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCase.ShouldNotBeNull();
        
            var frequencia = await useCase.Executar(new FiltroFrequenciaPorPeriodoDto
            {
                DataInicio = periodoSelecionado.DataInicio,
                DataFim = periodoSelecionado.DataFim,
                DisciplinaId = filtroListao.ComponenteCurricularId.ToString(),
                ComponenteCurricularId = filtroListao.ComponenteCurricularId.ToString(),
                TurmaId = TURMA_CODIGO_1
            });
            
            frequencia.ShouldNotBeNull();
            
            var listaCodigoAluno = frequencia.Alunos.Select(c => c.CodigoAluno).Distinct().ToList();
            listaCodigoAluno.ShouldNotBeNull();
            
            var listaFrequenciaPreDefinida = ObterTodos<FrequenciaPreDefinida>();
            listaFrequenciaPreDefinida.ShouldNotBeNull();
        
            foreach (var codigoAluno in listaCodigoAluno)
            {
                var frequenciaPreDefinidaAluno = listaFrequenciaPreDefinida
                    .FirstOrDefault(c => c.CodigoAluno == codigoAluno && 
                                         c.ComponenteCurricularId == filtroListao.ComponenteCurricularId && 
                                         c.TurmaId == TURMA_ID_1);
                
                frequenciaPreDefinidaAluno.ShouldNotBeNull();
        
                var aulasAluno = frequencia.Alunos.FirstOrDefault(c => c.CodigoAluno == codigoAluno)?.Aulas;
                aulasAluno.ShouldNotBeNull();

                if (aulasAluno.Any(x => x.DetalheFrequencia?.Any() ?? false))
                    foreach (var aula in aulasAluno)
                        aula.TipoFrequencia.ShouldBe(frequenciaPreDefinidaAluno.TipoFrequencia.ObterNomeCurto());
            }
        }
    }
}