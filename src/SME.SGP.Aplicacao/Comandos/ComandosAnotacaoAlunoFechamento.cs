using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAnotacaoAlunoFechamento: IComandosAnotacaoAlunoFechamento
    {
        private readonly IRepositorioAnotacaoAlunoFechamento repositorio;

        public ComandosAnotacaoAlunoFechamento(IRepositorioAnotacaoAlunoFechamento repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<AuditoriaPersistenciaDto> SalvarAnotacaoAluno(AnotacaoAlunoDto anotacaoAluno)
        {
            var anotacao = await MapearParaEntidade(anotacaoAluno);

            // Excluir anotacao quando enviado string vazia
            if (string.IsNullOrEmpty(anotacaoAluno.Anotacao))
                anotacao.Excluido = true;

            await repositorio.SalvarAsync(anotacao);
            return (AuditoriaPersistenciaDto)anotacao;
        }

        private async Task<AnotacaoAlunoFechamento> MapearParaEntidade(AnotacaoAlunoDto anotacaoAluno)
        {
            var anotacao = await repositorio.ObterAnotacaoAlunoPorFechamento(anotacaoAluno.FechamentoId, anotacaoAluno.CodigoAluno);
            if (anotacao == null)
                anotacao = new AnotacaoAlunoFechamento()
                {
                    FechamentoTurmaDisciplinaId = anotacaoAluno.FechamentoId,
                    AlunoCodigo = anotacaoAluno.CodigoAluno,
                    Anotacao = anotacaoAluno.Anotacao
                };
            else
                anotacao.Anotacao = anotacaoAluno.Anotacao;

            return anotacao;
        }
    }
}
