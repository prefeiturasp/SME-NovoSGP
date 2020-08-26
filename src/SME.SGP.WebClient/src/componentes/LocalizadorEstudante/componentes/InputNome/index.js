import { AutoComplete, Input } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { InputNomeEstilo } from './styles';

const InputNome = props => {
  const {
    dataSource,
    onSelect,
    onChange,
    pessoaSelecionada,
    desabilitado,
    regexIgnore,
  } = props;

  const [sugestoes, setSugestoes] = useState([]);
  const [valor, setValor] = useState('');

  const onChangeValor = selecionado => {
    if (
      regexIgnore &&
      regexIgnore !== '' &&
      selecionado &&
      selecionado !== ''
    ) {
      selecionado = selecionado.replace(regexIgnore, '');
    }
    setValor(selecionado);
  };

  useEffect(() => {
    setSugestoes(dataSource);
  }, [dataSource]);

  useEffect(() => {
    setValor(pessoaSelecionada && pessoaSelecionada.alunoNome);
  }, [pessoaSelecionada]);

  const options =
    sugestoes &&
    sugestoes.map(item => (
      <AutoComplete.Option key={item.alunoCodigo} value={item.alunoNome}>
        {item.alunoNome}
      </AutoComplete.Option>
    ));

  return (
    <InputNomeEstilo>
      <AutoComplete
        onChange={onChangeValor}
        onSearch={busca => onChange(busca)}
        onSelect={(value, option) => onSelect(option)}
        dataSource={options}
        value={valor}
        disabled={desabilitado}
        allowClear
      >
        <Input
          placeholder="Digite o nome"
          prefix={<i className="fa fa-search fa-lg" />}
          disabled={desabilitado}
          allowClear
        />
      </AutoComplete>
    </InputNomeEstilo>
  );
};

InputNome.propTypes = {
  dataSource: PropTypes.oneOfType([PropTypes.array]),
  pessoaSelecionada: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onSelect: PropTypes.func,
  onChange: PropTypes.func,
  desabilitado: PropTypes.bool,
  regexIgnore: PropTypes.string,
};

InputNome.defaultProps = {
  dataSource: [],
  pessoaSelecionada: {},
  onSelect: {},
  onChange: {},
  desabilitado: false,
  regexIgnore: '',
};

export default InputNome;
