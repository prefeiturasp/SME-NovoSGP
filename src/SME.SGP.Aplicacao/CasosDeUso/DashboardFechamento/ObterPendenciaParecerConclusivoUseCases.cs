using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaParecerConclusivoUseCases : AbstractUseCase, IObterPendenciaParecerConclusivoUseCases
    {
        public ObterPendenciaParecerConclusivoUseCases(IMediator mediator) : base(mediator)
        {
        }
        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var parecerConclusivo = await mediator.Send(new ObterPendenciaParecerConclusivoSituacaoQuery(param.UeId, param.AnoLetivo,
                        param.DreId,
                        param.Modalidade,
                        param.Semestre,
                        param.Bimestre));

            if (parecerConclusivo.NaoPossuiRegistros())
                return default;

            List<GraficoBaseDto> parecerConclusivos = new List<GraficoBaseDto>();

            await AdicionaGraficoPareceresConslusivosPorTurma(parecerConclusivos, parecerConclusivo.GroupBy(p => p.TurmaCodigo));
            return parecerConclusivos.OrderBy(a => a.Grupo).ToList();
        }

        private async Task AdicionaGraficoPareceresConslusivosPorTurma(List<GraficoBaseDto> graficos, IEnumerable<IGrouping<string, ParecerConclusivoSituacaoQuantidadeDto>> pareceresConslusivosTurma)
        {
            foreach (var parecerTurma in pareceresConslusivosTurma)
            {
                var alunosAtivos = await ObterAlunosAtivosTurma(parecerTurma.Key);
                var qdadeAlunosInativosComParecerConclusivo = 0;

                var grupo = $"{parecerTurma.FirstOrDefault().AnoTurma}";
                foreach (var parecerPorSituacao in parecerTurma.GroupBy(s => s.Situacao))
                {
                    var quantidadesPareceresConclusicosSituacao = ObterQdadePareceresConslusivosPorSituacao(parecerPorSituacao, alunosAtivos);
                    qdadeAlunosInativosComParecerConclusivo += quantidadesPareceresConclusicosSituacao.alunosInativosComParecer;
                    if (quantidadesPareceresConclusicosSituacao.quantidadeParecer > 0)
                        graficos.Add(new GraficoBaseDto(grupo, quantidadesPareceresConclusicosSituacao.quantidadeParecer, parecerPorSituacao.FirstOrDefault().Situacao));
                }

                var quantidade = ObterQdadeAlunosAtivosComParecerConclusivo(parecerTurma, alunosAtivos, qdadeAlunosInativosComParecerConclusivo);
                graficos.Add(new GraficoBaseDto(parecerTurma.FirstOrDefault().AnoTurma, quantidade, "Sem parecer"));
            }
        }

        private int ObterQdadeAlunosAtivosComParecerConclusivo(IEnumerable<ParecerConclusivoSituacaoQuantidadeDto> pareceresConslusivosTurma, 
                                                                                                              IEnumerable<AlunoPorTurmaResposta> alunosAtivos,
                                                                                                              int qdadeAlunosInativosComParecerConclusivo)
        {
            var quantidadePareceres = pareceresConslusivosTurma.Count();
            var alunosAtivosComParecer = Math.Max(quantidadePareceres - qdadeAlunosInativosComParecerConclusivo, 0);  
            return Math.Max(alunosAtivos.Count() - alunosAtivosComParecer, 0);
        }

        private (int quantidadeParecer, int alunosInativosComParecer) ObterQdadePareceresConslusivosPorSituacao(IEnumerable<ParecerConclusivoSituacaoQuantidadeDto> pareceresConslusivosTurmaSituacao, IEnumerable<AlunoPorTurmaResposta> alunosAtivos)
        {
            var quantidadeParecer = 0;
            var alunosInativosComParecer = 0;
            foreach (var parecer in pareceresConslusivosTurmaSituacao)
            {
                if (parecer.Quantidade > 0 && alunosAtivos.Any(a => a.CodigoAluno == parecer.AlunoCodigo))
                    quantidadeParecer++;
                else
                    alunosInativosComParecer++;
            }
            return (quantidadeParecer, alunosInativosComParecer);
        }

        private async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosAtivosTurma(string turmaCodigo)
        {
            var alunos = await mediator.Send(new ObterAlunosPorTurmaQuery(turmaCodigo));
            return alunos.Where(a => a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo || a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);
        }
    }
}
