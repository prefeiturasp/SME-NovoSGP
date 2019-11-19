using SME.SGP.Dominio.Interfaces;
using System.Linq;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAtribuicaoEsporadica
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ServicoAtribuicaoEsporadica(IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public void Salvar(AtribuicaoEsporadica atribuicaoEsporadica, int anoLetivo)
        {
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, ModalidadeTipoCalendario.FundamentalMedio);

            if (tipoCalendario == null)
                throw new NegocioException("Nenhum tipo de calendario para o ano letivo vigente encontrado");

            var periodosEscolares = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Nenhum periodo escolar encontrado para o ano letivo vigente");
        }
    }
}