import React from 'react';
import Card from '~/componentes/card';
import { EstiloDetalhe } from './estiloDetalhe';

const DetalheNotificacao = () => {
  return (
    <>
      <h3>Notificações</h3>
      <Card>
        <EstiloDetalhe>
          <div className="col-xs-12 col-md-12 col-lg-12">
            <div className="row mg-bottom">
              <div className="col-xs-12 col-md-12 col-lg-2 bg-id">
                <div className="row">
                  <div className="col-xs-12 col-md-12 col-lg-12 text-center">
                    ID
                  </div>
                  <div className="id-notificacao col-xs-12 col-md-12 col-lg-12 text-center">
                    00000010
                  </div>
                </div>
              </div>
              <div className="col-xs-12 col-md-12 col-lg-10">
                <div className="row">
                  <div className="col-xs-12 col-md-12 col-lg-12">
                    <div className="notificacao-horario">
                      Notificação automática 15/08/2019, 09:20
                    </div>
                  </div>
                  <div className="col-xs-12 col-md-12 col-lg-12">
                    <div className="row">
                      <div className="col-xs-12 col-md-12 col-lg-4 titulo-coluna">
                        Tipo <div className="conteudo-coluna">Frequência</div>
                      </div>
                      <div className="col-xs-12 col-md-12 col-lg-6 titulo-coluna">
                        Título <div className="conteudo-coluna">Frequência</div>
                      </div>
                      <div className="col-xs-12 col-md-12 col-lg-2 titulo-coluna">
                        Situação
                        <div className="conteudo-coluna">APROVADO</div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <hr className="mt-hr" />
          <div className="row">
            <div className="col-xs-12 col-md-12 col-lg-12 mensagem">
              MENSAGEM: Lorem ipsum dolor sit amet, consectetur adipiscing elit,
              sed do et eiusmod tempor incididunt ut labore et dolore magna
              aliqua. Ut enim ad mini venia dolor et. Lorem ipsum dolor sit
              amet, consectetur adipiscing elit, sed do et eiusmod tempor
              incididunt ut labore et dolore magna aliqua. Ut enim ad mini venia
              dolor et.
            </div>
          </div>
          <div className="row">
            <div className="col-xs-12 col-md-12 col-lg-12 obs">
              <label>Observações</label>
              <textarea className="form-control" rows="5">
                Lorem Ipsum is simply dummy text of the printing and typesetting
                industry. Lorem Ipsum has been the industry's standard dummy
                text ever since the 1500s, when an unknown printer took a galley
              </textarea>
            </div>
          </div>
        </EstiloDetalhe>
      </Card>
    </>
  );
};
export default DetalheNotificacao;
