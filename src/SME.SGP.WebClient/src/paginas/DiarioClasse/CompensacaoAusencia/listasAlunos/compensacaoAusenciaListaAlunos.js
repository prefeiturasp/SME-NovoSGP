import PropTypes from 'prop-types';
import React, { useState } from 'react';

import { BotaoListaAlunos, ColunaBotaoListaAlunos } from '../styles';
import ListaAlunos from './listaAlunos';
import ListaAlunosAusenciasCompensadas from './listaAlunosAusenciasCompensadas';

const CompensacaoAusenciaListaAlunos = props => {
  const { lista, listaAusenciaCompensada } = props;

  const [listaAlunos, setListaAlunos] = useState(lista);
  const [
    listaAlunosAusenciaCompensada,
    setListaAlunosAusenciaCompensada,
  ] = useState(listaAusenciaCompensada);

  const [idsAlunos, setIdsAlunos] = useState([]);
  const [
    idsAlunosAusenciaCompensadas,
    setIdsAlunosAusenciaCompensadas,
  ] = useState([]);

  const obterListaAlunosComIdsSelecionados = (list, ids) => {
    return list.filter(item => ids.find(id => id == item.alunoCodigo));
  };

  const obterListaAlunosSemIdsSelecionados = (list, ids) => {
    return list.filter(item => !ids.find(id => id == item.alunoCodigo));
  };

  const onClickAdicionarAlunos = () => {
    const novaListaAlunosAusenciaCompensada = obterListaAlunosComIdsSelecionados(
      listaAlunos,
      idsAlunos
    );

    const novaListaAlunos = obterListaAlunosSemIdsSelecionados(
      listaAlunos,
      idsAlunos
    );

    setListaAlunos([...novaListaAlunos]);
    setListaAlunosAusenciaCompensada([
      ...novaListaAlunosAusenciaCompensada,
      ...listaAlunosAusenciaCompensada,
    ]);
    setIdsAlunos([]);
  };

  const onClickRemoverAlunos = () => {
    const novaListaAlunos = obterListaAlunosComIdsSelecionados(
      listaAlunosAusenciaCompensada,
      idsAlunosAusenciaCompensadas
    );

    const novaListaAlunosAusenciaCompensada = obterListaAlunosSemIdsSelecionados(
      listaAlunosAusenciaCompensada,
      idsAlunosAusenciaCompensadas
    );

    setListaAlunos([...novaListaAlunos, ...listaAlunos]);
    setListaAlunosAusenciaCompensada([...novaListaAlunosAusenciaCompensada]);
    setIdsAlunosAusenciaCompensadas([]);
  };

  const onSelectRowAlunos = ids => {
    setIdsAlunos(ids);
  };

  const onSelectRowAlunosAusenciaCompensada = ids => {
    setIdsAlunosAusenciaCompensadas(ids);
  };

  return (
    <>
      <div className="col-md-5">
        <ListaAlunos
          lista={listaAlunos}
          onSelectRow={onSelectRowAlunos}
          idsAlunos={idsAlunos}
        />
      </div>
      <ColunaBotaoListaAlunos className="col-md-2">
        <BotaoListaAlunos className="mb-2" onClick={onClickAdicionarAlunos}>
          <i className="fas fa-chevron-right" />
        </BotaoListaAlunos>
        <BotaoListaAlunos onClick={onClickRemoverAlunos}>
          <i className="fas fa-chevron-left" />
        </BotaoListaAlunos>
      </ColunaBotaoListaAlunos>
      <div className="col-md-5">
        <ListaAlunosAusenciasCompensadas
          listaAusenciaCompensada={listaAlunosAusenciaCompensada}
          onSelectRow={onSelectRowAlunosAusenciaCompensada}
          idsAlunosAusenciaCompensadas={idsAlunosAusenciaCompensadas}
        />
      </div>
    </>
  );
};

CompensacaoAusenciaListaAlunos.propTypes = {
  lista: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  listaAusenciaCompensada: PropTypes.oneOfType([
    PropTypes.array,
    PropTypes.object,
  ]),
};

CompensacaoAusenciaListaAlunos.defaultProps = {
  lista: [],
  listaAusenciaCompensada: [],
};

export default CompensacaoAusenciaListaAlunos;
