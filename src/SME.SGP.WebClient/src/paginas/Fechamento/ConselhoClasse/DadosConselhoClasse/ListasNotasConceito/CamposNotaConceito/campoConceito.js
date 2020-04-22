import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
// import styled from 'styled-components';
import SelectComponent from '~/componentes/select';

// export const Combo = styled.div`
//   width: 105px;
//   height: 35px;
//   display: inline-block;
// `;

const CampoConceito = props => {
  const { notaPosConselho } = props;

  const listaTiposConceitos = useSelector(
    store => store.conselhoClasse.listaTiposConceitos
  );

  const [notaValorAtual, setNotaValorAtual] = useState(notaPosConselho);

  return (
    // <Combo>
    <SelectComponent
      onChange={setNotaValorAtual}
      valueOption="id"
      valueText="valor"
      lista={listaTiposConceitos}
      valueSelect={notaValorAtual}
      showSearch
      placeholder="Conceito"
    />
    // </Combo>
  );
};

CampoConceito.defaultProps = {
  notaPosConselho: PropTypes.string,
};

CampoConceito.propTypes = {
  notaPosConselho: '',
};

export default CampoConceito;
