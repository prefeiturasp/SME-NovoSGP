using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes
{
    public class ObterAlunosEolPorCodigosQueryFake : IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>
    {
        public async Task<IEnumerable<TurmasDoAlunoDto>> Handle(ObterAlunosEolPorCodigosQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<TurmasDoAlunoDto>();
            foreach (var codigo in request.CodigosAluno)
            {
                lista.Add(new TurmasDoAlunoDto
                {
                    CodigoAluno = (int)codigo,
                    NomeAluno = $"Nome do Aluno {codigo} ",
                    NomeSocialAluno = $"Nome Social do Aluno {codigo}",
                    CodigoSituacaoMatricula = 1,
                    DataSituacao = DateTimeExtension.HorarioBrasilia(),
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 1,
                });
            }

            return lista;
        }
    }
}