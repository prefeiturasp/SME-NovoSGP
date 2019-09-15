import styled from 'styled-components';
const EstiloTimeline = styled.div`
  width: 100%;

  .timeline {
    overflow-x: hidden;
    padding: 20px 0;
  }

  .timeline ol {
    width: 100%;
    transition: all 1s;
    margin: 0;
    padding: 0;
    display: flex;
    justify-content: space-between;

    i {
      position: relative;
      top: -12px;
      left: 10px;
      font-size: 18px;
      color: #297805;
      background-color: white;
    }
  }

  .timeline ol li {
    list-style: none;
    position: relative;
    text-align: center;
    flex-grow: 1;
    flex-basis: 0;
    padding: 0 5px;
  }

  .timeline ol li:before {
    content: '';
    position: relative;
    top: 4px;
    width: 100%;
    height: 3px;
    display: block;
    background: #297805;
    margin: 0 auto 5px auto;
  }
  .timeline ol li:last-child:before {
    width: 0px;
  }

  .timeline ol li:first-child:before {
    width: 40% !important;
  }
  .timeline ol li:not(:last-child)::after {
    content: '';
    width: calc(100% - 34px);
    height: 3px;
    display: block;
    background: #297805;
    margin: 0;
    position: absolute;
    top: 4px;
    left: calc(50% + 7px);
    overflow: hidden;
  }
`;

export default EstiloTimeline;
