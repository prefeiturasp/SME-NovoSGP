import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Form
import { Field } from 'formik';

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

  return form ? (
    <InputNomeEstilo>
      <Field
        name={name}
        id={id || name}
        className={`campo ${possuiErro() ? 'is-invalid' : ''} ${className ||
          ''} ${desabilitado ? 'desabilitado' : ''}`}
        component={Input}
        readOnly={desabilitado}
        onBlur={executaOnBlur}
        maxLength={maxlength || 7}
        value={valor}
        placeholder="Digite o RF"
        onKeyDown={onKeyDown}
        onChange={e => {
          form.setFieldValue(name, e.target.value);
          form.setFieldTouched(name, true, true);
          setValor(e.target.value);
          onSelect(e);
        }}
        style={style}
        suffix={botao}
        onPressEnter={e => onSubmitRF(e.target.value)}
      />
      {form && form.touched[name] ? (
        <span className="mensagemErro">{form.errors[name]}</span>
      ) : (
        ''
      )}
    </InputNomeEstilo>
  ) : (
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
  pessoaSelecionada: PropTypes.objectOf(PropTypes.object),
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
