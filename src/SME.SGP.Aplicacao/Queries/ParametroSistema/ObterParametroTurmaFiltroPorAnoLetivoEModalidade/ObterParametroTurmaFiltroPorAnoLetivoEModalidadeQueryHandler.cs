using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQueryHandler : IRequestHandler<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery, string[]>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;

        public ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQueryHandler(IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public async Task<string[]> Handle(ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery request, CancellationToken cancellationToken)
        {
            var parametro = await repositorioParametrosSistema.ObterParametroPorTipoEAno(TipoParametroSistema.AgrupamentoTurmasFiltro, request.AnoLetivo);
            if (parametro != null && !string.IsNullOrEmpty(parametro.Valor))
                return ObterAnosInfantilParaDesconsiderar(parametro, (int)request.Modalidade);
            else return null;
        }

        private string[] ObterAnosInfantilParaDesconsiderar(ParametrosSistema parametro, int modalidade)
        {
            if (parametro != null && !string.IsNullOrEmpty(parametro.Valor))
            {
                var modalidadesAnos = parametro.Valor.Split(';');
                Dictionary<int, string[]> dictionary = new Dictionary<int, string[]>();
                foreach (string modalidadeAno in modalidadesAnos)
                {
                    if (!string.IsNullOrEmpty(modalidadeAno))
                    {
                        string[] valor = modalidadeAno.Split('=');
                        dictionary.Add(int.Parse(valor[0]), valor[1].Split(','));
                    }
                }
                if (dictionary.ContainsKey(modalidade))
                    return dictionary[modalidade];
            }
            return null;
        }
    }
}
