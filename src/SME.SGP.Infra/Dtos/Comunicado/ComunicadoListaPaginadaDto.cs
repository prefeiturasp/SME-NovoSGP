using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class ComunicadoListaPaginadaDto
    {
        public ComunicadoListaPaginadaDto()
        {
            Modalidades = new List<Modalidade>();
        }

        public long Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime DataExpiracao { get; set; }
        public int ModalidadeCodigo { get; set; }
        public List<Modalidade> Modalidades { get; set; }
        public string Modalidade 
        {
            get => string.Join(",", Modalidades.Select(c => c.ShortName()));
        }

        public void AdicionarModalidade(int modalidadeCodigo)
        {
            if (!Modalidades.Any(modalidade => (int)modalidade == modalidadeCodigo))
                Modalidades.Add((Modalidade)modalidadeCodigo);
        }
    }
}
