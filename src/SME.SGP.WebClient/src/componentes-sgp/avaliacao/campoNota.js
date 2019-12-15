import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import CampoNumero from '~/componentes/campoNumero';
import api from '~/servicos/api';
import { erros } from '~/servicos/alertas';

const CampoNota = props => {
  const { nota, onChangeNotaConceito } = props;

  const [notaValorAtual, setNotaValorAtual] = useState();

  useEffect(() => {
    setNotaValorAtual(nota.notaConceito);
  }, [nota.notaConceito]);

  const setarValorNovo = async valorNovo => {
    setNotaValorAtual(valorNovo);
    const notaArredondada = await api
      .get(
        `v1/avaliacoes/${nota.atividadeAvaliativaId}/notas/${Number(
          valorNovo
        )}/arredondamento`
      )
      .catch(e => erros(e));
    setNotaValorAtual(notaArredondada.data || 0.0);
    onChangeNotaConceito(notaArredondada.data || 0.0);
  };

  return (
    <CampoNumero
      onBlur={valorNovo => setarValorNovo(valorNovo.target.value)}
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
