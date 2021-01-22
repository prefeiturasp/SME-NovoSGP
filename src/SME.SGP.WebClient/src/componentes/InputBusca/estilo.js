import styled from 'styled-components';
import { Base } from '~/componentes';

export const InputEstilo = styled.div`
  .ant-input {
    height: 38px !important;
    padding-left: 40px !important;
  }
  .ant-select-selection__rendered {
    line-height: 38px !important;
  }
  .ant-input-prefix {
    i {
      color: ${Base.CinzaMenu};
    }
  }
  .ant-select-show-search {
    width: 100%;
  }

  .ant-btn {
    padding: 0 !important;
  }
`;
