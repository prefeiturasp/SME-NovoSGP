ALTER TABLE evento ADD evento_pai_id bigint NULL;
ALTER TABLE evento ADD CONSTRAINT evento_fk FOREIGN KEY (evento_pai_id) REFERENCES evento(id);
