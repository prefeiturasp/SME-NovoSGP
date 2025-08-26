using System;

namespace SME.SGP.Dominio
{
    public static class Perfis
    {
        public readonly static Guid PERFIL_ADMSME = Guid.Parse("5ae1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_ADMCOTIC = Guid.Parse("5be1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_ADMDRE = Guid.Parse("48e1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_AD = Guid.Parse("45E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_ADMUE = Guid.Parse("42e1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_CJ = Guid.Parse("41e1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_CP = Guid.Parse("44E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_CEFAI = Guid.Parse("4be1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_DIRETOR = Guid.Parse("46E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_PROFESSOR = Guid.Parse("40E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_SECRETARIO = Guid.Parse("43E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_SUPERVISOR = Guid.Parse("4EE1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_PROFESSOR_INFANTIL = Guid.Parse("60E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_CJ_INFANTIL = Guid.Parse("61E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_PAEE = Guid.Parse("3de1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_PAP = Guid.Parse("3ee1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_POEI = Guid.Parse("5ce1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_POED = Guid.Parse("5de1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_POSL = Guid.Parse("5ee1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_COMUNICADOS_UE = Guid.Parse("64e1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_PAAI = Guid.Parse("4ae1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_PSICOLOGO_ESCOLAR = Guid.Parse("66e1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_PSICOPEDAGOGO = Guid.Parse("67e1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_ASSISTENTE_SOCIAL = Guid.Parse("68e1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_COORDENADOR_NAAPA = Guid.Parse("65e1e074-37d6-e911-abd6-f81654fe895d");
        public readonly static Guid PERFIL_NAAPA_DRE = Guid.Parse("4CE1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_COORDENADOR_POLO_FORMACAO = Guid.Parse("32C01A4F-B251-4A0F-933D-5B61C8B5DDBF");
        public readonly static Guid PERFIL_ABAE = Guid.Parse("EA741BF4-47EA-486D-8B88-5327521BCFC5");

        public readonly static Guid PERFIL_POA_ALFABETIZACAO = Guid.Parse("2e89cf10-e42b-476f-8673-2dfbeeee3cd0");
        public readonly static Guid PERFIL_POA_LINGUA_PORTUGUESA = Guid.Parse("57a7b9ab-8e61-4093-b692-a0bb1f9f46bd");
        public readonly static Guid PERFIL_POA_MATEMATICA = Guid.Parse("cf181fd4-dd30-47cf-a97d-57e602fd8d10");
        public readonly static Guid PERFIL_POA_HUMANAS = Guid.Parse("2c7ced81-7109-4276-9262-5c56efd8992f");
        public readonly static Guid PERFIL_POA_NATURAIS = Guid.Parse("3104735d-c369-4710-ae64-bca37bc78f3b");
        public readonly static Guid PERFIL_ATE = Guid.Parse("3BE1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_ADM_UE = Guid.Parse("42E1E074-37D6-E911-ABD6-F81654FE895D");

        public readonly static Guid PERFIL_COORDENADOR_PEDAGOGICO_CIEJA = Guid.Parse("76E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_ASSISTENTE_COORDENADOR_GERAL_CIEJA = Guid.Parse("77E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_COORDENADOR_GERAL_CIEJA = Guid.Parse("78E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_SECRETARIO_FORMACAO_CELP = Guid.Parse("79E1E074-37D6-E911-ABD6-F81654FE895D");
        public readonly static Guid PERFIL_ASSISTENTE_PEDAGOGICO_POLO_CELP = Guid.Parse("88E1E074-37D6-E911-ABD6-F81654FE895D");

        public static bool EhPerfilPOA(this Guid source)
            => source == PERFIL_POA_ALFABETIZACAO
                  || source == PERFIL_POA_HUMANAS
                  || source == PERFIL_POA_LINGUA_PORTUGUESA
                  || source == PERFIL_POA_MATEMATICA
                  || source == PERFIL_POA_NATURAIS;

        public static bool EhPerfilPorUe(Guid perfil)
             => perfil == PERFIL_ATE
                    || perfil == PERFIL_AD
                    || perfil == PERFIL_CP
                    || perfil == PERFIL_DIRETOR
                    || perfil == PERFIL_ADM_UE
                    || perfil == PERFIL_ABAE
                    || perfil == PERFIL_PAEE;
    }

}