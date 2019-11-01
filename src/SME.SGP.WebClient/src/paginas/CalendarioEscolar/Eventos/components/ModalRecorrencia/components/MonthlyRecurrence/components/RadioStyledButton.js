import React from 'react';

// Ant
import { Radio } from 'antd';

// Styles
import styled from 'styled-components';
import { Base } from '~/componentes/colors';

const CustomRadio = styled.div`
  .ant-radio-inner::after {
    background-color: ${Base.Roxo} !important;
  }
  .ant-radio-checked .ant-radio-inner {
    border-color: ${Base.Roxo} !important;
  }
  .ant-radio-wrapper:hover .ant-radio,
  .ant-radio:hover .ant-radio-inner,
  .ant-radio-input:focus + .ant-radio-inner {
    border-color: ${Base.Roxo} !important;
  }
  .ant-radio-group {
    white-space: nowrap;
    margin-bottom: 5px;
  }
`;

// const Error = styled.span`
//   color: ${Base.Vermelho};
// `;

function RadioStyledButton({ className, name, label, value, onChange }) {
  return (
    <CustomRadio className={className}>
      <Radio onClick={onChange} name={name} value={value}>
        {label && label}
      </Radio>
    </CustomRadio>
  );
}

export default RadioStyledButton;
