using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalInformacoesEducacionaisConsulta
    {
        Task<PaginacaoResultadoDto<RegistroInformacoesEducacionaisUeDto>> ObterInformacoesEducacionais(FiltroInformacoesEducacionais filtro);
    }
}
