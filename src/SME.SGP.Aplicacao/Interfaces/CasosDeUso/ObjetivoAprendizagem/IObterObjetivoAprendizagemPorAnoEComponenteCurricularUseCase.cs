using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase
    {
        Task<IEnumerable<ObjetivoAprendizagemDto>> Executar(string ano, long componenteCurricularId, bool ensinoEspecial);
    }
}
