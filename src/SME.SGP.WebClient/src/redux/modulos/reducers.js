import { combineReducers } from 'redux';

import notificacoes from './alertas/reducers';
import bimestres from './planoAnual/reducers';
import usuario from './usuario/reducers';
import navegacao from './navegacao/reducers'

export default combineReducers({
  notificacoes, 
  bimestres,
  usuario,
  navegacao
});
