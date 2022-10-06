using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class FiltroValidarMediaAlunosAtividadeAvaliativaDto
    {
        public FiltroValidarMediaAlunosAtividadeAvaliativaDto(IEnumerable<AtividadeAvaliativa> atividadesAvaliativas, double percentualAlunosInsuficientes, long chaveNotasAvaliacao, IEnumerable<NotaConceito> notasPorAvaliacao, Usuario usuario, string disciplinaId, string hostAplicacao, bool temAbrangenciaUeOuDreOuSme)
        {
            AtividadesAvaliativas = atividadesAvaliativas;
            PercentualAlunosInsuficientes = percentualAlunosInsuficientes;
            NotasPorAvaliacao = notasPorAvaliacao;        
            Usuario = usuario;
            DisciplinaId = disciplinaId;
            HostAplicacao = hostAplicacao;
            ChaveNotasPorAvaliacao = chaveNotasAvaliacao;
            TemAbrangenciaUeOuDreOuSme = temAbrangenciaUeOuDreOuSme;
        }

        public bool TemAbrangenciaUeOuDreOuSme { get; }
        public Usuario Usuario { get; set; }
        public string DisciplinaId { get; set; }
        public IEnumerable<AtividadeAvaliativa> AtividadesAvaliativas { get; set; }
        public double PercentualAlunosInsuficientes { get; set; }
        public long ChaveNotasPorAvaliacao { get; set; }
        public IEnumerable<NotaConceito> NotasPorAvaliacao { get; set; }
        public string HostAplicacao { get; set; }
    }
}
