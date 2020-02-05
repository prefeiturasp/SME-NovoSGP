import React, { useEffect, useState } from 'react';
import SelectComponent from '~/componentes/select';

const CampoConceitoFinal = props => {
  // const { onChangeNotaConceito } = props;

  const [conceitoValorAtual, setConceitoValorAtual] = useState();

  const listaConceitos = [
    { valor: 1, descricao: 'P' },
    { valor: 2, descricao: 'S' },
    { valor: 3, descricao: 'NS' },
  ];

  const setarValorNovo = valorNovo => {
    setConceitoValorAtual(valorNovo);
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
      className="tamanho-conceito-final"
    />
  );
};

// CampoConceitoFinal.defaultProps = {
//   nota: {},
//   onChangeNotaConceito: PropTypes.func,
// };

// CampoConceitoFinal.propTypes = {
//   nota: {},
//   onChangeNotaConceito: () => {},
// };

export default CampoConceitoFinal;
