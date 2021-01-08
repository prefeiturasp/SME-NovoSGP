import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setExpandirLinhaAusenciaAluno } from '~/redux/modulos/encaminhamentoAEE/actions';
import { ContainerColunaMotivoAusencia } from './informacoesEscolares.css';

const BtnExpandirAusenciaAluno = props => {
  const dispatch = useDispatch();

  const expandirLinhaAusenciaAluno = useSelector(
    store => store.encaminhamentoAEE.expandirLinhaAusenciaAluno
  );

  const { indexLinha } = props;

  const onClickExpandir = index => {
    let novaLinha = [];
    if (expandirLinhaAusenciaAluno[index]) {
      expandirLinhaAusenciaAluno[index] = false;
      novaLinha = expandirLinhaAusenciaAluno;
    } else {
      novaLinha[index] = true;
    }

    dispatch(setExpandirLinhaAusenciaAluno([...novaLinha]));
  };

  return (
    <ContainerColunaMotivoAusencia
      onClick={() => onClickExpandir(indexLinha)}
      className={
        expandirLinhaAusenciaAluno[indexLinha]
          ? 'fas fa-chevron-up'
          : 'fas fa-chevron-down'
      }
    />
  );
};

BtnExpandirAusenciaAluno.defaultProps = {
  indexLinha: PropTypes.number,
};

BtnExpandirAusenciaAluno.propTypes = {
  indexLinha: null,
};

export default BtnExpandirAusenciaAluno;
