using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public interface IComandosRelatorioSemestralPAPAlunoSecao
    {
        Task SalvarAsync(RelatorioSemestralPAPAlunoSecao secaoRelatorioAluno);

        Task RemoverAsync(RelatorioSemestralPAPAlunoSecao secaoRelatorioAluno);
    }
}
