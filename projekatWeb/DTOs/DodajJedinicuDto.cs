using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.DTOs
{
	public class DodajJedinicuDto
	{

        public int IdSmestaja { get; set; }
        public int IdAranzmana { get; set; }

        public DodajJedinicuDto(int idSmestaja, int idAranzmana)
        {
            IdSmestaja = idSmestaja;
            IdAranzmana = idAranzmana;
        }
    }
}