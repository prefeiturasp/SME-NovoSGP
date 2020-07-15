using Dapper;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComponenteCurricular : RepositorioBase<ComponenteCurricular>, IRepositorioComponenteCurricular
    {
        private const string CHAVE_LISTA_COMPONENTES_CURRICULARES = "lista-componentes-curriculares";
        private readonly IRepositorioCache repositorioCache;

        public RepositorioComponenteCurricular(ISgpContext conexao, IRepositorioCache repositorioCache)
            : base(conexao)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public IEnumerable<ComponenteCurricular> ObterComponentesJuremaPorCodigoEol(long codigoEol)
        {            
            return Listar()
                .Where(cc => cc.CodigoEOL.Equals(codigoEol));
        }

        public override IEnumerable<ComponenteCurricular> Listar()
        {
            var jsonListaComponentesCurriculares = repositorioCache.Obter(CHAVE_LISTA_COMPONENTES_CURRICULARES, true);
            IEnumerable<ComponenteCurricular> listaComponentesCurriculares;

            if (jsonListaComponentesCurriculares == null)
            {                
                listaComponentesCurriculares = base.Listar();
                repositorioCache.SalvarAsync(CHAVE_LISTA_COMPONENTES_CURRICULARES, 
                    JsonConvert.SerializeObject(listaComponentesCurriculares), (int)TimeSpan.FromDays(7).TotalMinutes, true);
            }
            else
                listaComponentesCurriculares = JsonConvert
                    .DeserializeObject<IEnumerable<ComponenteCurricular>>(jsonListaComponentesCurriculares);

            return listaComponentesCurriculares;
        }
    }
}