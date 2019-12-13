import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import CampoNumero from '~/componentes/campoNumero';

const CampoNota = props => {
  const { nota, onChangeNotaConceito } = props;

  const [notaValorAtual, setNotaValorAtual] = useState();

  useEffect(() => {
    setNotaValorAtual(nota.notaConceito);
  }, [nota.notaConceito]);

  const setarValorNovo = valorNovo => {
    setNotaValorAtual(valorNovo);
    onChangeNotaConceito(valorNovo);
  };

  return (
    <CampoNumero
      onChange={valorNovo => setarValorNovo(valorNovo)}
      value={notaValorAtual}
      min={0}
      max={10}
      step={0.5}
      placeholder="Nota"
      classNameCampo={`${nota.ausente ? 'aluno-ausente-notas' : 'aluno-notas'}`}
      desabilitado={!nota.podeEditar}
    />
  );
};

CampoNota.defaultProps = {
  onChangeNotaConceito: PropTypes.func,
};

CampoNota.propTypes = {
  onChangeNotaConceito: () => {},
};

export default CampoNota;
