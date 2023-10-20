using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

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
        
        public static bool EhTelefoneValido(this string telefone)
        {
            return new Regex(@"^\(\d{2}\) \d{4}-\d{4}$").Match(telefone).Success;
        }
        
        public static bool NaoEhTelefoneValido(this string telefone)
        {
            return !EhTelefoneValido(telefone);
        }
        
        public static bool EhEmailValido(this string email)
        {
            return new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").Match(email).Success;
        }
        
        public static bool NaoEhEmailValido(this string email)
        {
            return !EhEmailValido(email);
        }

        public static bool NaoEhCpfValido(this string cpf)
        {
            return !EhCpfValido(cpf);
        }

        public static bool EhCpfValido(this string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto;

            return cpf.EndsWith(digito);
        }
    }
}