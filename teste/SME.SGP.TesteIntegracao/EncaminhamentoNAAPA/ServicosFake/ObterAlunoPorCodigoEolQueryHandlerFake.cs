using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class
        ObterAlunoPorCodigoEolQueryHandlerFakeNAAPA : IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>
    {
        
        public ObterAlunoPorCodigoEolQueryHandlerFakeNAAPA()
        {}

        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorCodigoEolQuery request,
            CancellationToken cancellationToken)
        {
            var alunos = new List<AlunoPorTurmaResposta>()
              {
                  new AlunoPorTurmaResposta() {
                    CodigoAluno = "1",
                    NomeAluno = "Nome do aluno 1",
                    DataMatricula = DateTimeExtension.HorarioBrasilia().AddYears(-10).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    DataSituacao = DateTimeExtension.HorarioBrasilia().AddMonths(-120).Date,
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    NumeroAlunoChamada = 1,
                    TurmaEscola = "Modalidade - Nome Turno",
                    CodigoTurma = 222222,
                    CelularResponsavel = "99999999999",
                    NomeResponsavel = "Responsável do aluno 1",
                    DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia().Date.AddDays(-30),
                    TipoResponsavel = Dominio.TipoResponsavel.ResponsavelLegal.Name()
                  }
            };

            return await Task.Run(() => alunos.Where(aluno => aluno.CodigoAluno == request.CodigoAluno).FirstOrDefault());
        }
    }
}