using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SME.SGP.Infra
{
    public static class EnumExtensao
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static Modalidade[] ObterModalidadesTurma(this ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            switch (modalidadeTipoCalendario)
            {
                case ModalidadeTipoCalendario.FundamentalMedio:
                    return new[] { Modalidade.Fundamental, Modalidade.Medio };
                case ModalidadeTipoCalendario.EJA:
                    return new[] { Modalidade.EJA };
                case ModalidadeTipoCalendario.Infantil:
                    return new[] { Modalidade.EducacaoInfantil };
                default:
                    throw new NegocioException("ModalidadeTipoCalendario não implementada.");
            }
        }

        public static Cargo ObterCargoPorPerfil(this PerfilUsuario perfilCodigo)
        {
            switch (perfilCodigo)
            {
                case PerfilUsuario.CP:
                    return Cargo.CP;
                case PerfilUsuario.AD:
                    return Cargo.AD;
                case PerfilUsuario.DIRETOR:
                    return Cargo.Diretor;
                default:
                    throw new NegocioException("Perfil não relacionado com Cargo");
            }
        }

        public static FuncaoExterna ObterFuncaoExternaPorPerfil(this PerfilUsuario perfilCodigo)
        {
            switch (perfilCodigo)
            {
                case PerfilUsuario.CP:
                    return FuncaoExterna.CP;
                case PerfilUsuario.AD:
                    return FuncaoExterna.AD;
                case PerfilUsuario.DIRETOR:
                    return FuncaoExterna.Diretor;
                default:
                    throw new NegocioException("Perfil não relacionado com Função Externa");
            }
        }

        public static FuncaoAtividade ObterFuncaoAtividadePorPerfil(this PerfilUsuario perfilCodigo)
        {
            switch (perfilCodigo)
            {
                case PerfilUsuario.CP:
                    return FuncaoAtividade.COORDERNADOR_PEDAGOGICO_CIEJA;
                case PerfilUsuario.AD:
                    return FuncaoAtividade.ASSISTENTE_COORDERNADOR_GERAL_CIEJA;
                case PerfilUsuario.DIRETOR:
                    return FuncaoAtividade.ASSISTENTE_COORDERNADOR_GERAL_CIEJA;
                default:
                    throw new NegocioException("Perfil não relacionado com Função Atividade");
            }
        }

        public static PerfilUsuario ObterPerfilPorCargo(this Cargo cargoId)
        {
            switch (cargoId)
            {
                case Cargo.CP:
                    return PerfilUsuario.CP;
                case Cargo.AD:
                    return PerfilUsuario.AD;
                case Cargo.Diretor:
                    return PerfilUsuario.DIRETOR;
                case Cargo.Supervisor:
                    return PerfilUsuario.SUPERVISOR;
                case Cargo.SupervisorTecnico:
                    return PerfilUsuario.SUPERVISOR_TECNICO;
                default:
                    throw new NegocioException("Cargo não relacionado a um Perfil");
            }
        }

        public static PerfilUsuario ObterPerfilPorFuncaoExterna(this FuncaoExterna funcaoExternaId)
        {
            switch (funcaoExternaId)
            {
                case FuncaoExterna.CP:
                    return PerfilUsuario.CP;
                case FuncaoExterna.AD:
                    return PerfilUsuario.AD;
                case FuncaoExterna.Diretor:
                    return PerfilUsuario.DIRETOR;
                default:
                    throw new NegocioException("Funcao Externa não relacionada a um Perfil");
            }
        }

        public static PerfilUsuario ObterPerfilPorFuncaoAtividade(this FuncaoAtividade codigoFuncaoAtividade)
        {
            switch (codigoFuncaoAtividade)
            {
                case FuncaoAtividade.COORDERNADOR_PEDAGOGICO_CIEJA:
                    return PerfilUsuario.CP;
                case FuncaoAtividade.ASSISTENTE_COORDERNADOR_GERAL_CIEJA:
                    return PerfilUsuario.AD;
                case FuncaoAtividade.COORDERNADOR_GERAL_CIEJA:
                    return PerfilUsuario.DIRETOR;
                default:
                    throw new NegocioException("Funcao Externa não relacionada a um Perfil");
            }
        }

        public static IEnumerable<EnumeradoRetornoDto> ListarDto<TEnum>()
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new InvalidOperationException();

            return ((TEnum[])Enum.GetValues(typeof(TEnum))).Cast<Enum>().Select(v => new EnumeradoRetornoDto
            {
                Descricao = v.GetAttribute<DisplayAttribute>().Name,
                Id = (int)Convert.ChangeType(v, Enum.GetUnderlyingType(v.GetType()))
            }).ToList();
        }

        public static string Name(this Enum enumValue)
            => enumValue.GetAttribute<DisplayAttribute>().Name;

        public static string ShortName(this Enum enumValue)
            => enumValue.GetAttribute<DisplayAttribute>().ShortName;


        public static string Description(this Enum enumValue)
           => enumValue.GetAttribute<DisplayAttribute>().Description;

        public static string GroupName(this Enum enumValue)
           => enumValue.GetAttribute<DisplayAttribute>().GroupName;


        public static IEnumerable<TEnum> Listar<TEnum>()
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new InvalidOperationException();

            return ((TEnum[])Enum.GetValues(typeof(TEnum)));
        }


        public static Dictionary<Enum,string> ToDictionary<TEnum>()
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new InvalidOperationException();

            return ((TEnum[])Enum.GetValues(typeof(TEnum))).Cast<Enum>().ToDictionary(key => key, value => value.Name());
        }
        
        public static bool EhMaiorQueZero(this long valor)
        {
            return valor > 0;
        }
        
        public static bool EhMaiorQueZero(this int valor)
        {
            return valor > 0;
        }
        
        public static bool EhMenorQueZero(this long valor)
        {
            return valor < 0;
        }
        
        public static bool EhIgualZero(this long valor)
        {
            return valor == 0;
        }

        public static string ObterCaseWhenSQL<TEnum>(string atributoComparacao)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("CASE");
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                var enumName = Enum.GetName(typeof(TEnum), value);
                var enumMemberInfo = typeof(TEnum).GetMember(enumName)[0];
                var displayAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(enumMemberInfo, typeof(DisplayAttribute));
                var shortName = displayAttribute.ShortName;

                sql.AppendLine($"    WHEN {atributoComparacao} = {(int)value} THEN '{shortName}'");
            }
            sql.AppendLine("    ELSE ''");
            sql.AppendLine("END");

            return sql.ToString();
        }

        public static bool EhOpcaoTodos(this long? valor)
        {
            return (!valor.HasValue || valor.Equals(-99) || valor.Equals(0));
        }

        public static string ObterDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = field?.GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? value.ToString();
        }
    }
}