using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.Bulk;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios.Base
{
    public abstract class RepositorioBaseEntity<T> where T : EntidadeBase
    {
        private readonly IConfiguration configuration;

        public RepositorioBaseEntity(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task InserirVariosAsync(IEnumerable<T> entidades)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ContextoDbSGP>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("SGP_Postgres"));

            using (ContextoDbSGP dbContext = new ContextoDbSGP(optionsBuilder.Options))
            {
                var uploader = new NpgsqlBulkUploader(dbContext);

                await uploader.InsertAsync(entidades);
            }
        }
        public async Task AlterarVariosAsync(IEnumerable<T> entidades)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ContextoDbSGP>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("SGP_Postgres"));

            using (ContextoDbSGP dbContext = new ContextoDbSGP(optionsBuilder.Options))
            {
                var uploader = new NpgsqlBulkUploader(dbContext);

                await uploader.UpdateAsync(entidades);
            }
        }
    }
}