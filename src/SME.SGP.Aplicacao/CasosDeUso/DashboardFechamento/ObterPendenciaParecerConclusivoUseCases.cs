using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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

            if (parecerConclusivo == null || !parecerConclusivo.Any())
                return default;

            List<GraficoBaseDto> parecerConclusivos = new List<GraficoBaseDto>();

            foreach (var parecerAgrupado in parecerConclusivo.GroupBy(p => p.TurmaCodigo))
            {
                var alunos = await mediator.Send(new ObterAlunosPorTurmaQuery(parecerAgrupado.Key));
                var alunosAtivos = alunos.Where(a => a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo || a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);
                var alunosInativosComParecer = 0;                

                var grupo = $"{parecerAgrupado.FirstOrDefault().AnoTurma}";
                foreach (var parecerPorSituacao in parecerAgrupado.GroupBy(s => s.Situacao))
                {
                    var quantidadeParecer = 0;
                    foreach (var parecer in parecerPorSituacao)
                    {
                        if (parecer.Quantidade > 0 && alunosAtivos.Any(a => a.CodigoAluno == parecer.AlunoCodigo))
                            quantidadeParecer++;
                        else
                            alunosInativosComParecer++;
                    }
                    if (quantidadeParecer > 0)
                        parecerConclusivos.Add(new GraficoBaseDto(grupo, quantidadeParecer, parecerPorSituacao.FirstOrDefault().Situacao));
                }
                var quantidadePareceres = parecerAgrupado.Where(p => p.TurmaCodigo == parecerAgrupado.Key).Count();
                var alunosAtivosComParecer = quantidadePareceres > 0
                    ?
                    quantidadePareceres - alunosInativosComParecer
                    :
                    0;
                var quantidade = alunosAtivos.Count() - alunosAtivosComParecer;

                parecerConclusivos.Add(new GraficoBaseDto(parecerAgrupado.FirstOrDefault().AnoTurma, quantidade, "Sem parecer"));
            }

            return parecerConclusivos.OrderBy(a => a.Grupo).ToList();
        }
    }
}
