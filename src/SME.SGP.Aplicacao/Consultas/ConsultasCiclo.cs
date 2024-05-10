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
            var lista = repositorioCiclo.ObterCiclosPorAnoModalidade(filtroCicloDto);            

            if (!lista.Any())
                throw new NegocioException("Não foi possível localizar o ciclo da turma selecionada");

            if (!lista.Any(ciclos => ciclos.Selecionado))
                lista.First().Selecionado = true;

            return lista;
        }

        public CicloDto Selecionar(int ano)
        {
            return repositorioCiclo.ObterCicloPorAno(ano);
        }
    }
}