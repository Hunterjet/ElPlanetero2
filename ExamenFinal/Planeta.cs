using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamenFinal
{
    class Planeta
    {
        double x, y, z;
        double radio;
        int textura;

        public Planeta()
        {
        }

        public Planeta(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        void setXYZ(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        void setTextura(int textura)
        {
            this.textura = textura;
        }
        void setRadio(double radio)
        {
            this.radio = radio;
        }
    }
}
