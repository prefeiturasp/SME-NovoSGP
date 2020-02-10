import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setExpandirLinha } from '~/redux/modulos/notasConceitos/actions';

import { IconePlusMarcadores } from './avaliacao.css';

const ColunaNotaFinalRegencia = props => {
  const dispatch = useDispatch();

  const expandirLinha = useSelector(
    store => store.notasConceitos.expandirLinha
  );

  const { indexLinha } = props;

  const onClickExpandir = index => {
    expandirLinha[index] = !expandirLinha[index];

    dispatch(setExpandirLinha([...expandirLinha]));
  };

  return (
    <IconePlusMarcadores
      onClick={() => onClickExpandir(indexLinha)}
      className={
        expandirLinha[indexLinha]
          ? 'fas fa-minus-circle '
          : 'fas fa-plus-circle'
      }
    />
  );
};

ColunaNotaFinalRegencia.defaultProps = {
  indexLinha: PropTypes.number,
};

ColunaNotaFinalRegencia.propTypes = {
  indexLinha: null,
};

export default ColunaNotaFinalRegencia;
