export interface Archivo{
    id: number,
    nombre: string,
    fechaCarga: Date,
    fechaEliminacion: Date | null,
    numeroDescargas: number,
    rutaDocumento: string
}