using Dapper;

namespace SME.SGP.Dados.Mapeamentos
{
    public static class RegistrarMapeamentos
    {
        public static void Registrar()
        {
            MapRegistry.Initialize();
            RegistrarDapperTypeMaps();
        }

        /// <summary>
        /// Registra um MappedTypeMap para cada entidade mapeada no Dapper,
        /// permitindo que queries SQL sem alias (AS) resolvam automaticamente
        /// os nomes de colunas do banco para as propriedades da entidade.
        /// 
        /// Exemplo: SELECT u.ue_id FROM ue u  →  preenche Ue.CodigoUe
        /// sem necessidade de "u.ue_id AS CodigoUe".
        /// </summary>
        private static void RegistrarDapperTypeMaps()
        {
            foreach (var map in MapRegistry.GetAllMaps())
            {
                SqlMapper.SetTypeMap(
                    map.EntityType,
                    new MappedTypeMap(
                        map.EntityType,
                        map));
            }
        }
    }
}