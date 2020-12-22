import React, { useState } from 'react';
import InputNome from '../Localizador/componentes/InputNome';

const LocalizadorCrianca = () => {
  const [dataSource, setDataSource] = useState([]);
  const [criancaSelecionada, setCriancaSelecionada] = useState();

  const onSelectCrianca = () => {};
  const onChangeCrianca = () => {};

  return (
    <>
      <InputNome
        dataSource={dataSource}
        onSelect={onSelectCrianca}
        onChange={onChangeCrianca}
        pessoaSelecionada={criancaSelecionada}
        placeholderNome={'Procure pelo nome da criança'}
      />
    </>
  );
};

export default LocalizadorCrianca;
