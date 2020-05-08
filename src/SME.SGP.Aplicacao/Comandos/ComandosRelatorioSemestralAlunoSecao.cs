using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosRelatorioSemestralAlunoSecao: IComandosRelatorioSemestralAlunoSecao
    {
        private readonly IRepositorioRelatorioSemestralAlunoSecao repositorioRelatorioSemestralAlunoSecao;

        public ComandosRelatorioSemestralAlunoSecao(IRepositorioRelatorioSemestralAlunoSecao repositorioRelatorioSemestralAlunoSecao)
        {
            this.repositorioRelatorioSemestralAlunoSecao = repositorioRelatorioSemestralAlunoSecao ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestralAlunoSecao));
        }

        public async Task SalvarAsync(RelatorioSemestralAlunoSecao secaoRelatorioAluno)
            => await repositorioRelatorioSemestralAlunoSecao.SalvarAsync(secaoRelatorioAluno);
    }
}
