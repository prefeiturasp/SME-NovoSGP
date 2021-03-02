import React from 'react';
import PropTypes from 'prop-types';

import SinalizacaoAEE from '../SinalizacaoAEE/sinalizacaoAEE';

const NomeEstudanteLista = ({ nome, exibirSinalizacao, tituloSinalizacao }) => {
  return (
    <div className="d-flex justify-content-between w-100">
      <span className="pr-2">{nome} </span>
      <SinalizacaoAEE
        exibirSinalizacao={exibirSinalizacao}
        tituloSinalizacao={tituloSinalizacao}
      />
    </div>
  );
};

NomeEstudanteLista.defaultProps = {
  nome: '',
  exibirSinalizacao: false,
  tituloSinalizacao: 'Criança/Estudante atendida pelo AEE',
};

NomeEstudanteLista.propTypes = {
  nome: PropTypes.string,
  exibirSinalizacao: PropTypes.bool,
  tituloSinalizacao: PropTypes.string,
};

export default NomeEstudanteLista;
