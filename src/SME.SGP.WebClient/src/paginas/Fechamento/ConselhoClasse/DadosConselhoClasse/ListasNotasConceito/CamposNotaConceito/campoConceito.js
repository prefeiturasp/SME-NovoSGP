import PropTypes from 'prop-types';
import React, { useState } from 'react';
import styled from 'styled-components';
import SelectComponent from '~/componentes/select';

export const Combo = styled.div`
  width: 105px;
  height: 35px;
  display: inline-block;
`;

const CampoConceito = props => {
  const { notaPosConselho, listaTiposConceitos } = props;

  const [notaValorAtual, setNotaValorAtual] = useState(notaPosConselho);

  const onChangeConceito = valorNovo => {
    setNotaValorAtual(String(valorNovo));
  };

  return (
    <Combo>
      <SelectComponent
        onChange={onChangeConceito}
        valueOption="id"
        valueText="valor"
        lista={listaTiposConceitos}
        valueSelect={notaValorAtual ? String(notaValorAtual) : ''}
        showSearch
        placeholder="Conceito"
      />
    </Combo>
  );
};

CampoConceito.propTypes = {
  notaPosConselho: PropTypes.oneOfType([PropTypes.any]),
  listaTiposConceitos: PropTypes.oneOfType([PropTypes.array]),
};

CampoConceito.defaultProps = {
  notaPosConselho: '',
  listaTiposConceitos: [],
};

export default CampoConceito;
