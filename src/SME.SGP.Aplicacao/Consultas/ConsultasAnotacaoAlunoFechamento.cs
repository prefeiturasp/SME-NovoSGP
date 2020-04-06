using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAnotacaoAlunoFechamento: IConsultasAnotacaoAlunoFechamento
    {
        private readonly IRepositorioAnotacaoAlunoFechamento repositorio;
        private readonly IServicoEOL servicoEOL;

        public ConsultasAnotacaoAlunoFechamento(IRepositorioAnotacaoAlunoFechamento repositorio
                                            , IServicoEOL servicoEOL)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<AnotacaoAlunoCompletoDto> ObterAnotacaoAluno(string codigoAluno, long fechamentoId, string codigoTurma, int anoLetivo)
        {
            var consultaAnotacaoAluno = repositorio.ObterAnotacaoAlunoPorFechamento(fechamentoId, codigoAluno);
            var dadosAlunos = await servicoEOL.ObterDadosAluno(codigoAluno, anoLetivo);
            if (dadosAlunos == null || !dadosAlunos.Any(c => c.CodigoTurma.ToString() == codigoTurma))
                throw new NegocioException($"Não foram localizados dados do aluno {codigoAluno} na turma {codigoTurma} no EOL para o ano letivo {anoLetivo}");

            var dadosAluno = (AlunoDadosBasicosDto)dadosAlunos.FirstOrDefault(c => c.CodigoTurma.ToString() == codigoTurma);

            var anotacaoAluno = await consultaAnotacaoAluno;
            var anotacaoDto = anotacaoAluno == null ? 
                            new AnotacaoAlunoCompletoDto() { Aluno = dadosAluno } :
                            MapearParaDto(anotacaoAluno, dadosAluno);

            return anotacaoDto;
        }

        public async Task<AnotacaoAlunoFechamento> ObterAnotacaoPorAlunoEFechamento(long fechamentoId, string codigoAluno)
            => await repositorio.ObterAnotacaoAlunoPorFechamento(fechamentoId, codigoAluno);

        private AnotacaoAlunoCompletoDto MapearParaDto(AnotacaoAlunoFechamento anotacaoAluno, AlunoDadosBasicosDto dadosAluno)
            => anotacaoAluno == null ? null :
            new AnotacaoAlunoCompletoDto()
            {
                Id = anotacaoAluno.Id,
                Anotacao = anotacaoAluno.Anotacao,
                Aluno = dadosAluno,

                CriadoEm = anotacaoAluno.CriadoEm,
                CriadoPor = anotacaoAluno.CriadoPor,
                CriadoRF = anotacaoAluno.CriadoRF,
                AlteradoEm = anotacaoAluno.AlteradoEm,
                AlteradoPor = anotacaoAluno.AlteradoPor,
                AlteradoRF = anotacaoAluno.AlteradoRF
            };
    }
}
