import styled from 'styled-components';
import { Base } from '~/componentes';

export const CampoCentralizado = styled.div`
  justify-content: center;
  align-items: center;
  display: flex;
`;

export const CampoAlerta = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: ${Base.Verde};
  border-radius: 4px;
  width: ${props => (props.ehNota ? '87px' : '107px')};
  height: 37px;
  padding-left: 1px;
  .ant-input-number {
    width: 67px !important;
  }
  div {
    height: 35px !important;
  }
  .icone {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 20px;
    i {
      font-size: 8px;
      color: ${Base.Branco};
    }
    cursor: pointer;
  }
`;
