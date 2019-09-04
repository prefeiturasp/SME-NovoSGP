import API from '../api';

const Service = {
  obterBimestre: bimestre => {
    return API.post(Service.urlObterPlanoAnual(), bimestre);
  },
  getMateriasProfessor: async (RF, CodigoTurma) => {
    const requisicao = await API.get(
      Service._getBaseUrlMateriasProfessor(RF, CodigoTurma)
    );

    return requisicao.data.map(req => {
      return {
        codigo: req.codigoComponenteCurricular,
        materia: req.nome,
      };
    });
  },

  getObjetivoseByDisciplinas: async (Ano, disciplinas) => {
    const corpoRequisicao = {
      Ano,
      ComponentesCurricularesIds: disciplinas,
    };

    const requisicao = await API.post(
      Service._getBaseUrlObjetivosFiltro(),
      corpoRequisicao
    );

    return requisicao.data;
  },

  postPlanoAnual: async Bimestres => {
    console.log(Service._getObjetoPostPlanoAnual(Bimestres));
  },

  _getBaseUrlMateriasProfessor: (RF, CodigoTurma) => {
    return `v1/professores/${RF}/turmas/${CodigoTurma}/disciplinas/`;
  },

  _getBaseUrlObjetivosFiltro: () => {
    return `v1/objetivos-aprendizagem`;
  },

  _getBaseUrlSalvarPlanoAnual: () => {
    return `api/v1/planos/anual`;
  },

  urlObterPlanoAnual: () => {
    return `v1/planos/anual/obter`;
  },

  _getObjetoPostPlanoAnual: Bimestres => {
    const BimestresFiltrados = Bimestres.filter(x => x.ehExpandido);

    const ArrayEnviar = [];

    BimestresFiltrados.forEach(bimestre => {
      const temObjetivos =
        bimestre.objetivosAprendizagem &&
        bimestre.objetivosAprendizagem.length > 0;

      const objetivosAprendizagem = temObjetivos
        ? bimestre.objetivosAprendizagem
            .filter(x => x.selected)
            .map(obj => {
              return {
                Id: obj.id,
                IdComponenteCurricular: obj.idComponenteCurricular,
              };
            })
        : [];

      const BimestreDTO = {
        AnoLetivo: bimestre.anoLetivo,
        Bimestre: bimestre.indice,
        Descricao: bimestre.objetivo,
        EscolaId: bimestre.escolaId,
        TurmaId: bimestre.turmaId,
        ObjetivosAprendizagem: objetivosAprendizagem,
      };

      ArrayEnviar.push(BimestreDTO);
    });

    return ArrayEnviar;
  },
};

export default Service;
