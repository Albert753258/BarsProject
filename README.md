DROP TABLE "informix".humans3;

CREATE TABLE "informix".humans3(
   id SERIAL NOT NULL,
   last_name CHAR(40) NOT NULL,
   first_name CHAR(40) NOT NULL,
   patronymic CHAR(40) NOT NULL,
   birthday DATE NOT NULL)
EXTENT SIZE 32 NEXT SIZE 32 LOCK MODE ROW;

CREATE INDEX "informix".fio_index ON "informix".humans3(last_name, first_name, patronymic);

CREATE INDEX "informix".birthday_index ON "informix".humans3(birthday);

CREATE INDEX "informix".name_patronymic_birthday_index ON "informix".humans3(first_name, patronymic, birthday);

ALTER TABLE "informix".humans3 ADD CONSTRAINT PRIMARY KEY 
   (id) CONSTRAINT "informix".u108_14;

GRANT select, update, insert, delete, index ON humans3 TO public AS informix;
