ALTER SESSION SET CONTAINER = XEPDB1;

DECLARE
    user_count NUMBER;
BEGIN
    SELECT COUNT(*)
      INTO user_count
      FROM dba_users
     WHERE username = 'LOGISTICS_INVOICE';

    IF user_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE USER logistics_invoice IDENTIFIED BY "LogisticsDev123"';
        EXECUTE IMMEDIATE
            'GRANT CREATE SESSION, CREATE TABLE, CREATE VIEW, CREATE SEQUENCE, ' ||
            'CREATE PROCEDURE, CREATE TRIGGER TO logistics_invoice';
        EXECUTE IMMEDIATE
            'ALTER USER logistics_invoice QUOTA UNLIMITED ON USERS';
    END IF;
END;
/
