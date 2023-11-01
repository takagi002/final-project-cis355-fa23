-- Database: UserDb

-- DROP DATABASE IF EXISTS "UserDb";

CREATE DATABASE "UserDb"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.utf8'
    LC_CTYPE = 'en_US.utf8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

-- Table: public.Users

--  Add extension for uuid_generate_v4()
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

DROP TABLE IF EXISTS public."Users";

CREATE TABLE IF NOT EXISTS public."Users"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "FirstName" character varying(255) COLLATE pg_catalog."default",
    "LastName" character varying(255) COLLATE pg_catalog."default",
    "Username" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "Email" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "PasswordHash" bytea NOT NULL,
    "PasswordSalt" bytea NOT NULL,
    "Role" character varying(255) COLLATE pg_catalog."default",
    "IsActive" boolean DEFAULT true,
    "DateCreated" timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    "DateModified" timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    "LastLogin" timestamp without time zone,
    "FailedLoginAttempts" integer DEFAULT 0,
    CONSTRAINT "Users_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Users_Email_key" UNIQUE ("Email"),
    CONSTRAINT "Users_Username_key" UNIQUE ("Username")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Users"
    OWNER to postgres;