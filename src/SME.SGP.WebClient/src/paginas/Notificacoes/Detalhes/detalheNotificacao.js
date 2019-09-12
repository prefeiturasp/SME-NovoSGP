import React, { useEffect, useState } from 'react';
import Card from '~/componentes/card';
import { EstiloDetalhe } from './estiloDetalhe';
import api from '~/servicos/api';
import { erros } from '~/servicos/alertas';
import * as cores from '~/componentes/colors';
import Button from '~/componentes/button';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Textarea from '~/componentes/textarea';

const DetalheNotificacao = ({ match }) => {
  const [observacao, setObservacao] = useState('teste');
  const [notificacao, setNotificacao] = useState({
    alteradoEm: '',
    alteradoPor: null,
    criadoEm: '',
    criadoPor: '',
    id: 1,
    mensagem: '',
    mostrarBotaoMarcarComoLido: false,
    mostrarBotoesDeAprovacao: true,
    situacao: '',
    tipo: '',
    titulo: '',
  });

  useEffect(() => {
    const buscaNotificacao = id => {
      api
        .get(`v1/notificacoes/${id}`)
        .then(resposta => setNotificacao(resposta.data))
        .catch(listaErros => erros(listaErros));
    };
    if (match.params.id) {
      buscaNotificacao(match.params.id);
    }
  }, [match.params.id]);

  return (
    <>
      <Cabecalho pagina="Notificações" />
      <Card mtop="mt-2">
        <div className="col-md-12 d-flex justify-content-end pb-3">
          {notificacao.mostrarBotoesDeAprovacao && (
            <>
              <Button
                label="Aceitar"
                color={cores.Colors.Roxo}
                className="mr-2"
                onClick={() => true}
              />
              <Button
                label="Recusar"
                color={cores.Colors.Roxo}
                border
                className="mr-2"
                onClick={() => true}
              />
            </>
          )}
          {notificacao.mostrarBotaoMarcarComoLido && (
            <Button
              label="Marcar como lida"
              color={cores.Colors.Azul}
              border
              className="mr-2"
              onClick={() => true}
            />
          )}
          <Button
            label="Excluir"
            color={cores.Colors.Vermelho}
            border
            className="mr-2"
            border
            onClick={() => true}
          />
        </div>
        <EstiloDetalhe>
          <div className="col-xs-12 col-md-12 col-lg-12">
            <div className="row mg-bottom">
              <div className="col-xs-12 col-md-12 col-lg-2 bg-id">
                <div className="row">
                  <div className="col-xs-12 col-md-12 col-lg-12 text-center">
                    ID
                  </div>
                  <div className="id-notificacao col-xs-12 col-md-12 col-lg-12 text-center">
                    {notificacao.id}
                  </div>
                </div>
              </div>
              <div className="col-xs-12 col-md-12 col-lg-10">
                <div className="row">
                  <div className="col-xs-12 col-md-12 col-lg-12">
                    <div className="notificacao-horario">
                      Notificação automática {notificacao.criadoEm}
                    </div>
                  </div>
                  <div className="col-xs-12 col-md-12 col-lg-12">
                    <div className="row">
                      <div className="col-xs-12 col-md-12 col-lg-4 titulo-coluna">
                        Tipo{' '}
                        <div className="conteudo-coluna">
                          {notificacao.tipo}
                        </div>
                      </div>
                      <div className="col-xs-12 col-md-12 col-lg-6 titulo-coluna">
                        Título
                        <div className="conteudo-coluna">
                          {notificacao.titulo}
                        </div>
                      </div>
                      <div className="col-xs-12 col-md-12 col-lg-2 titulo-coluna">
                        Situação
                        <div className="conteudo-coluna">
                          {notificacao.situacao}
                        </div>
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
              MENSAGEM: {notificacao.mensagem}
            </div>
          </div>
          <div className="row">
            <div className="col-xs-12 col-md-12 col-lg-12 obs">
              <label>Observações</label>
              <Textarea
                value={observacao}
                onChange={setObservacao}
                name="Observacao"
              />
            </div>
          </div>
        </EstiloDetalhe>
      </Card>
    </>
  );
};
export default DetalheNotificacao;
