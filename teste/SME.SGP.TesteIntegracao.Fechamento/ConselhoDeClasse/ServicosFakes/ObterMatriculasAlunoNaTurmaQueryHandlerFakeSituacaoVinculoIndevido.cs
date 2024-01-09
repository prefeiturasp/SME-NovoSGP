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

namespace SME.SGP.TesteIntegracao.Fechamento
{
    public class ObterMatriculasAlunoNaTurmaQueryHandlerFakeSituacaoVinculoIndevido : IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterMatriculasAlunoNaTurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    Ano = 1,
                    CelularResponsavel = "99999999999",
                    CodigoAluno = "1",
                    CodigoEscola = "1",
                    CodigoTurma = 2,
                    DataMatricula = DateTimeExtension.HorarioBrasilia().AddMonths(-1),
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddMonths(-12),
                    DataSituacao = DateTimeExtension.HorarioBrasilia().AddDays(-10),
                    EscolaTransferencia = "Escola tranferência",
                    NomeAluno = "Nome Aluno",
                    NomeResponsavel = "Nome responsável",
                    PossuiDeficiencia = false,
                    SituacaoMatricula = "Ativo",
                    TipoResponsavel = "Tipo responsável",
                    Transferencia_Interna = false,
                    TurmaEscola = "1",
                    TurmaRemanejamento = "",
                    TurmaTransferencia = "",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido,
                    DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia().AddDays(-20),
                    NomeSocialAluno = "Nome social",
                    NumeroAlunoChamada = '1',
                    CodigoComponenteCurricular = null,
                    ParecerConclusivo = null
                }
            });
        }
    }
}
