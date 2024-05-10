import { AutoComplete } from 'antd';
import { Field } from 'formik';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { Base } from './colors';
import Label from './label';

const { Option } = AutoComplete;

const Container = styled.div`
  .ant-select-arrow {
    color: ${Base.CinzaMako};
  }
  .ant-select-selection--single {
    align-items: center;
    display: flex;
  }

  .ant-select-selection__rendered {
    width: 100%;
  }
  .ant-select-selection__placeholder {
    display: block;
  }
  .ant-select-selection-selected-value {
    font-weight: bold;
  }
  .ant-select-auto-complete.ant-select .ant-input {
    height: 38px;
    line-height: 1.5;
    background: transparent;
    border-width: 1px;
  }

  div[class*='is-invalid'] {
    input {
      border-color: ${Base.Vermelho} !important;
    }
  }
`;

const SelectAutocomplete = ({
  allowClear,
  className,
  filtro,
  id,
  isHandleSearch,
  handleSearch,
  hideLabel,
  label,
  lista,
  name,
  onChange,
  onSelect,
  placeholder,
  textField,
  showList,
  value,
  valueField,
  disabled,
  form,
}) => {
  const [itensFiltrados, setItensFiltrados] = useState(lista);

  const possuiErro = () => {
    return form && form.errors[name] && form.touched[name];
  };

  const obterErros = () => {
    return form && form.touched[name] && form.errors[name] ? (
      <Erro>{form.errors[name]}</Erro>
    ) : (
      ''
    );
  };

  const Erro = styled.span`
    color: ${Base.Vermelho};
  `;

  const filtrar = valueFiltrar => {
    if (valueFiltrar) {
      const textoFiltro = valueFiltrar.toLowerCase();
      const dadosFiltrados = lista
        ? lista.filter(item => filtro(item, textoFiltro))
        : [];
      setItensFiltrados(dadosFiltrados);
    } else {
      setItensFiltrados([]);
    }
  };

  const opcoes = itensFiltrados.map(item => (
    <Option key={item[valueField]}>{item[textField]}</Option>
  ));

  const showDataSource = showList ? lista.map(item => item[textField]) : [];

  const onSearch = isHandleSearch ? handleSearch : filtrar;

  useEffect(() => {
    if (!value) {
      setItensFiltrados([]);
    } else {
      filtrar(value);
    }
  }, [value]);

  const campoComValidacoes = () => (
    <Field
      className={
        form
          ? `overflow-hidden ${possuiErro() ? 'is-invalid' : ''} ${className ||
              ''}`
          : ''
      }
      onSearch={onSearch}
      placeholder={placeholder}
      dataSource={showDataSource}
      name={name}
      allowClear={allowClear}
      onChange={e => {
        form.setFieldValue(name, e || '');
        form.setFieldTouched(name, true, true);
        if (onChange) onChange(e || '');
      }}
      onSelect={e => {
        form.setFieldValue(name, e || '');
        form.setFieldTouched(name, true, true);
        if (onSelect) onSelect(e || '');
      }}
      id={id}
      value={form.values[name] || undefined}
      disabled={disabled}
      notFoundContent="Sem dados"
      component={AutoComplete}
    >
      {opcoes}
    </Field>
  );

  const campoSemValidacoes = () => (
    <AutoComplete
      className={className}
      onSearch={onSearch}
      placeholder={placeholder}
      dataSource={showDataSource}
      name={name}
      allowClear={allowClear}
      onChange={onChange}
      onSelect={onSelect}
      id={id}
      value={value}
      disabled={disabled}
      notFoundContent="Sem dados"
    >
      {opcoes}
    </AutoComplete>
  );

  return (
    <Container>
      {!hideLabel && <Label text={label} control={name} />}
      {form ? campoComValidacoes() : campoSemValidacoes()}
      {form ? obterErros() : ''}
    </Container>
  );
};

SelectAutocomplete.defaultProps = {
  allowClear: true,
  className: '',
  filtro: () => {},
  id: '',
  isHandleSearch: false,
  handleSearch: () => {},
  hideLabel: false,
  label: '',
  lista: [],
  name: '',
  onChange: () => {},
  onSelect: () => {},
  placeholder: '',
  textField: '',
  showList: false,
  value: '',
  valueField: '',
  disabled: false,
};

SelectAutocomplete.propTypes = {
  allowClear: PropTypes.bool,
  className: PropTypes.string,
  filtro: PropTypes.func,
  id: PropTypes.string,
  isHandleSearch: PropTypes.bool,
  handleSearch: PropTypes.func,
  hideLabel: PropTypes.bool,
  label: PropTypes.string,
  lista: PropTypes.instanceOf(Array),
  name: PropTypes.string,
  onChange: PropTypes.func,
  onSelect: PropTypes.func,
  placeholder: PropTypes.string,
  textField: PropTypes.string,
  showList: PropTypes.bool,
  value: PropTypes.string,
  valueField: PropTypes.string,
};

export default SelectAutocomplete;
