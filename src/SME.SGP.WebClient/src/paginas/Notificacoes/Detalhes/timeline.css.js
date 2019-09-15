import styled from 'styled-components';
const EstiloTimeline = styled.div`
  width: 100%;
  .timeline {
    margin-left: 31px;
    display: flex;
    flex-direction: row;

    .timeline-item {
      margin-right: 20px;
      margin-top: 10px;
      width: 100%;
      text-align: center;
      max-width: 95.5px;
      align-items: center;
      .bar {
        width: 250px;
        height: 3px;
        background-color: #297805;
        position: relative;
        top: -14px;
        left: 55px;
      }
      span {
        font-family: Roboto;
        font-size: 11px;
        font-weight: bold;
        line-height: 1.33;
        letter-spacing: normal;
        color: #a8a8a8;
      }
      .fa-check-circle {
        width: 16.1px;
        height: 23px;
        font-size: 18px;
        margin-top: 5px;
        color: #297805;
      }

      div {
        display: flex;
        justify-items: center;
        justify-content: center;
        max-width: 88px;
      }
      span.timeline-data {
        color: #297805;
        font-size: 10px;
      }
    }
  }
`;

export default EstiloTimeline;
