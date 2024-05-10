alter table evento_tipo 
add column if not exists evento_escolaaqui boolean NOT NULL DEFAULT false;

update evento_tipo set evento_escolaaqui = true where id in ( 1,3,4,11,16,17,19,21)
