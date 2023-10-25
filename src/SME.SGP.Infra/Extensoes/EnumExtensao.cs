﻿using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

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
        
        public static bool EhMenorQueZero(this long valor)
        {
            return valor < 0;
        }
        
        public static bool EhIgualZero(this long valor)
        {
            return valor == 0;
        }
    }
}