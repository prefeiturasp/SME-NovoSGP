using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacoesDosAlunosNoHistoricoEscolarUseCase : AbstractUseCase, IObterObservacoesDosAlunosNoHistoricoEscolarUseCase
    {
        public ObterObservacoesDosAlunosNoHistoricoEscolarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<AlunoComObservacaoDoHistoricoEscolarDto>> Executar(string turmaCodigo)
        {
            var resultado = new PaginacaoResultadoDto<AlunoComObservacaoDoHistoricoEscolarDto>();
            var dadosAlunos = await mediator.Send(new ObterAlunosSimplesDaTurmaQuery(turmaCodigo));
            var alunosObs = new List<AlunoComObservacaoDoHistoricoEscolarDto>();
            var codigosAlunos = dadosAlunos.Select(aluno => aluno.Codigo).ToArray();
            var observacoes = (await mediator.Send(new ObterObservacoesDosAlunosNoHistoricoEscolarQuery(codigosAlunos))).ToList();

            foreach (var aluno in dadosAlunos)
            {
                alunosObs.Add(new AlunoComObservacaoDoHistoricoEscolarDto()
                {
                    NumeroChamada = aluno.NumeroChamada,
                    Codigo = aluno.Codigo,
                    Nome = aluno.Nome,
                    Observacao = observacoes.Find(obs => obs.AlunoCodigo == aluno.Codigo)?.Observacao ?? string.Empty
                });
            }

            resultado.Items = alunosObs;
            resultado.TotalPaginas = 1;
            resultado.TotalRegistros = resultado.Items.Count();

            return resultado;
        }
    }
}
