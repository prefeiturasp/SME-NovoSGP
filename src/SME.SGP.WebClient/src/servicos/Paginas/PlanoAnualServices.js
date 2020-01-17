import API from '../api';
import DisciplinaDTO from '~/dtos/disciplinaDto';

const Service = {
  getDisciplinasProfessor: async (RF, CodigoTurma, turmaPrograma) => {
    const requisicao = await API.get(
      Service._getBaseUrlDisciplinasProfessor(RF, CodigoTurma, turmaPrograma)
    );

    return requisicao.data.map(
      req =>
        new DisciplinaDTO(
          req.codigoComponenteCurricular,
          req.nome,
          req.possuiObjetivos,
          req.regencia
        )
    );
  },

  getDisciplinasProfessorObjetivos: async (
    codigoTurma,
    disciplinaSelecionada,
    turmaPrograma
  ) => {
    const requisicao = await API.get(
      Service._getBaseUrlDisciplinasProfessorObjetivo(
        codigoTurma,
        disciplinaSelecionada,
        turmaPrograma
      )
    );

    return requisicao.data.map(req => {
      return {
        codigo: req.codigoComponenteCurricular,
        materia: req.nome,
        possuiObjetivos: req.possuiObjetivos,
        regencia: req.regencia,
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

  postPlanoAnual: async (Bimestres, codigoDisciplinaPlanoAnual) => {
    const ObjetoEnviar = Service._getObjetoPostPlanoAnual(
      Bimestres,
      codigoDisciplinaPlanoAnual
    );

    const Erros = Service._validarDTO(ObjetoEnviar, Bimestres);

    if (Erros && Erros.length > 0) throw { error: Erros };

    const requisicao = API.post(
      Service._getBaseUrlSalvarPlanoAnual(),
      ObjetoEnviar
    );

    return requisicao
      .then(res => res)
      .catch(res => {
        throw { error: res.response.data.mensagens };
      });
  },

  copiarConteudo: async (PlanoAnualDTO, RF, TurmasDestino) => {
    const requisicao = API.post(Service._getUrlMigrarPlanoAnual(), {
      IdsTurmasDestino: TurmasDestino,
      PlanoAnual: PlanoAnualDTO,
      RFProfessor: RF,
    });

    return requisicao
      .then(res => res)
      .catch(res => {
        const erro =
          res.response && res.response.data.mensagens
            ? res.respose.data.mensagens
            : ['Por favor contate a equipe de suporte'];
        throw { error: erro };
      });
  },

  validarPlanoExistente: FiltroPlanoAnual => {
    const requisicao = API.post(
      Service._getBaseUrlValidarPlanoAnualExistente(),
      FiltroPlanoAnual
    );

    return requisicao
      .then(res => res.data)
      .catch(() => {
        throw {
          error:
            'Não foi possivel realizar a consulta, por favor contate a equipe de suporte',
        };
      });
  },

  _getBaseUrlDisciplinasProfessor: (RF, CodigoTurma, turmaPrograma) => {
    return `v1/professores/${RF}/turmas/${CodigoTurma}/disciplinas?turmaPrograma=${!!turmaPrograma}`;
  },

  _getBaseUrlDisciplinasProfessorObjetivo: (
    codigoTurma,
    disciplinaSelecionada,
    turmaPrograma
  ) => {
    return `v1/professores/turmas/${codigoTurma}/disciplinas/planejamento?codigoDisciplina=${
      disciplinaSelecionada.codigo
    }&regencia=${
      disciplinaSelecionada.regencia
    }&turmaPrograma=${turmaPrograma && turmaPrograma}`;
  },

  _getBaseUrlObjetivosFiltro: () => {
    return `v1/objetivos-aprendizagem`;
  },

  _getBaseUrlSalvarPlanoAnual: () => {
    return `v1/planos/anual`;
  },

  _getUrlMigrarPlanoAnual: () => {
    return 'v1/planos/anual/migrar';
  },

  _getBaseUrlValidarPlanoAnualExistente: () => {
    return 'v1/planos/anual/validar-existente';
  },

  _getObjetoPostPlanoAnual: (Bimestres, codigoDisciplinaPlanoAnual) => {
    const BimestresFiltrados = Bimestres.filter(x => x.ehExpandido);

    const ObjetoEnviar = {
      AnoLetivo: 0,
      EscolaId: 0,
      TurmaId: 0,
      Id: 0,
      ComponenteCurricularEolId: 0,
      Bimestres: [],
    };

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
        Bimestre: bimestre.indice,
        Descricao: bimestre.objetivo,
        ObjetivosAprendizagem: objetivosAprendizagem,
      };

      ObjetoEnviar.AnoLetivo = bimestre.anoLetivo;
      ObjetoEnviar.EscolaId = bimestre.escolaId;
      ObjetoEnviar.TurmaId = bimestre.turmaId;
      ObjetoEnviar.Id = bimestre.id;
      ObjetoEnviar.ComponenteCurricularEolId = codigoDisciplinaPlanoAnual;

      ObjetoEnviar.Bimestres.push(BimestreDTO);
    });

    return ObjetoEnviar;
  },

  _validarDTO: (DTO, Bimestres) => {
    let Erros = [];

    if (
      !DTO.AnoLetivo ||
      DTO.AnoLetivo === '' ||
      typeof DTO.AnoLetivo === 'undefined'
    )
      Erros.push('Ano letivo não informado');

    if (
      !DTO.ComponenteCurricularEolId ||
      DTO.ComponenteCurricularEolId === '' ||
      typeof DTO.ComponenteCurricularEolId === 'undefined'
    )
      Erros.push('Componente curricular do plano anual não informada');

    if (
      !DTO.EscolaId ||
      DTO.EscolaId === '' ||
      typeof DTO.EscolaId === 'undefined'
    )
      Erros.push('Unidade escolar não informada');

    if (
      !DTO.TurmaId ||
      DTO.TurmaId === '' ||
      typeof DTO.TurmaId === 'undefined'
    )
      Erros.push('Turma não informada');

    Erros = Service._validarBimestresDTO(DTO.Bimestres, Erros, Bimestres);

    return Erros;
  },

  _validarBimestresDTO: (Bimestres, Erros, BimestresFront) => {
    const LayoutEspecial = BimestresFront.map(x => x.LayoutEspecial).filter(
      x => x
    );

    Bimestres.forEach(bimestre => {
      if (
        !bimestre.Descricao ||
        bimestre.Descricao === '' ||
        typeof bimestre.Descricao === 'undefined' ||
        bimestre.Descricao === '<p><br></p>'
      )
        Erros.push(`${bimestre.Bimestre}º Bimestre: Descrição não informada`);

      if (
        (!bimestre.ObjetivosAprendizagem ||
          bimestre.ObjetivosAprendizagem.length === 0) &&
        LayoutEspecial.length === 0
      )
        Erros.push(
          `${bimestre.Bimestre}º Bimestre: Nenhum objetivo selecionado`
        );
    });

    return Erros;
  },

  urlObterPlanoAnual: () => {
    return `v1/planos/anual/obter`;
  },

  urlObterBimestreExpandido: () => {
    return `v1/planos/anual/obter/expandido`;
  },

  obterBimestre: bimestre => {
    return API.post(Service.urlObterPlanoAnual(), bimestre);
  },

  obterBimestreExpandido: filtroPlanoAnualExpandidoDto => {
    return API.post(
      Service.urlObterBimestreExpandido(),
      filtroPlanoAnualExpandidoDto
    );
  },
};

export default Service;
