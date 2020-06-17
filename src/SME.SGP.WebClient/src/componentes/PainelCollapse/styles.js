import styled from 'styled-components';

// Ant
import { Collapse } from 'antd';

// Componentes
import { Base } from '~/componentes';

export const IconeEstilizado = styled.i`
  color: ${Base.Verde};
  background: ${Base.Azul};
`;

export const CollapseEstilizado = styled(Collapse)`
  box-shadow: 0px 0px 4px -2px grey;
  //margin: 0.4rem;
`;

export const PainelEstilizado = styled(Collapse.Panel)`
  font-size: 16px;
  font-weight: normal;
  font-stretch: normal;
  font-style: normal;
  line-height: 1.75;
  letter-spacing: 0.15px;

  color: ${Base.CinzaMako};

  .ant-collapse-header {
    padding: 18px 40px 18px 18px !important;
    ${props => props.temBorda && `border-radius: 4px !important`};
    ${props =>
      props.temBorda &&
      `border-left: 7px solid ${
        props.corBorda ? props.corBorda : Base.AzulBreadcrumb
      }`};
  }

  &.ant-collapse-item-active {
    .ant-collapse-header {
      box-shadow: 0px 3px 4px -3px #42474a94;
      padding: 18px 16px;
      padding-right: 40px;
    }
  }
`;
