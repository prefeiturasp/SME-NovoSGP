import styled from 'styled-components';
import { Base } from '~/componentes';

export const ContainerTabsComponentesCorriculares = styled.div`
  .ant-tabs-nav {
    width: ${props => props.widthAntTabsNav} !important;
  }
`;

export const DescricaoNomeTabComponenteCurricular = styled.span`
  .desc-nome {
    color: ${props =>
      props.tabSelecionada ? `${Base.Roxo}` : `${Base.Verde}`};
  }

  i {
    color: ${Base.Verde} !important;
  }
`;

export const AvisoComponenteCurricular = styled.div`
  margin-bottom: 10px;

  span {
    color: ${Base.Vermelho};
  }
`;
