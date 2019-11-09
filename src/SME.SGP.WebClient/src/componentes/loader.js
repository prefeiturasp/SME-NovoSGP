import React from 'react';

// Ant
import { Spin } from 'antd';

// Styles
import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes';

const LoaderWrapper = styled.div`
  .ant-spin-dot-item {
    background-color: ${Base.Roxo};
  }
`;

export default function Loader({ children, loading }) {
  return (
    <LoaderWrapper>
      <Spin spinning={loading}>{children}</Spin>
    </LoaderWrapper>
  );
}
