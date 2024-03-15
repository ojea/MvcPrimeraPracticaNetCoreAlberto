using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcPrimeraPracticaNetCoreAlberto.Data;
using MvcPrimeraPracticaNetCoreAlberto.Models;
using System.Diagnostics.Metrics;

#region
    //ALTER procedure SP_FOTOS_ZAPAS_OUT
    //(@posicion int, @idzapatilla int, @registros int out)
    //as
    //select @registros = count(IDIMAGEN) from IMAGENESZAPASPRACTICA
    //WHERE IDPRODUCTO =@idzapatilla
    //select IDIMAGEN, IDPRODUCTO, IMAGEN from 
    //	(select cast(ROW_NUMBER() OVER(ORDER BY IDIMAGEN) AS INT) AS POSICION 
    //	, IDIMAGEN, IDPRODUCTO, IMAGEN
    //	FROM IMAGENESZAPASPRACTICA
    //	WHERE IDPRODUCTO = @idzapatilla) as QUERY 
    //	WHERE QUERY.POSICION = @posicion
    //go

#endregion


namespace MvcPrimeraPracticaNetCoreAlberto.Respository
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapatillas>> GetZapatillasAsync()
        {
            return await this.context.Zapatillas.ToListAsync();
        }

        public async Task<Zapatillas> ObtenerZapaPorId(int idZapatilla)
        {
            return await this.context.Zapatillas.FirstOrDefaultAsync(z => z.IdProducto == idZapatilla);
        }

        public async Task<PaginacionZapatillas> GetPaginacionZapatillasAsync(int posicion, int idZapatilla)
        {
            PaginacionZapatillas pagZapas = new PaginacionZapatillas();
            pagZapas.Zapatillas = await ObtenerZapaPorId(idZapatilla);

            string sql = "SP_FOTOS_ZAPAS_OUT @posicion, @idzapatilla, @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamZapatilla = new SqlParameter("@idzapatilla", idZapatilla);
            SqlParameter pamRegsitros = new SqlParameter("@registros", -1);

            pamRegsitros.Direction = System.Data.ParameterDirection.Output;

            var consulta = this.context.ImagenesZapas.FromSqlRaw(sql, pamPosicion, pamZapatilla, pamRegsitros);
            List<ImagenesZapas> imagenZapas = await consulta.ToListAsync();
            pagZapas.ImagenZapas = imagenZapas.FirstOrDefault();
            pagZapas.numeroRegistros = (int)pamRegsitros.Value;

            return pagZapas;

        }

        public async Task<int> ObtMaxId()
        {
            if (await this.context.ImagenesZapas.CountAsync() == 0)
                return 1;
            return await this.context.ImagenesZapas.MaxAsync(i => i.IdImagen) + 1;
        }

        public async Task InsertarImagen(List<string> imagenes, int idZapatilla)
        {
            foreach(string imagen in imagenes)
            {
                int idImagen = await ObtMaxId();
                await this.context.ImagenesZapas.AddAsync(
                         new ImagenesZapas
                         {
                             IdImagen = idImagen,
                             IdProducto = idZapatilla,
                             Imagen = imagen
                         }

                    );
                await this.context.SaveChangesAsync();
            }
        }

    }
}
