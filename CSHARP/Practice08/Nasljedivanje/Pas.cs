﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasljedivanje
{
    internal class Pas : Sisavac
    {
        public string Pasmina { get; set; }

        public override string ToString()
        {
            return base.ToString() + " " + Pasmina;
        }
    }
}