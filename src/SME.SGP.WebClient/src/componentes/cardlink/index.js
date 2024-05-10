import PropTypes from 'prop-types';
import React from 'react';
import CardBody from '../cardBody';
import { Base } from '../colors';
import LinkRouter from '../linkRouter';
import { CardLine, DivCardLink, LabelCardLink } from './style';

const CardLink = props => {
  const {
    icone,
    label,
    url,
    disabled,
    alt,
    cols,
    className,
    classHidden,
    iconSize,
    minHeight,
  } = props;

  const color = disabled ? Base.CinzaDesabilitado : Base.Roxo;
  const colorActive = disabled ? Base.CinzaDesabilitado : Base.Branco;
  const backgroundActive = disabled ? Base.Branco : Base.Roxo;
  const background = Base.Branco;

  const getCols = () => {
    let retorno = 'col-lg-3';

    if (!cols) return retorno;

    if (cols[0]) retorno = `col-lg-${cols[0]}`;

    if (cols[1]) retorno += ` col-md-${cols[1]}`;

    if (cols[2]) retorno += ` col-sm-${cols[2]}`;

    if (cols[3]) retorno += ` col-xs-${cols[3]}`;

    return retorno;
  };

  return (
    <DivCardLink
      className={`${getCols()} px-2 ${classHidden}`}
      style={{ cursor: `${disabled ? 'not-allowed' : ''}` }}
    >
      <LinkRouter
        className="altura-100"
        to={url}
        alt={disabled ? '' : alt}
        isactive={!disabled}
      >
        <DivCardLink
          className={`${className} altura-100`}
          minHeight={minHeight}
        >
          <CardLine
            className="card altura-100"
            color={color}
            colorActive={colorActive}
            backgroundActive={backgroundActive}
            background={background}
          >
            <CardBody className="text-center altura-100">
              <div className="col-md-12 p-0 text-center">
                <i
                  className={icone}
                  color={color}
                  style={{ fontSize: iconSize }}
                />
                <LabelCardLink>{label}</LabelCardLink>
              </div>
            </CardBody>
          </CardLine>
        </DivCardLink>
      </LinkRouter>
    </DivCardLink>
  );
};

CardLink.defaultProps = {
  icone: 'fa fa-check',
  label: 'Insira a label',
  url: '/',
  disabled: false,
  alt: '',
  cols: [3],
  className: '',
  iconSize: '15px',
  classHidden: '',
  minHeight: '20px',
};

CardLink.propTypes = {
  icone: PropTypes.string,
  label: PropTypes.string,
  url: PropTypes.string,
  disabled: PropTypes.bool,
  alt: PropTypes.string,
  cols: PropTypes.oneOfType([PropTypes.array]),
  className: PropTypes.string,
  iconSize: PropTypes.string,
  classHidden: PropTypes.string,
  minHeight: PropTypes.string,
};

export default CardLink;
