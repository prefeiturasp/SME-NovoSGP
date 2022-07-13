using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class FiltroValidarMediaAlunosAtividadeAvaliativaDto
    {
        public FiltroValidarMediaAlunosAtividadeAvaliativaDto(IEnumerable<AtividadeAvaliativa> atividadesAvaliativas, double percentualAlunosInsuficientes, IGrouping<long, NotaConceito> notasPorAvaliacao, Usuario usuario, string disciplinaId, FiltroValidarMediaAlunosDto filtroAtividadeAvaliativa)
        {
            AtividadesAvaliativas = atividadesAvaliativas;
            PercentualAlunosInsuficientes = percentualAlunosInsuficientes;
            NotasPorAvaliacao = notasPorAvaliacao;        
            Usuario = usuario;
            DisciplinaId = disciplinaId;
            FiltroAtividadeAvaliativa = filtroAtividadeAvaliativa;
        }

        public Usuario Usuario { get; set; }
        public string DisciplinaId { get; set; }
        public IEnumerable<AtividadeAvaliativa> AtividadesAvaliativas { get; set; }
        public double PercentualAlunosInsuficientes { get; set; }
        public IGrouping<long, NotaConceito> NotasPorAvaliacao { get; set; }
        public FiltroValidarMediaAlunosDto FiltroAtividadeAvaliativa { get; set; }
    }
}
