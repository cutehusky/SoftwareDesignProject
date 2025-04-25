--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4 (Debian 17.4-1.pgdg120+2)
-- Dumped by pg_dump version 17.4 (Debian 17.4-1.pgdg120+2)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Configs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Configs" (
    "Key" text NOT NULL,
    "Value" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Configs" OWNER TO postgres;

--
-- Name: Plugins; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Plugins" (
    "PluginId" uuid NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "Category" text NOT NULL,
    "IsEnabled" boolean NOT NULL,
    "IsPremium" boolean NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT '-infinity'::timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT '-infinity'::timestamp with time zone NOT NULL,
    "Icon" text
);


ALTER TABLE public."Plugins" OWNER TO postgres;

--
-- Name: UserPlugins; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserPlugins" (
    "UserId" uuid NOT NULL,
    "PluginId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT '-infinity'::timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT '-infinity'::timestamp with time zone NOT NULL
);


ALTER TABLE public."UserPlugins" OWNER TO postgres;

--
-- Name: UserRole; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserRole" (
    "Role" integer NOT NULL
);


ALTER TABLE public."UserRole" OWNER TO postgres;

--
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "UserId" uuid NOT NULL,
    "Username" text NOT NULL,
    "HashedPassword" text NOT NULL,
    "UserRole" integer NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT '-infinity'::timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT '-infinity'::timestamp with time zone NOT NULL
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Configs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Configs" ("Key", "Value", "CreatedAt", "UpdatedAt") FROM stdin;
\.


--
-- Data for Name: Plugins; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Plugins" ("PluginId", "Name", "Description", "Category", "IsEnabled", "IsPremium", "CreatedAt", "UpdatedAt", "Icon") FROM stdin;
771eb870-4afe-49d6-b2ce-4ea89cad97df	Bandwidth Calculator	Converts bandwidth to different units, calculate download time	Measurement	t	f	2025-04-17 14:23:58.673345+00	2025-04-17 14:26:17.620555+00	\N
45eb9a07-1a28-4e10-ad55-b12d7413bd95	Chronometer	A chronometer to time things	Measurement	t	f	2025-04-17 14:27:11.102974+00	2025-04-17 14:27:11.10298+00	\N
e1a4c6d4-654e-4c44-b8c3-bd246c87bb71	Temperature Conversion	Convert temperature to different units	Measurement	t	f	2025-04-17 14:28:01.061931+00	2025-04-17 14:28:01.061931+00	\N
024220f8-72f5-4520-955d-0ea26ecdc3af	Generate Password	Randomly generate a strong password	Security	t	f	2025-04-17 14:30:06.498016+00	2025-04-17 14:30:06.498017+00	\N
b9ee980a-af8b-4a80-923e-d5c822f83cc8	Password Test	Check if a password is strong or not	Security	t	f	2025-04-17 14:34:13.410039+00	2025-04-17 14:34:13.410039+00	\N
b73274a0-ca2c-4829-9cbb-a17ff2c6839d	Hide Email	Partially obfuscate a portion of an email address	Security	t	f	2025-04-18 07:38:15.530123+00	2025-04-18 07:38:15.530179+00	\N
2e4eb2bd-5431-420a-8fbb-0204c3bb6796	Git Cheatsheet	Showcase some of the useful git commands	Source	t	f	2025-04-18 07:39:13.376817+00	2025-04-18 07:39:13.376831+00	\N
f271ed95-1975-4401-9053-118404d0917b	Generate QR Code	Generate a QR Code from a string	Source	t	t	2025-04-18 07:40:19.878884+00	2025-04-18 07:40:19.878884+00	\N
3da5c205-3e5d-4de1-a441-ab8b215fae73	DNS Parser	List out IPv4s of a domain	Source	t	t	2025-04-18 07:41:24.681837+00	2025-04-18 07:41:24.681838+00	\N
5c8a3c79-db48-432e-8705-cd1f06cc8bfa	Generate Lorem Ispum	Generate a Lorem Ispum string	String	t	f	2025-04-18 07:42:17.406364+00	2025-04-18 07:42:17.406365+00	\N
37a1eea7-567c-4cac-87eb-cece7dc68d57	Obfuscate String	Obfuscate the start and end portion of a string	String	t	f	2025-04-18 07:42:55.335752+00	2025-04-18 07:42:55.335752+00	\N
705d679e-aee6-4dd7-9a91-6a261f5f77ee	Delete Space	Delete extra spaces from a string	String	t	f	2025-04-18 07:43:24.374838+00	2025-04-18 07:43:24.374839+00	\N
bf990d36-033d-4c9a-b52f-785f316fe2f7	URL Encoder	Encode and decode a url to valid url format	Web	t	f	2025-04-18 07:44:33.556973+00	2025-04-18 07:44:33.556973+00	\N
2e284ab1-aad9-4913-9db6-70fcb2b80d26	Generate Port	Generate a random port number	Web	t	f	2025-04-18 07:45:13.966305+00	2025-04-18 07:45:13.966305+00	\N
26cfe2fd-d31a-4f39-8d85-69386b1736df	URL Parser	Parse a URL and list out its components	Web	t	f	2025-04-18 07:45:55.618402+00	2025-04-18 07:45:55.618402+00	\N
5fa1d505-c776-489b-8560-b27d7a45b98b	String to Base64	Convert a string to base64 and back	Conversion	t	f	2025-04-18 07:49:03.71286+00	2025-04-18 07:49:03.712861+00	\N
dda68063-f056-444f-bc0e-37083517ba98	JWT Parser	Parse a JWT string and display its values	Conversion	t	f	2025-04-18 07:50:32.62878+00	2025-04-18 07:50:32.62878+00	\N
52990b50-978c-44d7-9c49-8832ff2f9be9	XML to JSON	Convert XML to JSON and back	Conversion	t	f	2025-04-18 07:51:24.656778+00	2025-04-18 07:51:24.656778+00	\N
10bba80b-2976-4bd7-a86f-5e97dce50bf4	Hash String	Hash a string and back	Hash	t	t	2025-04-18 07:54:42.437434+00	2025-04-18 07:54:51.26199+00	\N
b33c175c-94a9-4d92-b6f8-b56c6ad450b3	Hash BCrypt	Hash a string using BCrypt	Hash	t	t	2025-04-18 07:56:31.746412+00	2025-04-18 07:56:31.746413+00	\N
c34af0bf-11ad-4c51-bf2b-87d164bdf257	Hash File	Hash a file to base64	Hash	t	t	2025-04-18 07:57:38.560422+00	2025-04-18 07:57:38.560422+00	\N
0ee17453-ae55-4256-999f-6de9d4c27efd	IPv4 Subnet Calculator	Calculates stuff related to IPv4 subnet	Net	t	f	2025-04-18 07:59:11.869748+00	2025-04-18 07:59:11.869748+00	\N
5bc09e33-8c2c-4ec5-8071-6080fe8ebfdf	IPv4 Range Expander	Expand IPv4 range	Net	t	f	2025-04-18 07:59:49.001233+00	2025-04-18 07:59:49.001233+00	\N
e639b911-e8ed-403d-af54-e3f0da740b2a	IPv4 Address Converter	Convert IPv4 to different formats	Net	t	f	2025-04-18 08:00:21.636479+00	2025-04-18 08:00:21.63648+00	\N
1ba34c03-382c-4a7f-a7da-09de2544ae0f	Generate Mock Data	Generate random mock data	Generation	t	f	2025-04-18 08:01:45.940533+00	2025-04-18 08:01:45.940533+00	\N
9badce97-d32f-4215-9f6d-fb340957c238	Generate Token	Generate a random token	Generation	t	f	2025-04-18 08:05:38.840781+00	2025-04-18 08:05:38.840781+00	\N
7eab8161-81c9-41f2-a806-208ad89d0ca7	Generate UUID	Generate a random UUID	Generation	t	f	2025-04-18 08:06:20.574502+00	2025-04-18 08:06:20.574502+00	\N
7c22a439-fe40-4935-934b-e93804484289	Encrypt AES	Encrypt and decrypt using AES	Encrypt	t	t	2025-04-18 08:08:17.466214+00	2025-04-18 08:08:17.466214+00	\N
86562091-5483-4f5f-bc77-df763ad7bf3a	Encrypt RSA	Encrypt and decrypt using RSA	Encrypt	t	t	2025-04-18 08:09:19.875594+00	2025-04-18 08:09:19.875595+00	\N
7d89c6ac-6638-49ee-9012-d4c8a1dc8b7e	Generate RSA keys	Generate encrypt and decrypt RSA keys	Encrypt	t	t	2025-04-18 08:10:14.918181+00	2025-04-18 08:10:14.918181+00	\N
\.


--
-- Data for Name: UserPlugins; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserPlugins" ("UserId", "PluginId", "CreatedAt", "UpdatedAt") FROM stdin;
\.


--
-- Data for Name: UserRole; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserRole" ("Role") FROM stdin;
0
1
2
\.


--
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("UserId", "Username", "HashedPassword", "UserRole", "CreatedAt", "UpdatedAt") FROM stdin;
40bc8a2f-320f-4a5e-96ba-7c722f24a370	admin	$2a$11$EIupDkOCVJJ6dXw4KMnHG.Z.IqeYAFfzzOeXohEVuKKKL4laLCGaC	2	2025-04-13 01:54:06.222154+00	2025-04-13 01:54:06.222196+00
76ce30a7-1222-4e38-aae6-a5f80c3f5504	adam	$2a$11$KV..qM44mJbx/XxXCAY//O.noGefW9AoBRuNDJbIrkoF06.ryctUW	0	2025-04-18 09:05:06.351626+00	2025-04-18 09:05:06.351692+00
01bf43b7-6705-4e83-bde6-25a51a6ea03a	johndow	$2a$11$s0AuEEPPSt8XcHzexrxgBu/6aBd3Qi/GvGGn36XPHmxHtueXqwHzi	1	2025-04-18 09:05:24.586544+00	2025-04-18 09:05:24.586558+00
2e8b6543-1c69-416c-8cb4-e8d140fcc2c3	alexander	$2a$11$qtJe89ZPOaUoGnVSUaRtSOQXSvICDzNjJ0e3QTecJ8aX44sUxa7Mu	0	2025-04-18 09:05:38.830271+00	2025-04-18 09:05:38.830272+00
1c12f854-2e07-49c4-a550-3c43ad1d4b56	gonathong	$2a$11$FCrcKzTZ8yBmEIarYJk.h.IPKPcMXsyKdqq8CR4t1XWfJeMK7N3/q	1	2025-04-18 09:05:57.100123+00	2025-04-18 09:05:57.100123+00
8e6aad86-3389-4032-bcdb-17294b1b05bf	cecilia	$2a$11$Xq9yXr8WC2poeMWmynNe.Oco.02BvFmrGbeYCkfDACXCPMoHbxO9i	1	2025-04-18 09:06:09.763631+00	2025-04-18 09:06:09.763631+00
c6e48aeb-a5af-4938-abf9-945b3fc7723e	susamongus	$2a$11$dghWkbTd7cLgUZP0w3IJgO5U83VwiGFsChPE.AKiiFckuI3NczTKi	0	2025-04-20 03:14:05.344174+00	2025-04-20 03:14:05.34423+00
9bf0e450-52cd-438e-be1d-eabc174be2a5	normal	$2a$11$cpK2tLTM8.0IbhkgWJqLXOJb9RwYgJ3.NaikBqYihmW17DAe80cQy	1	2025-04-24 11:59:06.225074+00	2025-04-24 13:09:58.979272+00
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250327135026_InitialCreate	9.0.3
20250410114013_Update	9.0.3
20250410114359_RemoveStarredInUser	9.0.3
20250410142853_Timestamp	9.0.3
20250418030948_role	9.0.3
20250418031849_config	9.0.3
20250422064437_pluginIcon	9.0.3
\.


--
-- Name: Configs PK_Configs; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Configs"
    ADD CONSTRAINT "PK_Configs" PRIMARY KEY ("Key");


--
-- Name: Plugins PK_Plugins; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Plugins"
    ADD CONSTRAINT "PK_Plugins" PRIMARY KEY ("PluginId");


--
-- Name: UserPlugins PK_UserPlugins; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserPlugins"
    ADD CONSTRAINT "PK_UserPlugins" PRIMARY KEY ("UserId", "PluginId");


--
-- Name: UserRole PK_UserRole; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole"
    ADD CONSTRAINT "PK_UserRole" PRIMARY KEY ("Role");


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("UserId");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Plugins_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Plugins_Name" ON public."Plugins" USING btree ("Name");


--
-- Name: IX_UserPlugins_PluginId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserPlugins_PluginId" ON public."UserPlugins" USING btree ("PluginId");


--
-- Name: IX_Users_UserRole; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Users_UserRole" ON public."Users" USING btree ("UserRole");


--
-- Name: IX_Users_Username; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Users_Username" ON public."Users" USING btree ("Username");


--
-- Name: UserPlugins FK_UserPlugins_Plugins_PluginId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserPlugins"
    ADD CONSTRAINT "FK_UserPlugins_Plugins_PluginId" FOREIGN KEY ("PluginId") REFERENCES public."Plugins"("PluginId") ON DELETE CASCADE;


--
-- Name: UserPlugins FK_UserPlugins_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserPlugins"
    ADD CONSTRAINT "FK_UserPlugins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("UserId") ON DELETE CASCADE;


--
-- Name: Users FK_Users_UserRole_UserRole; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "FK_Users_UserRole_UserRole" FOREIGN KEY ("UserRole") REFERENCES public."UserRole"("Role") ON DELETE RESTRICT;


--
-- PostgreSQL database dump complete
--

