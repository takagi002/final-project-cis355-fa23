CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

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