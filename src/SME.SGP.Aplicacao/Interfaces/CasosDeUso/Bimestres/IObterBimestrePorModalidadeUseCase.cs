using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
   public interface IObterBimestrePorModalidadeUseCase
    {
        Task<List<FiltroBimestreDto>> Executar(bool opcaoTodos, bool opcaoFinal,Modalidade modalidade);
    }
}

