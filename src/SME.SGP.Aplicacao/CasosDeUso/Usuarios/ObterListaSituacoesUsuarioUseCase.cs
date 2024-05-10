using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaSituacoesUsuarioUseCase : IObterListaSituacoesUsuarioUseCase
    {
        public Task<IEnumerable<KeyValuePair<int, string>>> Executar()
        {
            var situacoes = new List<KeyValuePair<int, string>>();
            foreach (int situacao in Enum.GetValues(typeof(SituacaoUsuario)))
            {
                situacoes.Add(new KeyValuePair<int, string>(situacao, ((SituacaoUsuario)situacao).Name()));
            }

            return Task.FromResult(situacoes.AsEnumerable());
        }
    }
}
