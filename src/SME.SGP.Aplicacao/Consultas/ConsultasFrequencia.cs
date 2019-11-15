using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasFrequencia
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ConsultasFrequencia(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public IEnumerable<RegistroFrequenciaDto> Listar(long aulaId)
        {
            var frequencias = repositorioFrequencia.ObterListaFrequenciaPorAula(aulaId);

            return repositorioFrequencia.ObterListaFrequenciaPorAula(aulaId);
        }
    }
}