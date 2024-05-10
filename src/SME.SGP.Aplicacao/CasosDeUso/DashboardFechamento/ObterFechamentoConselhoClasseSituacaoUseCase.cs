using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoConselhoClasseSituacaoUseCase : AbstractUseCase, IObterFechamentoConselhoClasseSituacaoUseCase
    {
        private FiltroDashboardFechamentoDto parametrosUseCase;
        public ObterFechamentoConselhoClasseSituacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            parametrosUseCase = param ?? throw new ArgumentNullException(nameof(param));
            var conselhoClasse = await mediator.Send(new ObterConselhoClasseSituacaoQuery(parametrosUseCase.UeId,
                                                                                          parametrosUseCase.AnoLetivo,
                                                                                          parametrosUseCase.DreId,
                                                                                          parametrosUseCase.Modalidade,
                                                                                          parametrosUseCase.Semestre,
                                                                                          parametrosUseCase.Bimestre));

            if (conselhoClasse.NaoPossuiRegistros())
                return default;

            var conselhos = new List<GraficoBaseDto>();
            if (parametrosUseCase.EhFiltroPorUe())
                await AdicionaGraficoConselhosClassePorTurma(conselhos, conselhoClasse.GroupBy(c => c.CodigoTurma));
            else
                AdicionaGraficoConselhosClasse(conselhos, conselhoClasse);

            return conselhos.OrderBy(a => a.Grupo).ToList();
        }

        private async Task AdicionaGraficoConselhosClassePorTurma(List<GraficoBaseDto> graficos, IEnumerable<IGrouping<string, ConselhoClasseSituacaoQuantidadeDto>> conselhosClasseTurma)
        {
            foreach (var conselho in conselhosClasseTurma)
            {
                int quantidadeEmAndamentoEConcluido = 0;
                foreach (var informacoesTurma in conselho.ToList())
                    quantidadeEmAndamentoEConcluido += AdicionaGraficoConselhosClassePorTurma(graficos, informacoesTurma);

                if (quantidadeEmAndamentoEConcluido > 0)
                    graficos.Add(new GraficoBaseDto(conselho.FirstOrDefault().AnoTurma, await DefinirQuantidadeConselhoNaoIniciado(conselho.FirstOrDefault().CodigoTurma, parametrosUseCase.Bimestre, quantidadeEmAndamentoEConcluido), 
                                                    SituacaoConselhoClasse.NaoIniciado.Name()));
            }
        }

        private int AdicionaGraficoConselhosClassePorTurma(List<GraficoBaseDto> graficos, ConselhoClasseSituacaoQuantidadeDto conselhoClasseTurma)
        {
            var qdadeConselhosClasseTurma = conselhoClasseTurma.Quantidade > 0 ? conselhoClasseTurma.Quantidade : 0;
            switch (conselhoClasseTurma.Situacao)
            {
                case SituacaoConselhoClasse.EmAndamento:
                    graficos.Add(new GraficoBaseDto(conselhoClasseTurma.AnoTurma, qdadeConselhosClasseTurma, SituacaoConselhoClasse.EmAndamento.Name()));
                    return conselhoClasseTurma.Quantidade;

                case SituacaoConselhoClasse.Concluido:
                    graficos.Add(new GraficoBaseDto(conselhoClasseTurma.AnoTurma, qdadeConselhosClasseTurma, SituacaoConselhoClasse.Concluido.Name()));
                    return conselhoClasseTurma.Quantidade;
                default : return 0;
            }
        }

        private void AdicionaGraficoConselhosClasse(List<GraficoBaseDto> graficos, IEnumerable<ConselhoClasseSituacaoQuantidadeDto> conselhosClasse)
        {
            foreach (var conselho in conselhosClasse.Where(c => c.Quantidade > 0))
                graficos.Add(new GraficoBaseDto(conselho.AnoTurma, conselho.Quantidade, conselho.Situacao.Name()));
        }

        private async Task<int> DefinirQuantidadeConselhoNaoIniciado(string codigoTurma, int bimestre, int quantidadeEmAndamentoEConcluido)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));

            if(turma.NaoEhNulo())
            {
                var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));
                if(periodoEscolar.NaoEhNulo())
                {
                    var alunosDaTurma = await mediator.Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(turma.CodigoTurma)));
                    if (alunosDaTurma.Any() && alunosDaTurma.NaoEhNulo())
                    {
                        int alunosAtivos = RetornaAlunosAtivos(periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim, alunosDaTurma).Count();

                        return alunosAtivos >= quantidadeEmAndamentoEConcluido ? alunosAtivos - quantidadeEmAndamentoEConcluido : 0;
                    }
                }
            }
           
            return 0;
        }

        private IEnumerable<AlunoPorTurmaResposta> RetornaAlunosAtivos(DateTime dataInicio, DateTime dataFim, IEnumerable<AlunoPorTurmaResposta> alunos)
            => from a in alunos
               where a.DataMatricula.Date <= dataFim.Date
               && (!a.Inativo || a.Inativo && a.DataSituacao >= dataInicio.Date)
               group a by new { a.CodigoAluno, a.NumeroAlunoChamada } into grupoAlunos
               orderby grupoAlunos.First().NomeValido(), grupoAlunos.First().NumeroAlunoChamada
               select grupoAlunos.OrderByDescending(a => a.DataSituacao).First();
    }
}