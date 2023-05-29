using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoConselhoClasseSituacaoUseCase : AbstractUseCase, IObterFechamentoConselhoClasseSituacaoUseCase
    {
        public ObterFechamentoConselhoClasseSituacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var conselhoClasse = await mediator.Send(new ObterConselhoClasseSituacaoQuery(param.UeId,
                                                                                          param.AnoLetivo,
                                                                                          param.DreId,
                                                                                          param.Modalidade,
                                                                                          param.Semestre,
                                                                                          param.Bimestre));

            if (conselhoClasse == null || !conselhoClasse.Any())
                return default;

            var conselhos = new List<GraficoBaseDto>();

            if(param.UeId == 0)
            {
                foreach (var conselho in conselhoClasse.Where(c => c.Quantidade > 0).ToList())
                    conselhos.Add(new GraficoBaseDto(conselho.AnoTurma, conselho.Quantidade, conselho.Situacao.Name()));
            }                
            else
            {
                foreach (var conselho in conselhoClasse.GroupBy(c => c.CodigoTurma))
                {
                    int quantidadeEmAndamentoEConcluido = 0;

                    foreach(var informacoesTurma in conselho.ToList())
                    {
                        switch(informacoesTurma.Situacao)
                        {
                            case SituacaoConselhoClasse.EmAndamento:
                                conselhos.Add(new GraficoBaseDto(informacoesTurma.AnoTurma, informacoesTurma.Quantidade > 0 ? informacoesTurma.Quantidade : 0, SituacaoConselhoClasse.EmAndamento.Name()));
                                quantidadeEmAndamentoEConcluido += informacoesTurma.Quantidade;
                                break;

                            case SituacaoConselhoClasse.Concluido:
                                conselhos.Add(new GraficoBaseDto(informacoesTurma.AnoTurma, informacoesTurma.Quantidade > 0 ? informacoesTurma.Quantidade : 0, SituacaoConselhoClasse.Concluido.Name()));
                                quantidadeEmAndamentoEConcluido += informacoesTurma.Quantidade;
                                break;
                        }

                        
                    }

                    if(quantidadeEmAndamentoEConcluido > 0)
                        conselhos.Add(new GraficoBaseDto(conselho.FirstOrDefault().AnoTurma, await DefinirQuantidadeConselhoNaoIniciado(conselho.FirstOrDefault().CodigoTurma, param.Bimestre, quantidadeEmAndamentoEConcluido), SituacaoConselhoClasse.NaoIniciado.Name()));
                }
            }

            return conselhos.OrderBy(a => a.Grupo).ToList();
        }

        private async Task<int> DefinirQuantidadeConselhoNaoIniciado(string codigoTurma, int bimestre, int quantidadeEmAndamentoEConcluido)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));

            if(turma != null)
            {
                var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));
                if(periodoEscolar != null)
                {
                    var alunosDaTurma = await mediator.Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(turma.CodigoTurma)));
                    if (alunosDaTurma.Any() && alunosDaTurma != null)
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