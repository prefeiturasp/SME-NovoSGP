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
                foreach(var parecer in parecerAgrupado)
                {
                    var grupo = $"{parecer.AnoTurma}";
                    if (parecer.Quantidade > 0)
                        parecerConclusivos.Add(new GraficoBaseDto(grupo, parecer.Quantidade, parecer.Situacao));
                }
                var alunos = await mediator.Send(new ObterAlunosPorTurmaQuery(parecerAgrupado.Key));

                var alunosAtivos = alunos.Where(a => a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo || a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);
                var quantidade = alunosAtivos.Count() - parecerAgrupado.Where(p => p.TurmaCodigo == parecerAgrupado.Key).Count();
                parecerConclusivos.Add(new GraficoBaseDto(parecerAgrupado.FirstOrDefault().AnoTurma , quantidade, "Sem parecer"));
            }

            return parecerConclusivos.OrderBy(a => a.Grupo).ToList();
        }
    }
}
