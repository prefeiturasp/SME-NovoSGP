using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasCiclo : IConsultasCiclo
    {
        private readonly IRepositorioCiclo repositorioCiclo;

        public ConsultasCiclo(IRepositorioCiclo repositorioCiclo)
        {
            this.repositorioCiclo = repositorioCiclo ?? throw new System.ArgumentNullException(nameof(repositorioCiclo));
        }

        public IEnumerable<CicloDto> Listar(FiltroCicloDto filtroCicloDto)
        {
            var ciclos = repositorioCiclo.ObterCiclosPorAnoModalidade(filtroCicloDto);            

            if (!ciclos.Any())
                throw new NegocioException("Não foi possível localizar o ciclo da turma selecionada");

            if (!ciclos.Any(ciclos => ciclos.Selecionado))
                ciclos.First().Selecionado = true;

            return ciclos;
        }

        public CicloDto Selecionar(int ano)
        {
            return repositorioCiclo.ObterCicloPorAno(ano);
        }
    }
}