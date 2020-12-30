import React, { useState } from 'react';
import PropTypes from 'prop-types';
import InputNome from '../Localizador/componentes/InputNome';
import Label from '../label';
import servico from './servicos/LocalizadorCriancaServico';
import { erro } from '~/servicos';

const LocalizadorCrianca = ({ label, placeHolder }) => {
  const [dataSource, setDataSource] = useState([]);
  const [criancaSelecionada, setCriancaSelecionada] = useState();

  const onSelectPessoa = () => {};
  const onChangeInput = valor => {
    if (valor.length >= 3) {
      const url = '';
      servico
        .buscarDados(url)
        .then(dados => {
          if (dados) {
            setDataSource(dados);
          }
        })
        .catch(e => {
          setDataSource([]);
          erro(e);
        });
    }
  };

  return (
    <>
      {label ? <Label text={label} /> : ''}
      <InputNome
        dataSource={dataSource}
        onSelect={onSelectPessoa}
        onChange={onChangeInput}
        pessoaSelecionada={criancaSelecionada}
        placeholderNome={placeHolder}
      />
    </>
  );
};
LocalizadorCrianca.propTypes = {
  label: PropTypes.string,
  placeHolder: PropTypes.string,
};

LocalizadorCrianca.defaultProps = {
  label: '',
  placeHolder: '',
};

export default LocalizadorCrianca;
