import React, { useState, useEffect } from 'react';
import { Container } from './auditoria.css';

const Auditoria = ({
  criadoPor,
  criadoEm,
  alteradoPor,
  alteradoEm,
  criadoRf,
  alteradoRf,
  ignorarMarginTop,
}) => {
  const [criado, setCriado] = useState(window.moment());

  const [alterado, setAlterado] = useState(window.moment());

  useEffect(() => {
    if (criadoEm) setCriado(window.moment(criadoEm));
    console.log(criado);
  }, [criadoEm]);

  useEffect(() => {
    if (alteradoEm) setAlterado(window.moment(alteradoEm));
    console.log(alterado);
  }, [alteradoEm]);

  return (
    <Container ignorarMarginTop={ignorarMarginTop}>
      {criadoPor ? (
        <div
          className={`col-xs-12 col-md-12 col-lg-12 d-flex justify-content-start ${
            ignorarMarginTop ? '' : 'mt-2'
          }`}
        >
          INSERIDO por {criadoPor} {criadoRf && `(${criadoRf})`} em{' '}
          {`${criado.format('DD/MM/YYYY')} às ${criado.format('HH:mm')}`}
        </div>
      ) : (
        ''
      )}
      {alteradoPor ? (
        <div className="col-xs-12 col-md-12 col-lg-12 d-flex justify-content-start mt-2">
          ALTERADO por {alteradoPor} {alteradoRf && `(${alteradoRf})`} em{' '}
          {`${alterado.format('DD/MM/YYYY')}  às ${alterado.format('HH:mm')}`}
        </div>
      ) : (
        ''
      )}
    </Container>
  );
};

export default Auditoria;
