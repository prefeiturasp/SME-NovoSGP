using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFrequencia : IServicoFrequencia
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ServicoFrequencia(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new System.ArgumentNullException(nameof(repositorioFrequencia));
        }

        public List<RegistroFrequenciaDto> ObterListaFrequenciaPorAula(long aulaId)
        {
            var listaFrequenciaDto = new List<RegistroFrequenciaDto>();
            var frequencias = repositorioFrequencia.ObterListaFrequenciaPorAula(aulaId);
            if (frequencias != null)
                foreach (var frequencia in frequencias)
                {
                    listaFrequenciaDto.Add(new RegistroFrequenciaDto
                    {
                        CodigoAluno = frequencia.CodigoAluno,
                        NumeroAula = frequencia.NumeroAula,
                        Migrado = frequencia.Migrado,
                        Compareceu = false
                    });
                }
            return listaFrequenciaDto;
        }
    }
}