import React, { useLayoutEffect } from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import Button from './button';
import { Colors } from './colors';

const Modal = props => {
  const { title, content, cancelBtn, confirmBtn } = props;

  return (
    <div
      className="modal fade show"
      id={shortid.generate()}
      tabIndex="-1"
      role="dialog"
      aria-hidden="true"
    >
      <div className="modal-dialog modal-dialog-centered" role="document">
        <div className="modal-content">
          <div className="modal-header mx-3 px-0 pb-2">
            <h5 className="modal-title">{title}</h5>
            <Button
              type="button"
              className="close"
              data-dismiss="modal"
              aria-label="Fechar"
            >
              <span aria-hidden="true">×</span>
            </Button>
          </div>
          <div className="modal-body">
            <p>{content}</p>
          </div>
          <div className="modal-footer border-0 mx-3 px-0">
            <Button
              label="Sim"
              type="button"
              onClick={cancelBtn}
              color={Colors.Azul}
              data-dismiss="modal"
              border
            />
            <Button
              label="Não"
              type="button"
              onClick={confirmBtn}
              color={Colors.Azul}
            />
          </div>
        </div>
      </div>
    </div>
  );
};

Modal.propTypes = {
  title: PropTypes.string,
  content: PropTypes.string,
  cancelBtn: PropTypes.func,
  confirmBtn: PropTypes.func,
};

export default Modal;
