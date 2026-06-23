using System;
using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Atributos
{
    public class GrupoAtributoDto
    {
        public int IdGrupoAtributo { get; set; }
        public string CodigoGrupo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Separador { get; set; } = "*";
        public string? UnidadSufijo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public List<GrupoAtributoItemDto> Items { get; set; } = new();
    }

    public class GrupoAtributoItemDto
    {
        public int IdGrupoAtributoItem { get; set; }
        public int IdGrupoAtributo { get; set; }
        public int IdAtributo { get; set; }
        public string NombreAtributo { get; set; } = string.Empty;
        public string TipoDatoAtributo { get; set; } = string.Empty;
        public int? IdUnidadMedida { get; set; }
        public string? SimboloUnidad { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; }
    }

    public class CreateGrupoAtributoDto
    {
        public string CodigoGrupo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Separador { get; set; } = "*";
        public string? UnidadSufijo { get; set; }
        public bool Activo { get; set; } = true;
    }

    public class UpdateGrupoAtributoDto
    {
        public string CodigoGrupo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Separador { get; set; } = "*";
        public string? UnidadSufijo { get; set; }
        public bool Activo { get; set; }
    }

    public class AddItemToGrupoDto
    {
        public int IdAtributo { get; set; }
        public int Orden { get; set; } = 0;
        public int? IdUnidadMedida { get; set; }
    }

    public class FamiliaGrupoAtributoDto
    {
        public int IdFamiliaGrupoAtributo { get; set; }
        public int IdFamilia { get; set; }
        public string NombreFamilia { get; set; } = string.Empty;
        public int IdGrupoAtributo { get; set; }
        public string NombreGrupo { get; set; } = string.Empty;
        public string Separador { get; set; } = "*";
        public string? UnidadSufijo { get; set; }
        public bool Obligatorio { get; set; }
        public bool Activo { get; set; }
        public List<GrupoAtributoItemDto> Items { get; set; } = new();
    }

    public class CreateFamiliaGrupoAtributoDto
    {
        public int IdGrupoAtributo { get; set; }
        public bool Obligatorio { get; set; } = false;
        public bool Activo { get; set; } = true;
    }
}
