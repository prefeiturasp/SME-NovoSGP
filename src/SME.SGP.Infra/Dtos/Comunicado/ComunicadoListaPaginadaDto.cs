using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class ComunicadoListaPaginadaDto
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime DataExpiracao { get; set; }
        public int[] ModalidadeCodigo { get; set; }
        public int[] TipoEscolaCodigo { get; set; }
        public List<Modalidade> Modalidades
        {
            get => ModalidadeCodigo.Length > 0 
                ?
                ModalidadeCodigo.Select(a => (Modalidade)a).ToList()
                :
                default;
        }       

        public string Modalidade
        {
            get => string.Join(", ", Modalidades.Select(c => c.ShortName()));
        }        
    }
}
