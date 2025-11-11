using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkAddIn
{
    public class FirmaRetornadaDTO      /*GET/api/Firmas/{objectIdAzure}*/
    {
            public int IdFirma { get; set; }

            public string ImgBase64 { get; set; }


            public int UsuarioId { get; set; }


    }
}
