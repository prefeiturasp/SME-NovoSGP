using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasMatrizSaber : IConsultasMatrizSaber
    {
        private readonly IRepositorioMatrizSaber repositorioMatrizSaber;

        public ConsultasMatrizSaber(IRepositorioMatrizSaber repositorioMatrizSaber)
        {
            this.repositorioMatrizSaber = repositorioMatrizSaber ?? throw new System.ArgumentNullException(nameof(repositorioMatrizSaber));
        }

        public IEnumerable<MatrizSaberDto> Listar()
        {
            return MapearParaDto(repositorioMatrizSaber.Listar());
        }

        private IEnumerable<MatrizSaberDto> MapearParaDto(IEnumerable<MatrizSaber> matrizes)
        {
            return matrizes?.Select(m => new MatrizSaberDto()
            {
                Descricao = m.Descricao,
                Id = m.Id
            });
        }
    }
}