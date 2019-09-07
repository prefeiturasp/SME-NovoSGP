import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { AutoComplete } from 'antd';
import PropTypes from 'prop-types';
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
  .ant-select-auto-complete.ant-select .ant-select-selection--single {
    height: 38px;
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
`;

const SelectAutocomplete = ({
  lista,
  placeholder,
  textField,
  valueField,
  className,
  label,
  name,
  id,
  onChange,
  onSelect,
}) => {
  useEffect(() => {});

  const [result, setResult] = useState(lista);

  const handleSearch = value => {
    if (value) {
      const textoFiltro = value.toLowerCase();
      const dadosFiltrados = lista.filter(item => {
        return item.supervisorNome.toLowerCase().indexOf(textoFiltro) > -1;
      });
      setResult(dadosFiltrados);
    } else {
      setResult(lista);
    }
  };

  const children = result.map(item => (
    <Option key={item[valueField]}>{item[textField]}</Option>
  ));
  return (
    <Container>
      <Label text={label} control={name} />
      <AutoComplete
        className={className}
        onSearch={handleSearch}
        placeholder={placeholder}
        name={name}
        onChange={onChange}
        onSelect={onSelect}
        id={id}
      >
        {children}
      </AutoComplete>
    </Container>
  );
};
SelectAutocomplete.propTypes = {
  lista: PropTypes.array,
};
export default SelectAutocomplete;
