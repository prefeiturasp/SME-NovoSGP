using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SME.SGP.Dados
{
    /// <summary>
    /// TypeMap customizado que utiliza os mapeamentos definidos nos arquivos
    /// EntityMap para resolver automaticamente colunas do banco para
    /// propriedades da entidade, eliminando a necessidade de aliases
    /// (AS) nas queries SQL manuais.
    /// 
    /// Exemplo: com UeMap mapeando CodigoUe -> ue_id,
    /// a query "SELECT u.ue_id FROM ue u" já preenche a propriedade CodigoUe
    /// sem precisar de "u.ue_id AS CodigoUe".
    /// </summary>
    public sealed class MappedTypeMap : SqlMapper.ITypeMap
    {
        private readonly SqlMapper.ITypeMap _defaultTypeMap;
        private readonly Dictionary<string, string> _columnToProperty;

        public MappedTypeMap(
            Type entityType,
            IEntityMap entityMap)
        {
            _defaultTypeMap =
                new DefaultTypeMap(entityType);

            // Monta o dicionário reverso: coluna_banco -> NomePropriedade
            // Ex: "ue_id" -> "CodigoUe", "dre_id" -> "DreId"
            _columnToProperty = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

            foreach (var mapping in entityMap.ColumnMappings)
            {
                // mapping.Key   = nome da propriedade (CodigoUe)
                // mapping.Value = nome da coluna      (ue_id)
                _columnToProperty[mapping.Value] = mapping.Key;
            }
        }

        public ConstructorInfo FindConstructor(
            string[] names,
            Type[] types)
        {
            return _defaultTypeMap
                .FindConstructor(names, types);
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            return _defaultTypeMap
                .FindExplicitConstructor();
        }

        public SqlMapper.IMemberMap GetConstructorParameter(
            ConstructorInfo constructor,
            string columnName)
        {
            return _defaultTypeMap
                .GetConstructorParameter(
                    constructor,
                    columnName);
        }

        public SqlMapper.IMemberMap GetMember(
            string columnName)
        {
            // 1. Tenta o mapeamento padrão (nome da propriedade bate direto)
            var member = _defaultTypeMap.GetMember(columnName);

            if (member != null)
                return member;

            // 2. Se a coluna é um nome de banco (ex: "ue_id"),
            //    busca no mapeamento reverso qual propriedade corresponde
            if (_columnToProperty.TryGetValue(
                    columnName,
                    out var propertyName))
            {
                member = _defaultTypeMap.GetMember(propertyName);

                if (member != null)
                    return member;
            }

            return null;
        }
    }
}

