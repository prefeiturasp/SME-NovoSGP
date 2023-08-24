﻿using Newtonsoft.Json;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class DisciplinaDto
    {
        public long Id { get; set; }
        public long CodigoComponenteCurricular { get; set; }
        public long CodigoTerritorioSaber { get; set; }
        [JsonProperty("codDisciplinaPai")]
        public long? CdComponenteCurricularPai { get; set; }
        public bool Compartilhada { get; set; }
        public string Nome { get; set; }
        public string NomeComponenteInfantil { get; set; }
        public bool PossuiObjetivos { get; set; }
        public bool Regencia { get; set; }
        public bool RegistraFrequencia { get; set; }
        public bool TerritorioSaber { get; set; }
        public bool LancaNota { get; set; }
        public bool ObjetivosAprendizagemOpcionais { get; set; }
        public long GrupoMatrizId { get; set; }
        public string GrupoMatrizNome { get; set; }
        public string TurmaCodigo { get; set; }
        public string Professor { get; set; }
    }

    public static class DisciplinaExtension
    {
        public static long[] ObterCodigos(this IEnumerable<DisciplinaDto> disciplinas)
        {
            var codigosComponentes = disciplinas.Select(cc => cc.CodigoComponenteCurricular).ToList();
            codigosComponentes.AddRange(disciplinas.Select(cc => cc.CodigoTerritorioSaber).Where(cc => cc != 0).ToList());
            return codigosComponentes.ToArray();
        }

        public static void PreencherInformacoesPegagogicasSgp(this List<DisciplinaDto> disciplinas, IEnumerable<InfoComponenteCurricular> componentesCurricularesSgp)
        {
            disciplinas.ForEach(componenteCurricular =>
            {
                var componenteCurricularSgp = componentesCurricularesSgp.Where(cc => cc.Codigo == componenteCurricular.CodigoComponenteCurricular
                                                                                    || (componenteCurricular.CodigoTerritorioSaber != 0 &&
                                                                                        cc.Codigo == componenteCurricular.CodigoTerritorioSaber)).FirstOrDefault();

                if (componenteCurricularSgp != null)
                {
                    componenteCurricular.GrupoMatrizId = componenteCurricularSgp.GrupoMatrizId;
                    componenteCurricular.GrupoMatrizNome = componenteCurricularSgp.GrupoMatrizNome;
                    componenteCurricular.LancaNota = componenteCurricularSgp.LancaNota;
                    componenteCurricular.RegistraFrequencia = componenteCurricularSgp.RegistraFrequencia;
                    componenteCurricular.Compartilhada = componenteCurricularSgp.EhCompartilhada;
                    componenteCurricular.Regencia = componenteCurricularSgp.EhRegencia;
                    componenteCurricular.TerritorioSaber = componenteCurricularSgp.EhTerritorioSaber;

                    var naoEhComponenteTerritorioExtenso = componenteCurricular.CodigoTerritorioSaber == 0 ||
                                                            componenteCurricular.CodigoComponenteCurricular == componenteCurricular.CodigoTerritorioSaber;
                    if (naoEhComponenteTerritorioExtenso)
                    {
                        componenteCurricular.Nome = componenteCurricularSgp.Nome;
                        componenteCurricular.NomeComponenteInfantil = componenteCurricularSgp.NomeComponenteInfantil;
                    }
                }
            });
        }
    }
 }