using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake
{
    public class ObterAlunoPorCodigoEAnoQueryHandlerFake : IRequestHandler<ObterAlunoPorCodigoEAnoQuery, AlunoReduzidoDto>
    {
        
        public Task<AlunoReduzidoDto> Handle(ObterAlunoPorCodigoEAnoQuery request, CancellationToken cancellationToken)
        {
            var dataRefencia = DateTimeExtension.HorarioBrasilia();

            var alunoReduzido = new AlunoReduzidoDto()
            {
                Nome = "NOME ALUNO 1",
                NumeroAlunoChamada = 0,
                DataNascimento = new DateTime(DateTime.Now.AddYears(-10).Year, 1, 1).Date,
                DataSituacao = dataRefencia.AddDays(-10),
                CodigoAluno = "1",
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.ReclassificadoSaida,
                Situacao = "RECLASSIFICADO SAIDA",
                TurmaEscola = "",
                CodigoTurma = "1",
            };

            return Task.FromResult(alunoReduzido);
        }

    }
}