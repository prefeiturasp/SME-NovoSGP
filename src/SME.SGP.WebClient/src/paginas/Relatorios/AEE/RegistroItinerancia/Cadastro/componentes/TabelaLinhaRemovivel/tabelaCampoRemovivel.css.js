import styled from 'styled-components';
import { Table } from 'antd';

import { Base, Button } from '~/componentes';

export const CustomTable = styled(Table)`
  .ant-table {
    &-thead > tr:first-child > th:first-child,
    &-tbody > tr > td:first-child {
      border-right: 0;
    }

    &-column-title {
      font-weight: bold;
    }

    &-placeholder {
      display: none;
    }
  }
`;

export const BotaoEstilizado = styled(Button)`
  position: absolute !important;
  right: 16px;

  &.btn {
    background: transparent !important;
    padding: 0 !important;
    color: ${Base.CinzaMako};

    &:hover {
      background-color: transparent !important;
      color: inherit !important;
    }

    &-primary {
      &:focus {
        box-shadow: none;
      }
    }

    i {
      margin: 0 !important;
    }
  }
`;

export const LabelEstilizado = styled.span`
  font-weight: bold;
  color: ${Base.CinzaMako};
`;
