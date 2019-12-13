import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import SelectComponent from '~/componentes/select';

const CampoConceito = props => {
  const { nota, onChangeNotaConceito } = props;

  const [conceitoValorAtual, setConceitoValorAtual] = useState();

  const listaConceitos = [
    { valor: 1, descricao: 'P' },
    { valor: 2, descricao: 'S' },
    { valor: 3, descricao: 'NS' },
  ];

  useEffect(() => {
    setConceitoValorAtual(nota.notaConceito);
  }, [nota.notaConceito]);

  const setarValorNovo = valorNovo => {
    setConceitoValorAtual(valorNovo);
    onChangeNotaConceito(valorNovo);
  };

  return (
    <SelectComponent
      onChange={valorNovo => setarValorNovo(valorNovo)}
      valueOption="valor"
      valueText="descricao"
      lista={listaConceitos}
      valueSelect={conceitoValorAtual || undefined}
      showSearch
      placeholder="Conceito"
      className="select-conceitos"
      classNameContainer={
        nota.ausente ? 'aluno-ausente-conceitos' : 'aluno-conceitos'
      }
      disabled={!nota.podeEditar}
    />
  );
};

CampoConceito.defaultProps = {
  nota: {},
  onChangeNotaConceito: PropTypes.func,
  listaConceitos: [],
};

CampoConceito.propTypes = {
  nota: {},
  onChangeNotaConceito: () => {},
  listaConceitos: [],
};

export default CampoConceito;
