using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nest;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFakes
{
    public class ObterAlunoPorTurmaAlunoCodigoQueryHandlerFakeNAAPA: IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>
    {
        public ObterAlunoPorTurmaAlunoCodigoQueryHandlerFakeNAAPA()
        {}

        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorTurmaAlunoCodigoQuery request,CancellationToken cancellationToken)
        {
            var alunosPorTurmaResposta = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
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

            return await Task.FromResult(alunosPorTurmaResposta.FirstOrDefault(f => f.CodigoAluno.Equals(request.AlunoCodigo)));
        }
    }

}