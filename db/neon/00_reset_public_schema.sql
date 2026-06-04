-- ============================================================
-- PHARMAFLOW - RESET CONTROLADO DE BASE NEON
-- ============================================================
-- USO:
-- Ejecutar SOLO por Diego antes de recrear la base.
--
-- ADVERTENCIA:
-- Este script elimina todo el schema public, incluyendo tablas,
-- vistas, funciones, triggers, indices y datos.
--
-- No ejecutar si no tienes backup o si la base contiene datos reales
-- que deben conservarse.
-- ============================================================

BEGIN;

DROP SCHEMA IF EXISTS public CASCADE;
CREATE SCHEMA public;

GRANT USAGE ON SCHEMA public TO public;
GRANT CREATE ON SCHEMA public TO public;

COMMIT;

