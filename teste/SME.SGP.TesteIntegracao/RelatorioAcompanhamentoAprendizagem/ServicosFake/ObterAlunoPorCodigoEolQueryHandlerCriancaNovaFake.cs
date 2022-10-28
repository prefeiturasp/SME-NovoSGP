using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFakes
{
    public class ObterAlunoPorCodigoEolQueryHandlerCriancaNovaFake : IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>
    {
        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorCodigoEolQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CodigoTurma))
                return Obter(request.CodigoTurma).OrderByDescending(a => a.DataSituacao)?.FirstOrDefault();

            return Obter(request.CodigoTurma).Where(da => da.CodigoTurma.ToString().Equals(request.CodigoTurma)).FirstOrDefault(); ; 
        }

        private List<AlunoPorTurmaResposta> Obter(string turmaId)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            return new List<AlunoPorTurmaResposta> {
                new ()
                {
                    CodigoAluno = "1",
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = int.Parse(turmaId),
                    DataNascimento = dataReferencia.AddYears(-5),
                    DataSituacao = dataReferencia.AddDays(5),
                    DataMatricula = dataReferencia.AddDays(5),
                    NomeAluno= "Nome Criança nova 1",
                    NumeroAlunoChamada = 1,
                    SituacaoMatricula = "Ativo",
                    DataAtualizacaoContato = dataReferencia,
                }
            };
        }
    }
}
