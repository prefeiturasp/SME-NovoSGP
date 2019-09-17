import styled from 'styled-components';

const EstiloLinhaTempo = styled.div`
  .progressbar-main-title {
    font-size: 10px;
    color: #353535;
    margin-right: -15%;
    margin-left: 2%;
  }

  .progressbar-titles {
    list-style-type: none;
    text-align: center;
    width: 100%;
    display: inline-flex;
    margin-bottom: 5px;
    font-size: 9px;
    li {
      width: ${props => (props.quantidadeItems == 2 ? '50%' : '33.3%')};
    }
  }

  .progressbar {
    li {
      list-style-type: none;
      font-weight: bold;
      font-size: 8px;
      float: left;
      position: relative;
      text-align: center;
      z-index: 0;
      &.active {
        color: #297805;
        &:before {
          font-family: 'Font Awesome 5 Free';
          font-size: 18px;
          content: '\f00c';
          font-weight: 900;
          background-color: #297805;
          color: #fff;
          border-color: #297805;
        }
        &:after {
          background-color: #297805;
          border-color: #297805;
        }
      }
      &:first-child:after {
        content: none;
      }
      &:before {
        content: '';
        width: 30px;
        height: 30px;
        line-height: 30px;
        display: block;
        text-align: center;
        margin: 0 auto 10px auto;
        border-radius: 50%;
        background-color: #a4a4a4;
      }
      &:after {
        content: '';
        position: absolute;
        margin-left: 14px;
        width: 100%;
        height: 5px;
        background-color: #a4a4a4;
        top: 15px;
        left: -50%;
        z-index: -1;
      }
      &.disapproved {
        color: #b40c02;
        &:before {
          font-family: 'Font Awesome 5 Free';
          font-size: 18px;
          content: '\f00d';
          font-weight: 900;
          background-color: #b40c02;
          color: #fff;
        }
        &:after {
          background-color: #b40c02;
        }
      }
      &.cancelled {
        &:before {
          font-family: 'Font Awesome 5 Free';
          font-size: 18px;
          content: '\f057';
          font-weight: 900;
          background-color: #000;
          color: #fff;
        }
      }
    }
  }
`;
export default EstiloLinhaTempo;
