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

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterAlunoPorCodigoEolQueryHandlerAluno1AtivoFake : IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>
    {
        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorCodigoEolQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CodigoTurma))
                return Obter(request.CodigoTurma).OrderByDescending(a => a.DataSituacao)?.FirstOrDefault();

            return await Task.FromResult(Obter(request.CodigoTurma).Where(da => da.CodigoTurma.ToString().Equals(request.CodigoTurma)).FirstOrDefault()); 
        }

        private static List<AlunoPorTurmaResposta> Obter(string turmaId)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            return new List<AlunoPorTurmaResposta> {
                new AlunoPorTurmaResposta
                {
                    Ano=0,
                    CodigoAluno = "1",
                    CodigoComponenteCurricular= 0,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Rematriculado,
                    CodigoTurma=int.Parse(turmaId),
                    DataNascimento=new DateTime(dataReferencia.AddYears(-15).Year,01,16,00,00,00),
                    DataSituacao= new DateTime(dataReferencia.Year,11,09,17,25,31),
                    DataMatricula= new DateTime(dataReferencia.Year,11,09,17,25,31),
                    EscolaTransferencia=null,
                    NomeAluno= "Nome do aluno ativo de código 1",
                    NomeSocialAluno=null,
                    NumeroAlunoChamada=1,
                    ParecerConclusivo=null,
                    PossuiDeficiencia=false,
                    SituacaoMatricula="ATIVO",
                    Transferencia_Interna=false,
                    TurmaEscola=null,
                    TurmaRemanejamento=null,
                    TurmaTransferencia=null,
                    DataAtualizacaoContato= new DateTime(dataReferencia.AddYears(-1).Year,06,22,19,02,35),
                }
            };
        }
    }
}
