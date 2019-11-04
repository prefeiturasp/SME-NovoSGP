import React from 'react';

// Styles
import styled from 'styled-components';

// Components
import SelectComponent from '~/componentes/select';

const SelectWrapper = styled.div`
  .ant-select-selection {
    width: 150px;
  }
`;

function DaysDropDown({ onChange, selected, form }) {
  const items = [
    {
      desc: 'Domingo',
      valor: '0',
    },
    {
      desc: 'Segunda-feira',
      valor: '1',
    },
    {
      desc: 'Terça-feira',
      valor: '2',
    },
    {
      desc: 'Quarta-feira',
      valor: '3',
    },
    {
      desc: 'Quinta-feira',
      valor: '4',
    },
    {
      desc: 'Sexta-feira',
      valor: '5',
    },
    {
      desc: 'Sábado',
      valor: '6',
    },
  ];

  return (
    <SelectWrapper>
      <SelectComponent
        className="fonte-14"
        onChange={onChange}
        lista={items}
        valueOption="valor"
        valueText="desc"
        valueSelect={selected}
        placeholder="Selecione o dia"
        form={form}
        name="diaSemana"
      />
    </SelectWrapper>
  );
}

export default DaysDropDown;
