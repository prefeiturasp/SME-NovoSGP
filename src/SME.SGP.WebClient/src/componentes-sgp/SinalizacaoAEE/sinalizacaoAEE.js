import React from 'react';
import PropTypes from 'prop-types';
import { Tooltip } from 'antd';

import IconeAEE from '~/recursos/IconeAEE.svg';
import { ImagemEstilizada } from './sinalizacaoAEE.css';

const SinalizacaoAEE = ({ exibirSinalizacao, tituloSinalizacao }) => {
  return (
    <>
      {exibirSinalizacao && (
        <Tooltip title={tituloSinalizacao}>
          <ImagemEstilizada src={IconeAEE} alt="Sinalização AEE" />
        </Tooltip>
      )}
    </>
  );
};

SinalizacaoAEE.defaultProps = {
  exibirSinalizacao: false,
  tituloSinalizacao: 'Criança/Estudante atendida pelo AEE',
};

SinalizacaoAEE.propTypes = {
  exibirSinalizacao: PropTypes.bool,
  tituloSinalizacao: PropTypes.string,
};

export default SinalizacaoAEE;
