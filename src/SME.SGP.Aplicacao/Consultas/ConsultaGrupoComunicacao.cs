using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultaGrupoComunicacao : ConsultasBase, IConsultaGrupoComunicacao
    {
        public ConsultaGrupoComunicacao(
            IContextoAplicacao contextoAplicacao,
            IConsultasPeriodoFechamento consultasPeriodoFechamento) : base(contextoAplicacao)
        {
        }

        public Task<AtividadeAvaliativaCompletaDto> Listar(FiltroGrupoComunicacaoDto filtro)
        {
            throw new NotImplementedException();
        }

        public Task<AtividadeAvaliativaCompletaDto> ObterPorIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}