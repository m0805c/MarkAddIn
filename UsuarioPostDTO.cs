using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkAddIn
{
    public class UsuarioPostDTO /*DTO que va a enviar la info del usuario*/
    {
        public string? Nombre { get; set; }

        public string? Email { get; set; }

        public string? ObjectIdAzure { get; set; }


    }
}
