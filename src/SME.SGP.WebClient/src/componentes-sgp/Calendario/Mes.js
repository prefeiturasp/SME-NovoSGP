import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import { useTransition, animated } from 'react-spring';
import { alternaMes } from '~/redux/modulos/calendarioEscolar/actions';

const MonthChevronIcon = props => {
  const transitions = useTransition(props.isOpen, item => item, {
    from: { display: 'none' },
    enter: { display: 'block' },
    leave: { display: 'none' },
  });

  return transitions.map(({ item, key, props }) =>
    item ? (
      <animated.div style={props} key={key}>
        <i className="fas fa-chevron-down" />
      </animated.div>
    ) : (
      <animated.div style={props} key={key}>
        <i className="fas fa-chevron-right text-white" />
      </animated.div>
    )
  );
};

const Month = props => {
  const { mes } = props;

  const meses = useSelector(state => state.calendarioEscolar.meses);

  const abrirMes = () => {
    alternaMes(mes);
  };

  const month = Object.assign({}, meses[mes]);
  month.style = { backgroundColor: '#F7F9FA' };

  if (month.appointments > 0) month.chevronColor = '#10A3FB';

  if (month.isOpen) {
    month.chevronColor = 'rgba(0,0,0,0)';
    month.className += ' border-bottom-0';
    month.style = {};
  }

  return (
    <div className="col-3 w-100 px-0">
      <div className={month.className}>
        <div
          role="button"
          tabIndex={0}
          className="d-flex align-items-center justify-content-center clickable"
          onClick={abrirMes}
          style={{
            backgroundColor: month.chevronColor,
            height: 75,
            width: 33,
          }}
        >
          <MonthChevronIcon isOpen={month.isOpen} />
        </div>

        <div className="d-flex align-items-center w-100" style={month.style}>
          <div className="w-100 pl-2">{month.name}</div>
          <div className="flex-shrink-1 d-flex align-items-center pr-3">
            <div className="pr-2">{month.appointments}</div>
            <div>
              <i className="far fa-calendar-alt" />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

Month.propTypes = {
  mes: PropTypes.string,
};

Month.defaultProps = {
  mes: '',
};

export default Month;
