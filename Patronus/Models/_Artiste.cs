﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patronus.Models
{
    public partial class Artiste
    {
        public string FullName
        {
            get { return Nom + Prenom; }
        }
    }
}