import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { AutoComplete, Input } from 'antd';

// Styles
import { InputNomeEstilo } from './styles';

function InputNome({ dataSource, onSelect, onChange, pessoaSelecionada }) {
  const [sugestoes, setSugestoes] = useState([]);
  const [valor, setValor] = useState('');

  const onChangeValor = selecionado => {
    setValor(selecionado);
  };

  useEffect(() => {
    setSugestoes(dataSource);
  }, [dataSource]);

  useEffect(() => {
    setValor(pessoaSelecionada && pessoaSelecionada.professorNome);
  }, [pessoaSelecionada]);

  const options =
    sugestoes &&
    sugestoes.map(item => (
      <AutoComplete.Option key={item.professorRf} value={item.professorNome}>
        {item.professorNome}
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
  pessoaSelecionada: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onSelect: PropTypes.func,
  onChange: PropTypes.func,
};

InputNome.defaultProps = {
  dataSource: [],
  pessoaSelecionada: {},
  onSelect: () => null,
  onChange: () => null,
};

export default InputNome;
