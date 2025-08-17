CREATE database productosdb;

-- CONECTAR A LA BASE DE DATOS CREADA

CREATE TABLE public.productos (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    nombre character varying(150) NOT NULL,
    descripcion text,
    categoria character varying(100) NOT NULL,
    url_imagen character varying(512),
    precio numeric(12,2) DEFAULT 0 NOT NULL,
    existencias integer DEFAULT 0 NOT NULL,
    fecha_creacion timestamp with time zone DEFAULT now() NOT NULL,
    fecha_actualizacion timestamp with time zone
);
ALTER TABLE public.productos OWNER TO postgres;

CREATE TABLE public.transacciones (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    producto_id uuid NOT NULL,
    tipo character varying(10) NOT NULL,
    cantidad integer NOT NULL,
    fecha timestamp with time zone DEFAULT now() NOT NULL,
    observacion text,
    precio_unitario numeric(12,2) DEFAULT 0 NOT NULL,
    precio_total numeric(14,2) GENERATED ALWAYS AS (((cantidad)::numeric * precio_unitario)) STORED,
    CONSTRAINT transacciones_cantidad_check CHECK ((cantidad > 0))
);
ALTER TABLE public.transacciones OWNER TO postgres;


COMMENT ON TABLE public.transacciones IS 'Registro de transacciones de inventario';
COMMENT ON COLUMN public.transacciones.tipo IS 'Tipo de transaccion: entrada o salida';
COMMENT ON COLUMN public.transacciones.cantidad IS 'Cantidad de productos afectados';
COMMENT ON COLUMN public.transacciones.observacion IS 'Detalle u observacion de la transaccion';


ALTER TABLE ONLY public.productos
    ADD CONSTRAINT productos_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.transacciones
    ADD CONSTRAINT transacciones_pkey PRIMARY KEY (id);


CREATE INDEX ix_productos_nombre_categoria ON public.productos USING btree (nombre, categoria);
CREATE INDEX ix_transacciones_fecha ON public.transacciones USING btree (fecha);
CREATE INDEX ix_transacciones_producto ON public.transacciones USING btree (producto_id);
CREATE INDEX ix_transacciones_tipo ON public.transacciones USING btree (tipo);
CREATE UNIQUE INDEX ux_productos_nombre_lower ON public.productos USING btree (lower((nombre)::text));

ALTER TABLE ONLY public.transacciones
    ADD CONSTRAINT fk_transacciones_producto FOREIGN KEY (producto_id) REFERENCES public.productos(id) ON UPDATE CASCADE ON DELETE RESTRICT;
