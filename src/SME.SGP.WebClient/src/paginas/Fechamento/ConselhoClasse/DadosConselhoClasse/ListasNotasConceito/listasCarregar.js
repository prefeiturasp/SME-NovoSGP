import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import ListaBimestre from './ListaBimestre/listaBimestre';
import ListaFinal from './ListaFinal/listaFinal';

const ListasCarregar = props => {
  const { ehFinal, tipoNota, listaTiposConceitos } = props;

  const dadosListasNotasConceitos = useSelector(
    store => store.conselhoClasse.dadosListasNotasConceitos
  );

  const validaExibirLista = () => {
    if (ehFinal) {
      return dadosListasNotasConceitos.map(item => (
        <ListaFinal
          key={shortid.generate()}
          dadosLista={item}
          tipoNota={tipoNota}
          listaTiposConceitos={listaTiposConceitos}
        />
      ));
    }

    return dadosListasNotasConceitos.map(item => (
      <ListaBimestre
        key={shortid.generate()}
        dadosLista={item}
        tipoNota={tipoNota}
        listaTiposConceitos={listaTiposConceitos}
      />
    ));
  };

  return <>{validaExibirLista()}</>;
};

ListasCarregar.propTypes = {
  ehFinal: PropTypes.bool,
  tipoNota: PropTypes.oneOfType([PropTypes.any]),
  listaTiposConceitos: PropTypes.oneOfType([PropTypes.array]),
};

ListasCarregar.defaultProps = {
  ehFinal: false,
  tipoNota: 0,
  listaTiposConceitos: [],
};

export default ListasCarregar;
