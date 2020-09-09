import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import { AutoComplete } from 'antd';
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
`;

const SelectAutocomplete = ({
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
}) => {
  const [itensFiltrados, setItensFiltrados] = useState(lista);

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

  return (
    <Container>
      {!hideLabel && <Label text={label} control={name} />}
      <AutoComplete
        className={className}
        onSearch={onSearch}
        placeholder={placeholder}
        dataSource={showDataSource}
        name={name}
        onChange={onChange}
        onSelect={onSelect}
        id={id}
        value={value}
      >
        {opcoes}
      </AutoComplete>
    </Container>
  );
};

SelectAutocomplete.defaultProps = {
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
};

SelectAutocomplete.propTypes = {
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
