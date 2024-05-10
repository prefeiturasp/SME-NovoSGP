import styled from 'styled-components';

export const Container = styled.div`
  .btn-imprimir {
    i {
      margin-right: 0px !important;
    }
  }

  .bg-white {
    background: white;
  }

  .ant-tab-nav-33 {
    .ant-tabs-nav {
      width: 33.33% !important;
    }
  }

  .ant-tab-nav-20 {
    .ant-tabs-nav {
      width: 20% !important;
    }
  }

  .ck-editor__editable_inline {
    max-height: 180px !important;

    ::-webkit-scrollbar-track {
      background-color: #f4f4f4 !important;
    }

    ::-webkit-scrollbar {
      width: 9px !important;
      background-color: rgba(229, 237, 244, 0.71) !important;
      border-radius: 2.5px !important;
    }

    ::-webkit-scrollbar-thumb {
      background: #a8a8a8 !important;
      border-radius: 3px !important;
    }
  }
`;
