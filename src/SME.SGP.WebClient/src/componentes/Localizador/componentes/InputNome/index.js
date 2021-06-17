import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { AutoComplete, Input } from 'antd';

// Styles
import { InputNomeEstilo } from './styles';
import Loader from '~/componentes/loader';

function InputNome({
  dataSource,
  onSelect,
  onChange,
  pessoaSelecionada,
  desabilitado,
  placeholderNome,
  exibirLoader,
}) {
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
    <Loader loading={exibirLoader}>
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
            placeholder={placeholderNome}
            prefix={<i className="fa fa-search fa-lg" />}
            disabled={desabilitado}
            allowClear
          />
        </AutoComplete>
      </InputNomeEstilo>
    </Loader>
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
  desabilitado: PropTypes.bool,
  placeholderNome: PropTypes.string.isRequired,
  exibirLoader: PropTypes.bool,
};

InputNome.defaultProps = {
  dataSource: [],
  pessoaSelecionada: {},
  onSelect: null,
  onChange: null,
  desabilitado: false,
  exibirLoader: false,
};

export default InputNome;
