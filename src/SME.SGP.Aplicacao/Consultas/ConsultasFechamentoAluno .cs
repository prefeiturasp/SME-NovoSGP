using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoAluno : IConsultasFechamentoAluno
    {
        private readonly IRepositorioAnotacaoFechamentoAlunoConsulta repositorioAnotacaoFechamentoAlunoConsulta;
        private readonly IMediator mediator;

        public ConsultasFechamentoAluno(IRepositorioAnotacaoFechamentoAlunoConsulta repositorioAnotacaoFechamentoAlunoConsulta,
                                        IMediator mediator)
        {
            this.repositorioAnotacaoFechamentoAlunoConsulta = repositorioAnotacaoFechamentoAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFechamentoAlunoConsulta));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<FechamentoAlunoCompletoDto> ObterAnotacaoAluno(string codigoAluno, long fechamentoId, string codigoTurma, int anoLetivo)
        {
            var anotacaoAluno = await ObterAnotacaoPorAlunoEFechamento(fechamentoId, codigoAluno);
            var dadosAlunos = await mediator.Send(new ObterDadosAlunosQuery(codigoTurma, anoLetivo, null, true));
            if (dadosAlunos.EhNulo() || !dadosAlunos.Any(da => da.CodigoEOL.Equals(codigoAluno)))
                throw new NegocioException($"Não foram localizados dados do aluno {codigoAluno} na turma {codigoTurma} no EOL para o ano letivo {anoLetivo}");

            var dadosAluno = dadosAlunos.First(da => da.CodigoEOL.Equals(codigoAluno));

            dadosAluno.EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(codigoAluno, anoLetivo));
            dadosAluno.EhMatriculadoTurmaPAP = await BuscarAlunosTurmaPAP(dadosAluno.CodigoEOL, anoLetivo);
            var anotacaoDto = anotacaoAluno.EhNulo() ?
                            new FechamentoAlunoCompletoDto() { Aluno = dadosAluno } :
                            MapearParaDto(anotacaoAluno, dadosAluno);

            return anotacaoDto;
        }

        private async Task<bool> BuscarAlunosTurmaPAP(string alunoCodigo, int anoLetivo)
        {
            var matriculadosTurmaPAP =  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, new []{alunoCodigo}));
            return matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == alunoCodigo);
        }
        public async Task<AnotacaoFechamentoAluno> ObterAnotacaoPorAlunoEFechamento(long fechamentoId, string codigoAluno)
            => await repositorioAnotacaoFechamentoAlunoConsulta.ObterPorFechamentoEAluno(fechamentoId, codigoAluno);

        private static FechamentoAlunoCompletoDto MapearParaDto(AnotacaoFechamentoAluno anotacaoAluno, AlunoDadosBasicosDto dadosAluno)
        {
            if (anotacaoAluno.EhNulo())
                return null;
            else
            {
                var fechamentoAluno = new FechamentoAlunoCompletoDto()
                {
                    Id = anotacaoAluno.Id,
                    Anotacao = anotacaoAluno.Anotacao,
                    Aluno = dadosAluno,
                };

                if (!string.IsNullOrEmpty(anotacaoAluno.Anotacao))
                {
                    fechamentoAluno.CriadoEm = anotacaoAluno.CriadoEm;
                    fechamentoAluno.CriadoPor = anotacaoAluno.CriadoPor;
                    fechamentoAluno.CriadoRF = anotacaoAluno.CriadoRF;
                    fechamentoAluno.AlteradoEm = anotacaoAluno.AlteradoEm;
                    fechamentoAluno.AlteradoPor = anotacaoAluno.AlteradoPor;
                    fechamentoAluno.AlteradoRF = anotacaoAluno.AlteradoRF;
                }

                return fechamentoAluno;
            }
        }
    }
}