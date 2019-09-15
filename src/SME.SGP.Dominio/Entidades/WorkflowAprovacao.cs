using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class WorkflowAprovacao : EntidadeBase
    {
        public WorkflowAprovacao()
        {
            niveis = new List<WorkflowAprovacaoNivel>();
        }

        public int Ano { get; set; }
        public string DreId { get; set; }
        public IEnumerable<WorkflowAprovacaoNivel> Niveis { get { return niveis; } }
        public string NotifacaoMensagem { get; set; }
        public string NotifacaoTitulo { get; set; }
        public NotificacaoCategoria NotificacaoCategoria { get; set; }
        public NotificacaoTipo NotificacaoTipo { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
        private List<WorkflowAprovacaoNivel> niveis { get; set; }

        public void Adicionar(WorkflowAprovacaoNivel nivel)
        {
            if (nivel == null)
                throw new NegocioException("Não é possível incluir um nível sem informação");

            nivel.Workflow = this;

            if (nivel.Id == 0)
                niveis.Add(nivel);
            else if (!niveis.Any(a => a.Id == nivel.Id))
                niveis.Add(nivel);
        }

        public void Adicionar(long nivelId, Notificacao notificacao)
        {
            var nivel = niveis.FirstOrDefault(a => a.Id == nivelId);
            if (nivel == null)
                throw new NegocioException($"Não foi possível localizar o nível de Id {nivelId}");

            nivel.Adicionar(notificacao);
        }

        public void Adicionar(long nivelId, Usuario usuario)
        {
            var nivel = niveis.FirstOrDefault(a => a.Id == nivelId);
            if (nivel == null)
                throw new NegocioException($"Não foi possível localizar o nível de Id {nivelId}");

            nivel.Adicionar(usuario);
        }

        public IEnumerable<WorkflowAprovacaoNivel> ObtemNiveis(long nivel)
        {
            return niveis.Where(a => a.Nivel == nivel)
                .ToList();
        }

        public IEnumerable<WorkflowAprovacaoNivel> ObtemNiveisUnicosEStatus()
        {
            return niveis
                .OrderBy(a => a.Nivel)
                .GroupBy(a => a.Nivel)
                .Select(a => a.FirstOrDefault());
        }

        public int ObtemPrimeiroNivel()
        {
            return niveis
                .OrderBy(a => a.Nivel)
                .Select(a => a.Nivel)
                .FirstOrDefault();
        }
    }
}