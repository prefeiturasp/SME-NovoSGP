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
    internal class ObterAlunoPorCodigoEolQueryHandlerFake : IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>
    {
        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorCodigoEolQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CodigoTurma))
            {
                return ObtenhaLista().OrderByDescending(a => a.DataSituacao)?.FirstOrDefault();
            }

            return ObtenhaLista().Where(da => da.CodigoTurma.ToString().Equals(request.CodigoTurma)).FirstOrDefault(); ; 
        }

        private List<AlunoPorTurmaResposta> ObtenhaLista()
        {
            return new List<AlunoPorTurmaResposta> {
                new AlunoPorTurmaResposta
                {
                      Ano=0,
                      CodigoAluno = "11223344",
                      CodigoComponenteCurricular=0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=1,
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(2021,11,09,17,25,31),
                      DataMatricula= new DateTime(2021,11,09,17,25,31),
                      EscolaTransferencia=null,
                      NomeAluno="Maria Aluno teste",
                      NomeSocialAluno=null,
                      NumeroAlunoChamada=1,
                      ParecerConclusivo=null,
                      PossuiDeficiencia=false,
                      SituacaoMatricula="Ativo",
                      Transferencia_Interna=false,
                      TurmaEscola=null,
                      TurmaRemanejamento=null,
                      TurmaTransferencia=null,
                      NomeResponsavel="João teste",
                      TipoResponsavel="4",
                      CelularResponsavel="",
                      DataAtualizacaoContato= new DateTime(2018,06,22,19,02,35),
                },
                new AlunoPorTurmaResposta
                {
                      Ano=0,
                      CodigoAluno = "11223344",
                      CodigoComponenteCurricular=0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=2,
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(2022,11,09,17,25,31),
                      DataMatricula= new DateTime(2021,11,09,17,25,31),
                      EscolaTransferencia=null,
                      NomeAluno="Maria Aluno teste",
                      NomeSocialAluno=null,
                      NumeroAlunoChamada=1,
                      ParecerConclusivo=null,
                      PossuiDeficiencia=false,
                      SituacaoMatricula="Ativo",
                      Transferencia_Interna=false,
                      TurmaEscola=null,
                      TurmaRemanejamento=null,
                      TurmaTransferencia=null,
                      NomeResponsavel="ANA DOS TESTES",
                      TipoResponsavel="4",
                      CelularResponsavel="",
                      DataAtualizacaoContato= new DateTime(2018,06,22,19,02,35),
                }
            };
        }
    }
}
