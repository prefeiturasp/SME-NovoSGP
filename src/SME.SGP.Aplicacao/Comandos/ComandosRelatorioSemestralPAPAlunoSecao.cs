using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosRelatorioSemestralPAPAlunoSecao : IComandosRelatorioSemestralPAPAlunoSecao
    {
        private readonly IRepositorioRelatorioSemestralPAPAlunoSecao repositorioRelatorioSemestralAlunoSecao;

        public ComandosRelatorioSemestralPAPAlunoSecao(IRepositorioRelatorioSemestralPAPAlunoSecao repositorioRelatorioSemestralAlunoSecao)
        {
            this.repositorioRelatorioSemestralAlunoSecao = repositorioRelatorioSemestralAlunoSecao ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestralAlunoSecao));
        }

        public async Task RemoverAsync(RelatorioSemestralPAPAlunoSecao secaoRelatorioAluno)
            => await repositorioRelatorioSemestralAlunoSecao.RemoverAsync(secaoRelatorioAluno);

        public async Task SalvarAsync(RelatorioSemestralPAPAlunoSecao secaoRelatorioAluno)
            => await repositorioRelatorioSemestralAlunoSecao.SalvarAsync(secaoRelatorioAluno);
    }
}
