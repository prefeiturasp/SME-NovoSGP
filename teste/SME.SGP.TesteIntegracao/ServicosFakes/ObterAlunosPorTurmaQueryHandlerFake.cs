using System;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao
{
    public class ObterAlunosPorTurmaQueryHandlerFake : IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaQuery request, CancellationToken cancellationToken)
        {
           var alunos = new List<AlunoPorTurmaResposta>();
            if (!request.ConsideraInativos)
            {
                alunos = new List<AlunoPorTurmaResposta>
                {
                    new AlunoPorTurmaResposta
                    {
                          Ano=0,
                          CodigoAluno = "11223344",
                          CodigoComponenteCurricular=0,
                          CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                          CodigoTurma=int.Parse(request.TurmaCodigo),
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
                          CelularResponsavel="11961861993",
                          DataAtualizacaoContato= new DateTime(2018,06,22,19,02,35),
                    },
                    new AlunoPorTurmaResposta
                    {
                          Ano=0,
                          CodigoAluno = "6523614",
                          CodigoComponenteCurricular=0,
                          CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                          CodigoTurma=int.Parse(request.TurmaCodigo),
                          DataNascimento=new DateTime(1959,01,16,00,00,00),
                          DataSituacao= new DateTime(2021,11,09,17,25,31),
                          DataMatricula= new DateTime(2021,11,09,17,25,31),
                          EscolaTransferencia=null,
                          NomeAluno="ANA RITA ANDRADE FERREIRA DOS SANTOS",
                          NomeSocialAluno=null,
                          NumeroAlunoChamada=1,
                          ParecerConclusivo=null,
                          PossuiDeficiencia=false,
                          SituacaoMatricula="Ativo",
                          Transferencia_Interna=false,
                          TurmaEscola=null,
                          TurmaRemanejamento=null,
                          TurmaTransferencia=null,
                          NomeResponsavel="ANA RITA ANDRADE FERREIRA DOS SANTOS,",
                          TipoResponsavel="4",
                          CelularResponsavel="11961861993",
                          DataAtualizacaoContato= new DateTime(2018,06,22,19,02,35),
                    }
                };
            }
            else
            {
                alunos = new List<AlunoPorTurmaResposta>
                {
                    new AlunoPorTurmaResposta
                    {
                          Ano=0,
                          CodigoAluno = "6523616",
                          CodigoComponenteCurricular=0,
                          CodigoSituacaoMatricula= SituacaoMatriculaAluno.Desistente,
                          CodigoTurma=int.Parse(request.TurmaCodigo),
                          DataNascimento=new DateTime(1959,01,16,00,00,00),
                          DataSituacao= new DateTime(2021,11,09,17,25,31),
                          DataMatricula= new DateTime(2021,11,09,17,25,31),
                          EscolaTransferencia=null,
                          NomeAluno="ANA RITA ANDRADE FERREIRA DOS SANTOS",
                          NomeSocialAluno=null,
                          NumeroAlunoChamada=1,
                          ParecerConclusivo=null,
                          PossuiDeficiencia=false,
                          SituacaoMatricula="Desistente",
                          Transferencia_Interna=false,
                          TurmaEscola=null,
                          TurmaRemanejamento=null,
                          TurmaTransferencia=null,
                          NomeResponsavel="ANA RITA ANDRADE FERREIRA DOS SANTOS,",
                          TipoResponsavel="4",
                          CelularResponsavel="11961861993",
                          DataAtualizacaoContato= new DateTime(2018,06,22,19,02,35),
                    }
                };
            }
            return Task.FromResult(alunos.Where(x => x.CodigoTurma.ToString() == request.TurmaCodigo));
        }
    }
}
