import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { AutoComplete, Input } from 'antd';

// Styles
import { InputNomeEstilo } from './styles';

function InputNome({ dataSource, onSelect, onChange, pessoaSelecionada }) {
  const [sugestoes, setSugestoes] = useState([]);
  const [valor, setValor] = useState('');

  useEffect(() => {
    setSugestoes(dataSource);
  }, [dataSource]);

  useEffect(() => {
    setValor(pessoaSelecionada && pessoaSelecionada.nome);
  }, [pessoaSelecionada]);

  useEffect(() => {
    console.log(valor);
  }, [valor]);

  const onChangeValor = selecionado => {
    setValor(selecionado);
  };

  const options =
    sugestoes &&
    sugestoes.map(item => (
      <AutoComplete.Option key={item.rf} value={item.nome}>
        {item.nome}
      </AutoComplete.Option>
    ));

  return (
    <InputNomeEstilo>
      <AutoComplete
        onChange={onChangeValor}
        onSearch={busca => onChange(busca)}
        onSelect={(value, option) => onSelect(option)}
        dataSource={options}
        value={valor || ''}
      >
        <Input
          placeholder="Digite o nome da pessoa"
          prefix={<i className="fa fa-search fa-lg" />}
        />
      </AutoComplete>
    </InputNomeEstilo>
  );
}

InputNome.propTypes = {
  dataSource: PropTypes.oneOfType([PropTypes.array]),
};

InputNome.defaultProps = {
  dataSource: [],
};

export default InputNome;
